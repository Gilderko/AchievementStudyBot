using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordLayer.Handlers.Dialogue.Steps
{

    public class ReactionStep : DialogueStepBase
    {
        private IDialogueStep _nextStep;

        private readonly Dictionary<DiscordEmoji, ReactionStepData> _options;

        private DiscordEmoji _selectedEmoji;

        public ReactionStep(string content, Dictionary<DiscordEmoji, ReactionStepData> options) : base(content)
        {
            _options = options;
        }

        public override IDialogueStep NextStep => _nextStep ?? _options[_selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };

        public void SetNextStep(IDialogueStep nextstep)
        {
            _nextStep = nextstep;
        }

        public async override Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancelEmoji = DiscordEmoji.FromName(client, ":x:");

            DiscordEmbedBuilder embedBuidler;
            if (optionalEmbed == null)
            {
                embedBuidler = new DiscordEmbedBuilder()
                {
                    Title = _content,
                    Color = DiscordColor.Blue,
                    Description = $"{user.Mention}, please respond down below :)"
                };
            }
            else
            {
                embedBuidler = optionalEmbed;
            }

            foreach (var emoji in _options.Keys)
            {
                embedBuidler.AddField($"{emoji.ToString()} is used for:", _options[emoji].Content);
            }

            embedBuidler.AddField("To stop the dialogue", "React with the :x: emoji");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuidler).ConfigureAwait(false);

                OnMessageAdded(embed);

                foreach (var emoji in _options.Keys)
                {
                    await embed.CreateReactionAsync(emoji).ConfigureAwait(false);
                }

                await embed.CreateReactionAsync(cancelEmoji).ConfigureAwait(false);

                var reactionResult = await interactivity.WaitForReactionAsync(
                    x => _options.ContainsKey(x.Emoji) || x.Emoji == cancelEmoji, embed, user).ConfigureAwait(false);

                if (reactionResult.TimedOut)
                {
                    return true;
                }

                if (reactionResult.Result.Emoji == cancelEmoji)
                {
                    return true;
                }

                _selectedEmoji = reactionResult.Result.Emoji;


                OnValidResult(_selectedEmoji);

                return false;
            }
        }
    }

    public class ReactionStepData
    {
        public string Content { get; set; }
        public IDialogueStep NextStep { get; set; }
        public object optionalData { get; set; }
    }
}
