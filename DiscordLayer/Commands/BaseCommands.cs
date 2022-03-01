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
    public class BaseCommands : BaseCommandModule
    {
        [Command("ping")]        
        [RequireAdmin]
        private async Task Ping(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };
            messagesToDelete.Add(await ctx.Channel.SendMessageAsync("Pongus"));
            await DeleteMessages(messagesToDelete);
        }

        protected async Task SendErrorMessage(string errorMessage, DiscordChannel channel)
        {
            var errMessage = await channel.SendMessageAsync($"`{errorMessage}`");
            await Task.Delay(5000);
            await errMessage.DeleteAsync();
        }

        [Command("leaderBoard")]    
        [Description("Shows top 10 highest grossing students + your position as a student")]
        private async Task LeaderBoard(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var allStudents = await dbContext.Students.OrderByDescending(student => student.AcquiredPoints).ToListAsync();
                var top10Students = allStudents.Take(10);

                var embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Leaderboard of top 10",
                    Color = DiscordColor.Gold
                };

                string leaderBoardString = top10Students.Aggregate
                    (("",1), (total, next) => (total.Item1 + $"{total.Item2}. {next.OnRegisterName}: {next.AcquiredPoints} caps\n",total.Item2 + 1)).Item1;

                leaderBoardString.Replace("1.", ":first_place:");
                leaderBoardString.Replace("2.", ":second_place:");
                leaderBoardString.Replace("3.", ":third_place:");

                embedBuilder.Description = leaderBoardString;

                var dbStudent = dbContext.Students.Find(ctx.Member.Id);
                if (dbStudent != null)
                {
                    var position = allStudents.FindIndex(student => student.Id == dbStudent.Id);

                    string description = $"{position + 1}. {dbStudent.OnRegisterName}: {dbStudent.AcquiredPoints} caps";
 
                    embedBuilder.AddField("Your position", description);
                }

                await SendCorrectMessage(embedBuilder.Build(), ctx.Channel);
            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("viewAchievements")]
        [Description("Allows to look though all available achievements")]
        public async Task ViewAchievements(CommandContext ctx)
        {
            var discordStudent = ctx.Member;
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var allAchievements = await dbContext.Achievements.ToListAsync();

                var pagedDialogue = new PagedDialogue<Achievement>(ctx.Guild, ctx.Client, ctx.Channel, discordStudent, false, false, "Available achievements"
                    , false, allAchievements);

                var (_, _, message) = await pagedDialogue.ExecuteDialogue();
                messagesToDelete.Add(message);
            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("va")]
        [Description("Short for viewAchievements")]
        public async Task ViewAchievements2(CommandContext ctx)
        {
            await ViewAchievements(ctx);
        }

        protected async Task<DiscordMessage> SendCorrectMessage(string correctMessage, DiscordChannel channel)
        {
            return (await channel.SendMessageAsync($"`{correctMessage}`"));
        }

        protected async Task<DiscordMessage> SendCorrectMessage(DiscordEmbed embedMessage, DiscordChannel channel)
        {
            return (await channel.SendMessageAsync(embed: embedMessage));
        }

        protected async Task DeleteMessages(IEnumerable<DiscordMessage> messages)
        {
            await Task.Delay(5000);
            foreach (var message in messages)
            {
                if (message != null)
                {
                    await message.DeleteAsync();
                }
            }
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
