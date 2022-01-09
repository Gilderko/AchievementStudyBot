using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class StudentCommands : BaseCommands
    {
        [Command("registerStudent")]
        public async Task StudentRegister(CommandContext ctx)
        {
            using (var dbContext = new PB178StudyBotDbContext())
            {
                var existingStudent = dbContext.Students.Find(ctx.Member.Id);
                if (existingStudent != null)
                {
                    await SendErrorMessage("You are alreade registered as a student", ctx.Channel);
                    return;
                }

                var lowestRank = dbContext.Ranks.OrderBy(rank => rank.PointsRequired).First();

                var newStudent = new Student()
                {
                    Id = ctx.Member.Id,
                    AcquiredPoints = 0,
                    CurrentRankId = lowestRank.Id,
                    MyTeacherId = null
                };

                dbContext.Students.Add(newStudent);

                await dbContext.SaveChangesAsync();
                await SendCorrectMessage("Congratulation, you have been registered as a new student :)",ctx.Channel);
            }
        }
    }
}
