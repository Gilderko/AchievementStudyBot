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
using System.Linq;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class TeacherCommands : BaseCommands
    {
        [Command("assignStudent")]
        [RequireTeacher]
        public async Task AsignStudent(CommandContext ctx)
        {
            if (ctx.Message.MentionedUsers.Count == 0 || ctx.Message.MentionEveryone)
            {
                await SendErrorMessage("You failed to tag someone or you tagged everyone", ctx.Channel);
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
                    }

                    var discordStudent = await ctx.Guild.GetMemberAsync(mentionedUser.Id);
                    if (dbStudent.MyTeacherId.HasValue)
                    {
                        var currentRole = ctx.Guild.GetRole(dbTeacher.RoleId);
                        await discordStudent.RevokeRoleAsync(currentRole);                                
                    }

                    dbStudent.MyTeacherId = discordTeacher.Id;
                    var discordRole = ctx.Guild.GetRole(dbTeacher.RoleId);
                    await discordStudent.GrantRoleAsync(discordRole);

                    await dbContext.SaveChangesAsync();
                    await SendCorrectMessage("Student has been successfully assigned to a teacher", ctx.Channel);
                }
            }
        }

        [Command("resolveRequests")]
        [RequireTeacher]
        public async Task ResolveRequests(CommandContext ctx)
        {
            var discordTeacher = ctx.Member;

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
                    await SendCorrectMessage("You dont have any requests to resolve :)", ctx.Channel);
                }

                var pagedDialogue = new PagedDialogue<Request>(ctx.Guild, ctx.Client, ctx.Channel, ctx.Member,
                    true, true, "Grant this request to the user?", requests);

                (var acceptedRequests, var declinedRequests) = await pagedDialogue.ExecuteDialogue();

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

                var studentsToUpdate = dbContext.Students.Include(student => student.ReachedAchievements)
                    .ThenInclude(studAndAchiev => studAndAchiev.Achievement).Where(student => student.MyTeacherId == discordTeacher.Id);

                foreach (var dbStudent in studentsToUpdate)
                {
                    var studentPoints = dbStudent.ReachedAchievements.Aggregate(0, (total, next) => total + next.Achievement.PointReward);
                    dbStudent.AcquiredPoints = studentPoints;
                    var newRank = CalculateAppropriateRank(studentPoints);

                    var discordStudent = await ctx.Guild.GetMemberAsync(dbStudent.Id);
                    var currentDiscordRole = ctx.Guild.GetRole(dbStudent.CurrentRankId);
                    await discordStudent.RevokeRoleAsync(currentDiscordRole);

                    var newDiscordRole = ctx.Guild.GetRole(newRank.Id);
                    await discordStudent.GrantRoleAsync(newDiscordRole);
                }

                await dbContext.SaveChangesAsync();
                await SendCorrectMessage($"Requests resolved: {acceptedRequests.Count()} accepted, {declinedRequests.Count()} declined", ctx.Channel);
            }
        }

        [Command("displayStudents")]
        [RequireTeacher]
        public async Task DisplayStudents(CommandContext ctx)
        {
            var discordTeacher = ctx.Member;
            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbTeacher = await dbContext.Teachers.Include(teacher => teacher.MyStudents).FirstAsync(teacher => teacher.Id == discordTeacher.Id);

                var displayEmbed = new DiscordEmbedBuilder()
                {
                    Title = $"List of {dbTeacher.RoleName}s:",
                    Color = DiscordColor.SpringGreen,
                };

                string students = dbTeacher.MyStudents.Aggregate("", (total, next) => total + $"{next.OnRegisterName}: {next.AcquiredPoints} caps\n");
                displayEmbed.Description = students;

                await SendCorrectMessage(displayEmbed.Build(), ctx.Channel);
            }
        }
    }
}
