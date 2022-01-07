using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;

namespace DiscordLayer.Handlers.Dialogue.Steps
{
    public class IntStep : DialogueStepBase
    {
        private IDialogueStep _nextStep;
        public int _minValue;
        public int _maxValue;

        public string? dynamicOptionalCommentary = null;

        public IntStep(string content, IDialogueStep nextStep, int minValue, int maxValue) : base(content)
        {
            _nextStep = nextStep;
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public Action<int> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public void SetNextStep(IDialogueStep nextstep)
        {
            _nextStep = nextstep;
        }

        public IDialogueStep GetNextStep()
        {
            return _nextStep;
        }

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuidler = new DiscordEmbedBuilder()
            {
                Title = _content,
                Color = DiscordColor.Blue,
                Description = $"{user.Mention}, please respond down below :)"
            };

            if (dynamicOptionalCommentary != null)
            {
                embedBuidler.AddField("You still have atribute points to spend:", dynamicOptionalCommentary);
            }
            embedBuidler.AddField("To Stop the Dialogue", "User the ?cancel command");

            embedBuidler.AddField("Min value: ", $"{_minValue}");
            embedBuidler.AddField("Max value: ", $"{_maxValue}");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embded = await channel.SendMessageAsync(embed: embedBuidler).ConfigureAwait(false);

                OnMessageAdded(embded);

                var messageResult = await interactivity.WaitForMessageAsync(x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                if (messageResult.TimedOut)
                {
                    return true;
                }

                OnMessageAdded(messageResult.Result);

                if (messageResult.Result.Content.Equals("?cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (!int.TryParse(messageResult.Result.Content, out int inputValue))
                {
                    await TryAgain(channel, $"Your input is not an integer").ConfigureAwait(false);
                    continue;
                }

                if (inputValue < _minValue)
                {
                    await TryAgain(channel, $"Your input value {inputValue} is too small").ConfigureAwait(false);
                    continue;
                }
                if (inputValue > _maxValue)
                {
                    await TryAgain(channel, $"Your input value {inputValue} is too big").ConfigureAwait(false);
                    continue;
                }

                OnValidResult(inputValue);

                return false;
            }
        }
    }
}
