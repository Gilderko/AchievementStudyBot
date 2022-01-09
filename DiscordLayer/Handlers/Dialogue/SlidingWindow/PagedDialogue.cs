using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using PV178StudyBotDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordLayer.Handlers.Dialogue.SlidingWindow
{
    public class PagedDialogue<Display,Value>
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;

        private List<Display> _contentToDisplay = new List<Display>();
        private List<Value> _valuesToReturn = new List<Value>();
        private int _currentPageIndex = 0;

        private DiscordEmoji _goLeft;
        private DiscordEmoji _goRight;
        private DiscordEmoji _accept;
        private DiscordEmoji _decline;
        private DiscordEmoji _skip;
        private DiscordEmoji _end;

        public async Task<Value> ExecuteDialogue()
        {
            DiscordMessage theMessage = null;

            while (true)
            {
                var interactivity = _client.GetInteractivity();

                var displayValue = _contentToDisplay[_currentPageIndex];
                DiscordEmbedBuilder displayEmbed = null;

                if (displayValue is Achievement)
                {
                    displayEmbed = BuildEmbed(displayValue as Achievement);
                }

                if (theMessage == null)
                {
                    theMessage = await _channel.SendMessageAsync(displayEmbed.Build());
                }
                else
                {
                    await theMessage.ModifyAsync(displayEmbed.Build());
                }







            }
        }

        public DiscordEmbedBuilder BuildEmbed(Achievement achievement)
        {
            var embedBuidler = new DiscordEmbedBuilder()
            {
                Title = achievement.Name,
                Color = DiscordColor.Blue,
                ImageUrl = achievement.ImagePath,
                Description = achievement.Description
            };

            embedBuidler.AddField("Rewards", $"{achievement.PointReward} caps");

            return embedBuidler;
        }



    }
}
