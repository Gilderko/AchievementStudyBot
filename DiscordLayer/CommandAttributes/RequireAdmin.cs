﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PV178StudyBotDAL;
using PV178StudyBotDAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordLayer.CommandAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireAdmin : CheckBaseAttribute
    {
        public static ulong[] authorID;

        public RequireAdmin()
        {

        }

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild == null || ctx.Member == null)
            {
                return Task.FromResult(false);
            }

            Admin existingAdmin = null;
            using (var dbContext = new PB178StudyBotDbContext())
            {
                existingAdmin = dbContext.Admins.Find(ctx.Member.Id);
            }

            return Task.FromResult(existingAdmin != null);
        }
    }
}