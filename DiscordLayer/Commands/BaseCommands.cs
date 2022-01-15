using DiscordLayer.CommandAttributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class BaseCommands : BaseCommandModule
    {
        [Command("ping")]
        [RequireAdmin]
        private async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pongus");
        }
        protected async Task SendErrorMessage(string errorMessage, DiscordChannel channel)
        {
            var errMessage = await channel.SendMessageAsync($"`{errorMessage}`");
            await Task.Delay(5000);
            await errMessage.DeleteAsync();
        }

        [Command("leaderBoard")]        
        private async Task LeaderBoard(CommandContext ctx)
        {
            using (var dbContext = new PV178StudyBotDbContext())
            {
                var allStudents = await dbContext.Students.OrderByDescending(student => student.AcquiredPoints).ToListAsync();
                var top10Students = allStudents.Take(10);

                var embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Leaderboard of top 25",
                    Color = DiscordColor.Gold
                };

                string leaderBoardString = top10Students.Aggregate
                    (("",1), (total, next) => (total.Item1 + $"{total.Item2}. {next.OnRegisterName}: {next.AcquiredPoints} caps",total.Item2 + 1)).Item1;

                var dbStudent = dbContext.Students.Find(ctx.Member.Id);
                if (dbStudent != null)
                {
                    var position = allStudents.FindIndex(student => student.Id == dbStudent.Id);

                    string description = $"{position}. {dbStudent.OnRegisterName}: {dbStudent.AcquiredPoints} caps";
 
                    embedBuilder.AddField("Your position", description);
                }

                await SendCorrectMessage(embedBuilder.Build(), ctx.Channel);
            }
        }

        protected async Task<ulong> SendCorrectMessage(string correctMessage, DiscordChannel channel)
        {
            return (await channel.SendMessageAsync($"`{correctMessage}`")).Id;
        }

        protected async Task<ulong> SendCorrectMessage(DiscordEmbed embedMessage, DiscordChannel channel)
        {
            return (await channel.SendMessageAsync(embed: embedMessage)).Id;
        }

        protected Rank CalculateAppropriateRank(int points)
        {
            Rank calculatedRank = null;

            using (var dbContext = new PV178StudyBotDbContext())
            {
                foreach (var rank in dbContext.Ranks.OrderBy(rank => rank.PointsRequired))
                {
                    if (rank.PointsRequired <= points)
                    {
                        calculatedRank = rank;
                    }
                }
            }

            return calculatedRank;
        }
    }
}
