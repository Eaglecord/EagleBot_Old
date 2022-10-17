using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using Microsoft.Extensions.Logging;

namespace EagleBot.SlashCommands
{
    public class CacheCommand : ApplicationCommandModule
    {
        [SlashRequirePermissions(Permissions.Administrator)]
        [SlashCommand("cache", "Force refresh the cache")]
        public static async Task Cache(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            String index = await EagleBot.HttpClient.GetStringAsync($"{EagleBot.Configuration.TagUrl}index.json");
            File.WriteAllText("./data/indexCache.json", index);
            EagleBot.Client.Logger.LogInformation(new EventId(10, "Cache"), "Cache updated");
            await ctx.EditResponseAsync(new() { Content = "Cache updated" });
        }
    }
}
