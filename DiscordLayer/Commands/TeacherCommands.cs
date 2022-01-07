using DiscordLayer.CommandAttributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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

        }
    }
}
