﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;

namespace DiscordLayer.Handlers.Dialogue.Steps
{
    public class TextStep : DialogueStepBase
    {
        private IDialogueStep _nextStep;
        private readonly int? _minLength;
        private readonly int? _maxLength;

        public TextStep(string content, IDialogueStep nextStep, int? minLength = null, int? maxLength = null) : base(content)
        {
            _nextStep = nextStep;
            _minLength = minLength;
            _maxLength = maxLength;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

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
                Description = $"{user.Mention}, please respond down below :)"
            };

            embedBuidler.AddField("To Stop the Dialogue", "User the ?cancel command");

            if (_minLength.HasValue)
            {
                embedBuidler.AddField("Min length: ", $"{_minLength.Value} characters");
            }
            if (_maxLength.HasValue)
            {
                embedBuidler.AddField("Max length: ", $"{_maxLength.Value} characters");
            }

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

                if (_minLength.HasValue)
                {
                    if (messageResult.Result.Content.Length < _minLength.Value)
                    {
                        await TryAgain(channel, $"Your input is {_minLength.Value - messageResult.Result.Content.Length} characters too short").ConfigureAwait(false);
                        continue;
                    }
                }
                if (_maxLength.HasValue)
                {
                    if (messageResult.Result.Content.Length > _maxLength.Value)
                    {
                        await TryAgain(channel, $"Your input is {messageResult.Result.Content.Length - _maxLength.Value} characters too long").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(messageResult.Result.Content);

                return false;
            }
        }
    }
}
