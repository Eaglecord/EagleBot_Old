using System;
using System.IO;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using Microsoft.Extensions.Logging;

namespace EagleThreadBot.SlashCommands
{
    public class CacheCommand : ApplicationCommandModule
    {
        [SlashRequirePermissions(Permissions.Administrator)]
        [SlashCommand("cache", "Force refresh the cache")]
        public static async Task Cache(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            if (!Directory.Exists("./cache"))
                Directory.CreateDirectory("./cache");
            if (!File.Exists("./cache/index.json"))
                File.Create("./cache/index.json");

            String index = await Program.HttpClient.GetStringAsync($"{Program.Configuration.TagUrl}index.json");

            // Force refresh the cache
            File.WriteAllText("./cache/index.json", index);
            Program.Client.Logger.LogInformation(new EventId(10, "Cache"), "Cache updated");
            await ctx.CreateResponseAsync("Cache updated.");
        }
    }
}
