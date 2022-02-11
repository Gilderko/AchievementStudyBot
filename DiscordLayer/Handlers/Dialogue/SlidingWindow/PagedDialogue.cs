using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using PV178StudyBotDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordLayer.Handlers.Dialogue.SlidingWindow
{
    public class PagedDialogue<Value> where Value : class, ITextDisplayable
    {
        private readonly DiscordGuild _guild;
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private readonly bool _canAccept = false;
        private readonly bool _canDeny = false;
        private readonly bool _canConfirmAll = false;
        private readonly string _title = "";

        private readonly List<Value> _valuesToReturn = new List<Value>();
        private int _currentPageIndex = 0;

        private DiscordEmoji _goLeft;
        private DiscordEmoji _goRight;
        private DiscordEmoji _accept;
        private DiscordEmoji _decline;
        private DiscordEmoji _terminate;
        private DiscordEmoji _confirm;
        private DiscordEmoji _confirmAllButDeclined;

        public PagedDialogue(DiscordGuild guild, DiscordClient client, DiscordChannel channel,
            DiscordUser user, bool canAccept, bool canDeny, string title, bool allowConfirmAll, List<Value> valuesToReturn)
        {
            _guild = guild;
            _client = client;
            _channel = channel;
            _user = user;
            _canAccept = canAccept;
            _canDeny = canDeny;
            _title = title;
            _canConfirmAll = allowConfirmAll;
            _valuesToReturn = valuesToReturn;
        }

        public async Task<(IEnumerable<Value>, IEnumerable<Value>, DiscordMessage)> ExecuteDialogue()
        {
            if (_valuesToReturn.Count == 0)
            {
                return (new HashSet<Value>(), new HashSet<Value>(), null);
            }

            var availableReactions = InitializeEmojis();

            var toAccept = new HashSet<Value>();
            var toDeny = new HashSet<Value>();

            DiscordMessage theMessage = null;

            while (true)
            {
                var interactivity = _client.GetInteractivity();

                var displayValue = _valuesToReturn[_currentPageIndex];
                DiscordEmbedBuilder displayEmbed = null;

                if (displayValue is Achievement)
                {
                    displayEmbed = BuildEmbed(displayValue as Achievement);
                }
                else if (displayValue is Request)
                {
                    var student = _guild.Members.FirstOrDefault(member => member.Key == (displayValue as Request).StudentId).Value;
                    displayEmbed = BuildEmbed(displayValue as Request, student == null ? "Unknown" : student.DisplayName);
                }

                if (theMessage == null)
                {
                    theMessage = await _channel.SendMessageAsync(displayEmbed.Build());

                    foreach (var reaction in availableReactions)
                    {
                        await theMessage.CreateReactionAsync(reaction);
                    }
                }
                else
                {
                    string toAcceptField = toAccept.Aggregate("", (total, next) => total + $"{next.ToStringShort()}\n");
                    string toDenyField = toDeny.Aggregate("", (total, next) => total + $"{next.ToStringShort()}\n");

                    if (toAcceptField != string.Empty)
                    {
                        displayEmbed.AddField("Accepted values:", toAcceptField);
                    }
                    if (toDenyField != string.Empty)
                    {
                        displayEmbed.AddField("Denied values:", toDenyField);
                    }                    

                    await theMessage.ModifyAsync(displayEmbed.Build());
                }

                var reactionResult = await interactivity.WaitForReactionAsync
                    (reaction => availableReactions.Contains(reaction.Emoji), theMessage, _user);

                if (reactionResult.TimedOut)
                {
                    return (new HashSet<Value>(), new HashSet<Value>(), theMessage);
                }

                var reactionEmoji = reactionResult.Result.Emoji;

                if (reactionEmoji == _goLeft)
                {
                    _currentPageIndex = Math.Clamp(_currentPageIndex - 1, 0, _valuesToReturn.Count - 1);
                }
                else if (reactionEmoji == _goRight)
                {
                    _currentPageIndex = Math.Clamp(_currentPageIndex + 1, 0, _valuesToReturn.Count - 1);
                }
                else if (reactionEmoji == _accept && _canAccept)
                {
                    if (toDeny.Contains(_valuesToReturn[_currentPageIndex]))
                    {
                        toDeny.Remove(_valuesToReturn[_currentPageIndex]);                        
                    }
                    else
                    {
                        toAccept.Add(_valuesToReturn[_currentPageIndex]);                        
                    }
                    _currentPageIndex = Math.Clamp(_currentPageIndex + 1, 0, _valuesToReturn.Count - 1);
                }
                else if (reactionEmoji == _decline && _canDeny)
                {
                    if (toAccept.Contains(_valuesToReturn[_currentPageIndex]))
                    {
                        toAccept.Remove(_valuesToReturn[_currentPageIndex]);
                    }
                    else
                    {
                        toDeny.Add(_valuesToReturn[_currentPageIndex]);
                    }
                    _currentPageIndex = Math.Clamp(_currentPageIndex + 1, 0, _valuesToReturn.Count - 1);
                }
                else if (reactionEmoji == _confirm)
                {
                    return (toAccept, toDeny, theMessage);
                }
                else if (reactionEmoji == _terminate)
                {
                    return (new HashSet<Value>(), new HashSet<Value>(), theMessage);
                }
                else if (_canConfirmAll && reactionEmoji == _confirmAllButDeclined)
                {
                    toAccept = _valuesToReturn.Except(toDeny).ToHashSet();
                }

                await theMessage.DeleteReactionAsync(reactionEmoji, reactionResult.Result.User);
            }
        }

        private IEnumerable<DiscordEmoji> InitializeEmojis()
        {
            _goLeft = DiscordEmoji.FromName(_client, ":arrow_backward:");
            _goRight = DiscordEmoji.FromName(_client, ":arrow_forward:");
            _accept = DiscordEmoji.FromName(_client, ":white_check_mark:");
            _decline = DiscordEmoji.FromName(_client, ":negative_squared_cross_mark:");
            _confirm = DiscordEmoji.FromName(_client, ":green_square:");
            _terminate = DiscordEmoji.FromName(_client, ":red_square:");
            _confirmAllButDeclined = DiscordEmoji.FromName(_client, ":ballot_box_with_check:");

            var resultList = new List<DiscordEmoji>() { _goLeft, _goRight };

            if (_canAccept)
            {
                resultList.Add(_accept);                
            }
            if (_canDeny)
            {
                resultList.Add(_decline);
            }
            
            resultList.Add(_terminate);
            resultList.Add(_confirm);
            if (_canConfirmAll)
            {
                resultList.Add(_confirmAllButDeclined);
            }

            return resultList;
        }

        private DiscordEmbedBuilder BuildEmbed(Achievement achievement)
        {
            var embedBuidler = new DiscordEmbedBuilder()
            {
                Title = _title,
                Color = DiscordColor.Blue,
            };

            embedBuidler.WithThumbnail(achievement.ImagePath, 124, 124);

            embedBuidler.AddField($"{achievement.Name}", $"{achievement.Description}");
            embedBuidler.AddField("Rewards", $"{achievement.PointReward} caps");

            AddDescription(embedBuidler);

            return embedBuidler;
        }

        private DiscordEmbedBuilder BuildEmbed(Request request, string studentDiscordName)
        {
            var embedBuidler = new DiscordEmbedBuilder()
            {
                Title = _title,
                Color = DiscordColor.Blue,
                Description = request.ToString()
            };

            embedBuidler.WithThumbnail(request.RequestedAchievement.ImagePath, 124, 124);

            AddDescription(embedBuidler);

            return embedBuidler;
        }

        private void AddDescription(DiscordEmbedBuilder embedBuilder)
        {
            string description = $"{_goLeft.GetDiscordName()} -> used for turning page to the left" +
                $"\n{_goRight.GetDiscordName()} -> used for turning page to the right" +
                $"\n{_terminate.GetDiscordName()} -> terminate selection (wont process selected options)" +
                $"\n{_confirm.GetDiscordName()} -> confirm selection (will process selected option)";
                

            if (_canAccept)
            {
                description += $"\n{_accept.GetDiscordName()} -> accept appropriately toward the question";
            }
            if (_canDeny)
            {
                description += $"\n{_decline.GetDiscordName()} -> decline appropriately toward the question";
            }
            if (_canConfirmAll)
            {
                description += $"\n{_confirmAllButDeclined.GetDiscordName()} -> confirms ALL but the DECLINED options!";
            }

            embedBuilder.AddField("Description", description);            
        }
    }
}
