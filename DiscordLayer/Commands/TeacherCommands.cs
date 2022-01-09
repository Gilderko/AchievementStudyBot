using DiscordLayer.CommandAttributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PV178StudyBotDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordLayer.Commands
{
    public class TeacherCommands : BaseCommands
    {
        [Command("assignStudent")]
        [RequireTeacher]
        public async Task AsignStudent(CommandContext ctx)
        {
            if (ctx.Message.MentionedUsers.Count == 0 || ctx.Message.MentionEveryone)
            {
                await SendErrorMessage("You failed to tag someone or you tagged everyone", ctx.Channel);
                return;
            }

            using (var dbContext = new PV178StudyBotDbContext())
            {
                var dbTeacher = await dbContext .Teachers.FindAsync(ctx.Member.Id);
                var discordTeacher = ctx.Member;

                foreach (var mentionedUser in ctx.Message.MentionedUsers)
                {
                    var dbStudent = await dbContext.Students.FindAsync(mentionedUser.Id);
                    if (dbStudent == null)
                    {
                        await SendErrorMessage("This student did not register himself in the system", ctx.Channel);
                    }

                    dbStudent.MyTeacherId = discordTeacher.Id;

                    var discordStudent = await ctx.Guild.GetMemberAsync(mentionedUser.Id);
                    
                    // Grant student a role
                    var discordRole = ctx.Guild.GetRole(dbTeacher.RoleId);                    
                    await discordStudent.GrantRoleAsync(discordRole);

                    dbStudent.AcquiredPoints = 0;
                    dbStudent.CurrentRankId = CalculateAppropriateRank(dbStudent.AcquiredPoints).Id;     

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
