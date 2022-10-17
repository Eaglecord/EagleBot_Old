using System.Text.Json;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using EagleBot.Common;

namespace EagleBot.SlashCommands
{
    public class LevelCommand : ApplicationCommandModule
    {
        [SlashCommand("level", "Get level card for a discord member.")]
        public async Task Level(InteractionContext ctx,
            [Option("Member", "Get level card of a discord member.")]
            DiscordUser? member = null,
            [Option("IsEphemeral", "Request level card as ephemeral.")]
            Boolean isEphemeral = false)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new() { IsEphemeral = isEphemeral });

            if (member == null)
                member = ctx.Member;

            if (!File.Exists($"./data/levels/{member.Id}.json"))
            {
                await ctx.EditResponseAsync(new() { Content = $"Member **{member.Username}#{member.Discriminator}** is not ranked yet." });
                return;
            }

            StreamReader sr = new StreamReader($"./data/levels/{member.Id}.json");
            Levels levels = JsonSerializer.Deserialize<Levels>(sr.ReadToEnd())!;
            sr.Close();
            sr.Dispose();

            DiscordEmbedBuilder embed = new();
            embed.Title = $"{member.Username}#{member.Discriminator}";
            embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail() { Url = member.AvatarUrl };
            embed.AddField("Level", levels!.Rank.Level.ToString(), true);
            embed.AddField("Total Experience", levels!.Rank.TotalXp.ToString(), true);
            embed.AddField("Experience", $"`{levels!.Rank.Xp}` / `{levels!.Rank.RequiredXp}`", true);
            embed.AddField("Leaderboard position", $"#?", true);

            DiscordWebhookBuilder webhook = new();
            webhook.AddEmbed(embed);
            await ctx.EditResponseAsync(webhook);
        }
    }
}
