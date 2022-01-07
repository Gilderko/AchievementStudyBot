using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace DiscordLayer.Handlers.Dialogue.Steps
{
    public abstract class DialogueStepBase : IDialogueStep
    {
        protected readonly string _content;

        public DiscordEmbedBuilder optionalEmbed;

        public DialogueStepBase(string content)
        {
            _content = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };

        public abstract IDialogueStep NextStep { get; }

        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        protected async Task TryAgain(DiscordChannel channel, string problem)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Please try again",
                Color = DiscordColor.Red
            };

            embedBuilder.AddField("There was a problem with your previous input", problem);

            var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

            OnMessageAdded(embed);
        }
    }
}
