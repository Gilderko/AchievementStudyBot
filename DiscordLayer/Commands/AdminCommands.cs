using DiscordLayer.CommandAttributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class AdminCommands : BaseCommands
    {
        [Command("registerTeacher")]
        [RequireAdmin]
        public async Task RegisterTeacher(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            using (var dbcontext = new PV178StudyBotDbContext())
            {
                if (ctx.Message.MentionEveryone || ctx.Message.MentionedUsers.Count == 0)
                {
                    await SendErrorMessage("You failed to tag one person or tagged everyone", ctx.Channel);
                    await DeleteMessages(messagesToDelete);
                    return;
                }

                Random randomGenerator = new Random();

                foreach (var potentialTeacher in ctx.Message.MentionedUsers)
                {
                    if (dbcontext.Teachers.Find(potentialTeacher.Id) != null)
                    {
                        await SendErrorMessage($"{potentialTeacher.Username} is already registered as a teacher", ctx.Channel);
                        continue;
                    }

                    var roleName = $"{potentialTeacher.Username}-Fan";
                    var newRole = await ctx.Guild.CreateRoleAsync(roleName, null, new DSharpPlus.Entities.DiscordColor(), null, true);

                    var newTeacher = new Teacher()
                    {
                        Id = potentialTeacher.Id,
                        RoleId = newRole.Id
                    };

                    await dbcontext.Teachers.AddAsync(newTeacher);

                    await dbcontext.SaveChangesAsync();
                    messagesToDelete.Add(await SendCorrectMessage("Teacher was correctly created", ctx.Channel));
                }
            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("deleteTeacher")]
        [RequireAdmin]
        public async Task DeleteTeacher(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            using (var dbcontext = new PV178StudyBotDbContext())
            {
                if (ctx.Message.MentionEveryone || ctx.Message.MentionedUsers.Count == 0)
                {
                    await SendErrorMessage("You failed to tag one person or tagged everyone", ctx.Channel);
                    await DeleteMessages(messagesToDelete);
                    return;
                }

                foreach (var potentialTeacher in ctx.Message.MentionedUsers)
                {
                    var dbTeacher = dbcontext.Teachers.Find(potentialTeacher.Id);
                    if (dbTeacher == null)
                    {
                        await SendErrorMessage($"{potentialTeacher.Username} is not a teacher", ctx.Channel);
                        continue;
                    }

                    dbcontext.Teachers.Remove(dbTeacher);

                    await dbcontext.SaveChangesAsync();
                    messagesToDelete.Add(await SendCorrectMessage("Teacher was correctly deleted... dont forget to delete a role yourself", ctx.Channel));
                }
            }

            await DeleteMessages(messagesToDelete);
        }

        [Command("createRankRoles")]
        [RequireAdmin]
        public async Task CreateRankRoles(CommandContext ctx)
        {
            var messagesToDelete = new List<DiscordMessage>() { ctx.Message };

            var ranks = new List<Rank>()
            {
                new Rank()
                {
                    AwardedTitle = "Civilian",
                    Description = "Lorem",
                    PointsRequired = 0,
                    ColorR = 0,
                    ColorG = 0,
                    ColorB = 0,
                },
                new Rank()
                {
                    AwardedTitle = "Squire",
                    Description = "Lorem",
                    PointsRequired = 55,
                    ColorR = 0.17f,
                    ColorG = 1,
                    ColorB = 0,
                },
                new Rank()
                {
                    AwardedTitle = "Initiate",
                    Description = "Lorem",
                    PointsRequired = 100,
                    ColorR = 0,
                    ColorG = 0.635f,
                    ColorB = 1,
                },
                new Rank()
                {
                    AwardedTitle = "Knight",
                    Description = "Lorem",
                    PointsRequired = 145,
                    ColorR = 0,
                    ColorG = 0.266f,
                    ColorB = 1,
                },
                new Rank()
                {
                    AwardedTitle = "Senior Knight",
                    Description = "Lorem",
                    PointsRequired = 205,
                    ColorR = 0.54f,
                    ColorG = 0,
                    ColorB = 1,
                },
                new Rank()
                {
                    AwardedTitle = "Paladin",
                    Description = "Lorem",
                    PointsRequired = 270,
                    ColorR = 1,
                    ColorG = 0,
                    ColorB = 0.54f,
                },
                new Rank()
                {
                    AwardedTitle = "Elder",
                    Description = "Lorem",
                    PointsRequired = 335,
                    ColorR = 1,
                    ColorG = 1,
                    ColorB = 0,
                },
                new Rank()
                {
                    AwardedTitle = "High Elder",
                    Description = "Lorem",
                    PointsRequired = 430,
                    ColorR = 1,
                    ColorG = 0.67f,
                    ColorB = 0.67f,
                }
            };

            using (var dbContext = new PV178StudyBotDbContext())
            {
                foreach (var rank in ranks)
                {
                    var newRole = await ctx.Guild.CreateRoleAsync(rank.AwardedTitle, null,
                        new DSharpPlus.Entities.DiscordColor(rank.ColorR, rank.ColorG, rank.ColorB));
                    rank.Id = newRole.Id;
                    dbContext.Ranks.Add(rank);
                }

                await dbContext.SaveChangesAsync();
                messagesToDelete.Add(await SendCorrectMessage("'Roles successfully created'", ctx.Channel));
            }

            await DeleteMessages (messagesToDelete);
        }

        [Command("yatasiOut")]
        [RequireAdmin]
        public async Task YatasiOut(CommandContext ctx)
        {
            ulong yatasiID = 539429734921273345;
            var yatasi = await ctx.Guild.GetMemberAsync(yatasiID);

            var embed = new DiscordEmbedBuilder()
            {
                Title = $"Yatasi out",
                Description = $"{yatasi.Mention}",
                Color = DiscordColor.HotPink,
                ImageUrl = @"https://media.giphy.com/media/buNXAFTVafgzn3EeJf/giphy.gif"
            };

            await SendCorrectMessage(embed.Build(), ctx.Channel);
        }
    }
}
