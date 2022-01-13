using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System;
using System.Threading.Tasks;

namespace DiscordLayer.CommandAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireTeacher : CheckBaseAttribute
    {
        public RequireTeacher()
        {

        }

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild == null || ctx.Member == null)
            {
                return Task.FromResult(false);
            }

            Teacher dbTeacher = null;
            using (var dbContext = new PV178StudyBotDbContext())
            {
                dbTeacher = dbContext.Teachers.Find(ctx.Member.Id);
            }

            return Task.FromResult(dbTeacher != null);
        }
    }
}
