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
        public async Task StudentRegister(CommandContext ctx)
        {
            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbStudent = await dbContext.Students.FindAsync(ctx.Member.Id);

                if (dbStudent != null)
                {
                    await SendErrorMessage("You are already registered as a student", ctx.Channel);
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
                await SendCorrectMessage("Congratulation, you have been registered as a new student :)", ctx.Channel);
            }
        }

        [Command("requestAchievement")]
        [RequireStudent]
        public async Task RequestAchievement(CommandContext ctx)
        {
            using (var dbContext = new PV178StudyBotDbContext())
            {
                var discordStudent = ctx.Member;
                var dbStudent = await dbContext.Students.Include(student => student.ReachedAchievements)
                    .FirstAsync(student => student.Id == discordStudent.Id);

                if (!dbStudent.MyTeacherId.HasValue)
                {
                    await SendErrorMessage("You dont have a teacher assigned", ctx.Channel);
                }

                var allAchievements = await dbContext.Achievements.Where(_ => true).ToListAsync();
                var studentAchievements = new List<Achievement>();

                foreach (var studentAndAchiev in dbStudent.ReachedAchievements)
                {
                    studentAchievements.Add(await dbContext.Achievements.FindAsync(studentAndAchiev.AchievementId));
                }

                var availableAchievements = allAchievements.Except(studentAchievements).ToList();

                var pagedDialogue = new PagedDialogue<Achievement>(ctx.Guild, ctx.Client, ctx.Channel, ctx.Member,
                    true, true, "Would you like to request this achievement?", availableAchievements);

                (var accepted, var declined) = await pagedDialogue.ExecuteDialogue();

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
                await SendCorrectMessage($"Requests created in total: {accepted.Count()} requested", ctx.Channel);
            }
        }

        [Command("profile")]
        [RequireStudent]
        public async Task ShowProfile(CommandContext ctx)
        {
            var discordStudent = ctx.Member;
            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbStudent = dbContext.Students
                    .Include(student => student.ReachedAchievements)
                        .ThenInclude(reached => reached.Achievement)                    
                    .First(student => student.Id == discordStudent.Id);

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
                    false, false, $"Achievements: {dbStudent.ReachedAchievements.Count()}", achievementsToDisplay);

                await SendCorrectMessage(profileEmbded.Build(), ctx.Channel);
                await achievementsDialogue.ExecuteDialogue();
            }
        }
    }
}
