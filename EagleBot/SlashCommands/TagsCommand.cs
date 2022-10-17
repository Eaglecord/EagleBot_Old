using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

using EagleBot.Common;

namespace EagleBot.SlashCommands
{
    public class TagsCommand : ApplicationCommandModule
    {
        [SlashCommand("tags", "Fetches a list of tags from meta and posts them.")]
        public async Task Tags(InteractionContext ctx,
            [Option("advanced", "Request tag stats.")]
            Boolean advanced = false)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new() { IsEphemeral = true });
            String description = "List of all tags & their aliases.";
            DiscordEmbedBuilder embed = new();
            foreach (Common.Index item in EagleBot.TagList.Index)
            {
                String invocations = "";
                String aliases = "Aliases: *none* ";
                if (item.Aliases.Length > 0)
                    aliases = $"Aliases: `{String.Join("`, `", item.Aliases)}`";
                if (advanced)
                    foreach (Tag t in EagleBot.TagInvocations.Tags)
                        if (t.Identifier == item.Identifier)
                            invocations = $"\nInvocations: total: `{t.Invocations.Total}`, ephemeral: `{t.Invocations.WasEphemeral}`";
                String tag = "\n\n**" + item.Identifier + "**\n" + aliases + invocations; 
                description += tag;
            }
            embed.Title = "Tags";

            IEnumerable<Page> pages = EagleBot.Interactivity.GeneratePagesInEmbed(description, SplitType.Line, embed);
            await ctx.Channel?.SendPaginatedMessageAsync(ctx.Member, pages);
            await ctx.EditResponseAsync(new() { Content = "<https://github.com/ExaInsanity/eaglecord-meta/>" });
        }
    }
}
