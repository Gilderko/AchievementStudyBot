using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordLayer.CommandAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireStudent : CheckBaseAttribute
    {
        public RequireStudent()
        {

        }

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild == null || ctx.Member == null)
            {
                return Task.FromResult(false);
            }

            Student dbStudent = null;
            using (var dbContext = new PV178StudyBotDbContext())
            {
                dbStudent = dbContext.Students.Find(ctx.Member.Id);
            }

            return Task.FromResult(dbStudent != null);
        }
    }
}
