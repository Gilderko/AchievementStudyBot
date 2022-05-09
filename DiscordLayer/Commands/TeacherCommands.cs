using DiscordLayer.CommandAttributes;
using DiscordLayer.Handlers.Dialogue.SlidingWindow;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using PV178StudyBotDAL.Entities.ConnectionTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class TeacherCommands : BaseCommands
    {
        [Command("assignStudent")]
        [Description("Assign student to your group so he can request achievements")]
        [RequireTeacher]
        public async Task AsignStudent(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            if (ctx.Message.MentionedUsers.Count == 0 || ctx.Message.MentionEveryone)
            {
                await SendErrorMessage("You failed to tag someone or you tagged everyone", ctx.Channel);
                await DeleteMessages(messagesToDelete);
                return;
            }

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbTeacher = await dbContext.Teachers.FindAsync(ctx.Member.Id);
                var discordTeacher = ctx.Member;

                foreach (var mentionedUser in ctx.Message.MentionedUsers)
                {
                    var dbStudent = await dbContext.Students.FindAsync(mentionedUser.Id);
                    if (dbStudent == null)
                    {
                        await SendErrorMessage("This student did not register himself in the system", ctx.Channel);
                        continue;
                    }

                    var discordStudent = await ctx.Guild.GetMemberAsync(mentionedUser.Id);
                    if (dbStudent.MyTeacherId.HasValue)
                    {
                        var currentTeacher = await dbContext.Teachers.FindAsync(dbStudent.MyTeacherId);
                        var currentRole = ctx.Guild.GetRole(currentTeacher.RoleId);
                        await discordStudent.RevokeRoleAsync(currentRole);
                    }

                    dbStudent.MyTeacherId = discordTeacher.Id;
                    var discordRole = ctx.Guild.GetRole(dbTeacher.RoleId);
                    await discordStudent.GrantRoleAsync(discordRole);

                    await dbContext.SaveChangesAsync();
                    messagesToDelete.Add(await SendCorrectMessage("Student has been successfully assigned to a teacher", ctx.Channel));
                }
            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("as")]
        [Description("Short for assignStudent")]        
        [RequireTeacher]
        public async Task AsignStudent2(CommandContext ctx)
        {
            await AsignStudent(ctx);
        }

        [Command("yeetStudent")]
        [Description("Throw the student out of his teacher")]
        [RequireTeacher]
        public async Task YeetStudent(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            if (ctx.Message.MentionedUsers.Count == 0 || ctx.Message.MentionEveryone)
            {
                await SendErrorMessage("You failed to tag someone or you tagged everyone", ctx.Channel);
                await DeleteMessages(messagesToDelete);
                return;
            }

            using (var dbContext = new PV178StudyBotDbContext())
            {
                foreach (var mentionedUser in ctx.Message.MentionedUsers)
                {
                    var dbStudent = await dbContext.Students.FindAsync(mentionedUser.Id);
                    if (dbStudent == null)
                    {
                        await SendErrorMessage("This student did not register himself in the system", ctx.Channel);
                        continue;
                    }

                    if (!dbStudent.MyTeacherId.HasValue)
                    {
                        await SendErrorMessage("This student does not have a teacher", ctx.Channel);
                        continue;
                    }

                    var discordStudent = await ctx.Guild.GetMemberAsync(mentionedUser.Id);
                    var dbTeacher = await dbContext.Teachers.FindAsync(dbStudent.MyTeacherId);

                    var currentRole = ctx.Guild.GetRole(dbTeacher.RoleId);

                    // Removed role and teacher
                    await discordStudent.RevokeRoleAsync(currentRole);
                    dbStudent.MyTeacherId = null;

                    await dbContext.SaveChangesAsync();
                    messagesToDelete.Add(await SendCorrectMessage("Student´s teacher has been successfully removed", ctx.Channel));
                }
            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("ys")]
        [Description("Short for yeetStudent")]
        [RequireTeacher]
        public async Task YeetStudent2(CommandContext ctx)
        {
            await YeetStudent(ctx);
        }

        [Command("resolveRequests")]
        [Description("Command for resolving achievements of students")]
        [RequireTeacher]
        public async Task ResolveRequests(CommandContext ctx)
        {
            var discordTeacher = ctx.Member;
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbTeacher = await dbContext.Teachers
                    .Include(teacher => teacher.UnresolvedRequests)
                        .ThenInclude(req => req.RequestedAchievement)
                    .Include(teacher => teacher.UnresolvedRequests)
                        .ThenInclude(req => req.Student)
                    .FirstAsync(teacher => teacher.Id == discordTeacher.Id);

                var requests = dbTeacher.UnresolvedRequests.ToList();

                if (requests.Count == 0)
                {
                    await SendErrorMessage("You dont have any requests to resolve :)", ctx.Channel);
                    await DeleteMessages(messagesToDelete);
                    return;
                }

                var pagedDialogue = new PagedDialogue<Request>(ctx.Guild, ctx.Client, ctx.Channel, ctx.Member,
                    true, true, "Grant this request to the user?", true, requests);

                (var acceptedRequests, var declinedRequests, var message) = await pagedDialogue.ExecuteDialogue();
                messagesToDelete.Add(message);

                foreach (var accReq in acceptedRequests)
                {
                    var newStudentAndAchiev = new StudentAndAchievement()
                    {
                        AchievementId = accReq.AchievmentId,
                        StudentId = accReq.StudentId,
                        ReceivedWhen = DateTime.Now,
                    };

                    dbContext.Requests.Remove(accReq);
                    await dbContext.StudentAndAchievements.AddAsync(newStudentAndAchiev);
                }

                foreach (var decReq in declinedRequests)
                {
                    dbContext.Requests.Remove(decReq);
                }

                await dbContext.SaveChangesAsync();

                var resolvedStudentsIds = acceptedRequests.Select((req) => req.StudentId).ToHashSet();

                var studentsToUpdate = await dbContext.Students.Include(student => student.ReachedAchievements)
                    .ThenInclude(studAndAchiev => studAndAchiev.Achievement).Where(student => resolvedStudentsIds.Contains(student.Id)).ToListAsync();

                foreach (var dbStudent in studentsToUpdate)
                {
                    var studentPoints = dbStudent.ReachedAchievements.Aggregate(0, (total, next) => total + next.Achievement.PointReward);
                    var newRank = await CalculateAppropriateRank(dbContext, studentPoints);

                    var discordStudent = await ctx.Guild.GetMemberAsync(dbStudent.Id);
                    if (discordStudent == null)
                    {
                        continue;
                    }

                    var currentDiscordRole = ctx.Guild.GetRole(dbStudent.CurrentRankId);
                    if (currentDiscordRole == null)
                    {
                        continue;
                    }

                    await discordStudent.RevokeRoleAsync(currentDiscordRole);

                    var newDiscordRole = ctx.Guild.GetRole(newRank.Id);
                    if (newDiscordRole == null)
                    {
                        continue;
                    }

                    dbStudent.AcquiredPoints = studentPoints;
                    dbStudent.CurrentRankId = newRank.Id;
                    await discordStudent.GrantRoleAsync(newDiscordRole);
                }

                await dbContext.SaveChangesAsync();
                messagesToDelete.Add(await SendCorrectMessage($"Requests resolved: {acceptedRequests.Count()} accepted, {declinedRequests.Count()} declined", ctx.Channel));

                foreach (var accReq in acceptedRequests)
                {
                    var discordStudent = await ctx.Guild.GetMemberAsync(accReq.StudentId);
                    if (discordStudent == null)
                    {
                        continue;
                    }                    

                    var embed = new DiscordEmbedBuilder()
                    {
                        Title = "Achievement Unlocked",
                        Description = accReq.RequestedAchievement.ToString(),
                        Color = DiscordColor.SpringGreen
                    };

                    await discordStudent.SendMessageAsync(embed);
                }

            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("rr")]
        [Description("Short for resolveRequests")]
        [RequireTeacher]
        public async Task ResolveRequests2(CommandContext ctx)
        {
            await ResolveRequests(ctx);
        }

        [Command("displayStudents")]
        [Description("Displays all of your current students")]
        [RequireTeacher]
        public async Task DisplayStudents(CommandContext ctx)
        {
            var discordTeacher = ctx.Member;
            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbTeacher = await dbContext.Teachers.Include(teacher => teacher.MyStudents).FirstAsync(teacher => teacher.Id == discordTeacher.Id);
                var discordRole = ctx.Guild.GetRole(dbTeacher.RoleId);

                var displayEmbed = new DiscordEmbedBuilder()
                {
                    Title = $"List of {discordRole.Name}s:",
                    Color = DiscordColor.SpringGreen,
                };

                string students = dbTeacher.MyStudents.Aggregate("", (total, next) => total + $"{next.OnRegisterName}: {next.AcquiredPoints} caps\n");
                displayEmbed.Description = students;

                await SendCorrectMessage(displayEmbed.Build(), ctx.Channel);
            }
        }

        [Command("ds")]
        [Description("Short for displayStudents")]
        [RequireTeacher]
        public async Task DisplayStudents2(CommandContext ctx)
        {
            await DisplayStudents(ctx);
        }
    }
}
