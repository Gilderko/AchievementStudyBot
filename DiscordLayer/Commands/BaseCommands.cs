using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class BaseCommands : BaseCommandModule
    {
        [Command("ping")]
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

        protected async Task<ulong> SendCorrectMessage(string correctMessage, DiscordChannel channel)
        {
            return (await channel.SendMessageAsync($"`{correctMessage}`")).Id;
        }

        protected async Task<ulong> SendCorrectMessage(DiscordEmbed embedMessage, DiscordChannel channel)
        {
            return (await channel.SendMessageAsync(embed: embedMessage)).Id;
        }
    }
}
