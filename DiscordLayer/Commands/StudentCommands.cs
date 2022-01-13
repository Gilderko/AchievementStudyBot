using DiscordLayer.CommandAttributes;
using DiscordLayer.Handlers.Dialogue.SlidingWindow;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
                    CurrentRankId = lowestRank.Id,
                    MyTeacherId = null
                };

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
                    true, false, "Would you like to request this achievement?", availableAchievements);

                (var accepted, var declined) = await pagedDialogue.ExecuteDialogue();

                foreach (var achievAccepted in accepted)
                {
                    var newRequest = new Request()
                    {
                        AchievmentId = achievAccepted.Id,
                        StudentId = dbStudent.Id,
                        TeacherId = dbStudent.MyTeacherId.Value
                    };

                    await dbContext.Requests.AddAsync(newRequest);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
