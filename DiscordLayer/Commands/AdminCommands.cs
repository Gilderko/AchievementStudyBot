using DiscordLayer.CommandAttributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System;
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

                    dbcontext.SaveChanges();
                }
            }
        }
    }
}
