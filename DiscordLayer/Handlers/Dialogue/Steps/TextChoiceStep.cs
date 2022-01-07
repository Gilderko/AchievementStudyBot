using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordLayer.Handlers.Dialogue.Steps
{
    public class TextChoiceStep : DialogueStepBase
    {
        private IDialogueStep _nextStep;

        private readonly Dictionary<string, TextChoiceData> _options;

        public TextChoiceStep(string content, IDialogueStep nextStep, Dictionary<string, TextChoiceData> options) : base(content)
        {
            _nextStep = nextStep;
            _options = options;
        }

        public Action<TextChoiceData> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public void SetNextStep(IDialogueStep nextstep)
        {
            _nextStep = nextstep;
        }

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuidler = new DiscordEmbedBuilder()
            {
                Title = _content,
                Color = DiscordColor.Blue,
                Description = $"{user.Mention}, please type the response :)"
            };

            foreach (var text in _options.Keys)
            {
                embedBuidler.AddField($"'{text}'", _options[text].Description);
            }

            embedBuidler = embedBuidler ?? optionalEmbed;

            embedBuidler.AddField("To Stop the Dialogue", "User the ?cancel command");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embded = await channel.SendMessageAsync(embedBuidler).ConfigureAwait(false);

                OnMessageAdded(embded);

                var messageResult = await interactivity.WaitForMessageAsync(x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                Console.WriteLine("got message");

                if (messageResult.TimedOut)
                {
                    return true;
                }

                OnMessageAdded(messageResult.Result);

                if (messageResult.Result.Content.Equals("?cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                TextChoiceData match;
                if (!IsAnswerInOption(messageResult.Result, out match))
                {
                    await TryAgain(channel, $"Your input string doesnt match the options").ConfigureAwait(false);
                    continue;
                }

                OnValidResult(match);
                Console.WriteLine("returning from step");
                return false;
            }
        }

        private bool IsAnswerInOption(DiscordMessage messageResult, out TextChoiceData match)
        {
            foreach (string option in _options.Keys)
            {
                if (option.Trim().Equals(messageResult.Content, StringComparison.InvariantCultureIgnoreCase))
                {
                    match = _options[option];
                    return true;
                }
            }
            match = null;
            return false;
        }
    }

    public class TextChoiceData
    {
        public TextChoiceData(string desc, ulong optionalData)
        {
            Description = desc;
            OptionalData = optionalData;
        }

        public TextChoiceData(string desc, string optionalData)
        {
            Description = desc;
            OptionalData = optionalData;
        }

        public string Description { get; set; }
        public object OptionalData { get; set; }
    }
}
