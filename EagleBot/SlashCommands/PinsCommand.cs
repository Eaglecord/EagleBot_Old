using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EagleBot.SlashCommands
{
    public class PinsCommand : ApplicationCommandModule
    {
        [SlashRequirePermissions(Permissions.Administrator)]
        [SlashCommand("pins", "Does nothing currently")]
        public async Task Pins(InteractionContext ctx)
        {
            return;
        }
    }
}
