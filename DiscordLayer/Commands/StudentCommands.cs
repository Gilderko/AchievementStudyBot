using DiscordLayer.CommandAttributes;
using DiscordLayer.Handlers.Dialogue.SlidingWindow;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class StudentCommands : BaseCommands
    {
        [Command("registerStudent")]
        [Description("Allows the student to register into the bot")]
        public async Task StudentRegister(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbStudent = await dbContext.Students.FindAsync(ctx.Member.Id);

                if (dbStudent != null)
                {
                    await SendErrorMessage("You are already registered as a student", ctx.Channel);
                    await DeleteMessages(messagesToDelete);
                    return;
                }

                var lowestRank = await dbContext.Ranks.OrderBy(rank => rank.PointsRequired).FirstAsync();

                var newStudent = new Student()
                {
                    Id = ctx.Member.Id,
                    AcquiredPoints = 0,
                    OnRegisterName = ctx.Member.Username,
                    CurrentRankId = lowestRank.Id,
                    MyTeacherId = null
                };

                var initialRole = ctx.Guild.GetRole(lowestRank.Id);
                await ctx.Member.GrantRoleAsync(initialRole);

                await dbContext.Students.AddAsync(newStudent);

                await dbContext.SaveChangesAsync();
                messagesToDelete.Add(await SendCorrectMessage("Congratulation, you have been registered as a new student :)", ctx.Channel));
            }

            await DeleteMessages (messagesToDelete);
        }

        [Command("rs")]
        [Description("Short for registerStudent")]
        public async Task StudentRegister2(CommandContext ctx)
        {
            await StudentRegister(ctx);
        }

        [Command("requestAchievement")]
        [Description("Allows the student to request several achievements of his choice (please dont select too many)")]
        [RequireStudent]
        public async Task RequestAchievement(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var discordStudent = ctx.Member;
                var dbStudent = await dbContext.Students.Include(student => student.ReachedAchievements)
                    .ThenInclude(reached => reached.Achievement)
                    .Include(student => student.MyRequests)
                    .FirstAsync(student => student.Id == discordStudent.Id);

                if (!dbStudent.MyTeacherId.HasValue)
                {
                    await SendErrorMessage("You dont have a teacher assigned", ctx.Channel);
                    await DeleteMessages(messagesToDelete);
                    return;
                }

                var allAchievements = await dbContext.Achievements.Where(_ => true).ToListAsync();                

                var availableAchievements = allAchievements
                    .Except(dbStudent.ReachedAchievements.Select(reach => reach.Achievement))
                    .Except(dbStudent.MyRequests.Select(reach => reach.RequestedAchievement))
                    .ToList();

                var pagedDialogue = new PagedDialogue<Achievement>(ctx.Guild, ctx.Client, ctx.Channel, ctx.Member,
                    true, true, $"Would you like to request this achievement {discordStudent.DisplayName}?", false ,availableAchievements);

                (var accepted, var declined, var message) = await pagedDialogue.ExecuteDialogue();
                messagesToDelete.Add(message);

                foreach (var achievAccepted in accepted)
                {
                    var newRequest = new Request()
                    {                   
                        AchievmentId = achievAccepted.Id,
                        StudentId = dbStudent.Id,
                        TeacherId = dbStudent.MyTeacherId.Value
                    };

                    await dbContext.Set<Request>().AddAsync(newRequest);
                }

                await dbContext.SaveChangesAsync();
                messagesToDelete.Add(await SendCorrectMessage($"Requests created in total: {accepted.Count()} requested", ctx.Channel));
            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("ra")]
        [Description("Short for requestAchievement")]
        public async Task RequestAchievement2(CommandContext ctx)
        {
            await RequestAchievement(ctx);
        }

        [Command("profile")]
        [Description("Displays the profile of a student")]
        [RequireStudent]
        public async Task ShowProfile(CommandContext ctx)
        {
            var discordStudent = ctx.Member;
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message};

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbStudent = dbContext.Students
                    .Include(student => student.ReachedAchievements)
                        .ThenInclude(reached => reached.Achievement)                    
                    .First(student => student.Id == discordStudent.Id);

                if (dbStudent == null)
                {
                    await SendErrorMessage("You are not registered yet",ctx.Channel);
                    await DeleteMessages(messagesToDelete);
                    return;
                }

                var discordStudentsTeacherName = dbStudent.MyTeacherId.HasValue ? 
                    (await ctx.Guild.GetMemberAsync(dbStudent.MyTeacherId.Value)).DisplayName : "(no teacher)";

                var profileEmbded = new DiscordEmbedBuilder()
                {
                    Title = $"Profile of: {dbStudent.OnRegisterName}",
                    Color = DiscordColor.Aquamarine     
                };

                string descriptionString = $"Caps: {dbStudent.AcquiredPoints}\n" +
                    $"Teacher: {discordStudentsTeacherName}";
                profileEmbded.Description = descriptionString;

                var achievementsToDisplay = dbStudent.ReachedAchievements.Select(record => record.Achievement).ToList();
                var achievementsDialogue = new PagedDialogue<Achievement>(ctx.Guild, ctx.Client, ctx.Channel, ctx.Member,
                    false, false, $"Achievements: {dbStudent.ReachedAchievements.Count()}", false, achievementsToDisplay);

                messagesToDelete.Add(await SendCorrectMessage(profileEmbded.Build(), ctx.Channel));
                var (_,_,message) = await achievementsDialogue.ExecuteDialogue();
                messagesToDelete.Add(message);
            }

            await DeleteMessages(messagesToDelete);
        }
    }
}
