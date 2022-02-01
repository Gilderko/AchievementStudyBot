using DiscordLayer.CommandAttributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
            using (var dbcontext = new PV178StudyBotDbContext())
            {
                if (ctx.Message.MentionEveryone || ctx.Message.MentionedUsers.Count == 0)
                {
                    await SendErrorMessage("You failed to tag one person or tagged everyone", ctx.Channel);
                }

                Random randomGenerator = new Random();

                foreach (var potentialTeacher in ctx.Message.MentionedUsers)
                {
                    if (dbcontext.Teachers.Find(potentialTeacher.Id) != null)
                    {
                        await SendErrorMessage($"{potentialTeacher.Username} is already registered as a teacher", ctx.Channel);
                        continue;
                    }

                    var roleName = $"{ctx.User.Username}-Worshipper";
                    float roleColorR = (float)randomGenerator.NextDouble();
                    float roleColorG = (float)randomGenerator.NextDouble();
                    float roleColorB = (float)randomGenerator.NextDouble();
                    var newRole = await ctx.Guild.CreateRoleAsync(roleName, null, new DSharpPlus.Entities.DiscordColor(roleColorR, roleColorG, roleColorB), null, true);

                    var newTeacher = new Teacher()
                    {
                        Id = potentialTeacher.Id,
                        RoleId = newRole.Id,
                        RoleName = roleName,
                    };

                    await dbcontext.Teachers.AddAsync(newTeacher);

                    await dbcontext.SaveChangesAsync();
                    await SendCorrectMessage("Teacher was correctly created",ctx.Channel);
                }
            }
        }

        [Command("deleteTeacher")]
        [RequireAdmin]
        public async Task DeleteTeacher(CommandContext ctx)
        {
            using (var dbcontext = new PV178StudyBotDbContext())
            {
                if (ctx.Message.MentionEveryone || ctx.Message.MentionedUsers.Count == 0)
                {
                    await SendErrorMessage("You failed to tag one person or tagged everyone", ctx.Channel);
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
                    await SendCorrectMessage("Teacher was correctly deleted... dont forget to delete a role yourself", ctx.Channel);
                }
            }
        }

        [Command("createRankRoles")]
        [RequireAdmin]
        public async Task CreateRankRoles(CommandContext ctx)
        {
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
                    ColorR = 43,
                    ColorG = 255,
                    ColorB = 0,
                },
                new Rank()
                {
                    AwardedTitle = "Initiate",
                    Description = "Lorem",
                    PointsRequired = 100,
                    ColorR = 0,
                    ColorG = 162,
                    ColorB = 255,
                },
                new Rank()
                {
                    AwardedTitle = "Knight",
                    Description = "Lorem",
                    PointsRequired = 145,
                    ColorR = 0,
                    ColorG = 68,
                    ColorB = 255,
                },
                new Rank()
                {
                    AwardedTitle = "Senior Knight",
                    Description = "Lorem",
                    PointsRequired = 205,
                    ColorR = 137,
                    ColorG = 0,
                    ColorB = 255,
                },
                new Rank()
                {
                    AwardedTitle = "Paladin",
                    Description = "Lorem",
                    PointsRequired = 270,
                    ColorR = 255,
                    ColorG = 0,
                    ColorB = 137,
                },
                new Rank()
                {
                    AwardedTitle = "Elder",
                    Description = "Lorem",
                    PointsRequired = 335,
                    ColorR = 255,
                    ColorG = 255,
                    ColorB = 0,
                },
                new Rank()
                {
                    AwardedTitle = "High Elder",
                    Description = "Lorem",
                    PointsRequired = 430,
                    ColorR = 255,
                    ColorG = 171,
                    ColorB = 0,
                }
            };

            using (var dbContext = new PV178StudyBotDbContext())
            {
                foreach (var rank in ranks)
                {                    
                    var newRole = await ctx.Guild.CreateRoleAsync(rank.AwardedTitle, null,
                        new DSharpPlus.Entities.DiscordColor(rank.ColorR,rank.ColorG,rank.ColorB));

                    rank.Id = newRole.Id;
                    dbContext.Ranks.Add(rank);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
