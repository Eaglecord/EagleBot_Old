using System.Text.Json;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using EagleBot.Common;

namespace EagleBot.SlashCommands
{
    public class PreferencesCommand : ApplicationCommandModule
    {
        [SlashCommand("preferences", "Personal leveling settings.")]
        public async Task Preferences(InteractionContext ctx,
            [Option("LevelUpDm","Whether or not you wish to get your level up notification as a direct message from the bot.")]
            Boolean? dm = null,
            [Option("LevelUpMention","Whether or not you wish the bot to mention you when you level up.")]
            Boolean? mention = null,
            [Option("IsEphemeral", "Request preferences as ephemeral.")]
            Boolean isEphemeral = false)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new() { IsEphemeral = isEphemeral });
            StreamReader sr = new($"./data/levels/{ctx.Member.Id}.json");
            Levels levels = JsonSerializer.Deserialize<Levels>(sr.ReadToEnd())!;
            sr.Close();

            if (dm != null)
                levels!.Preferences.LevelUpDm = dm.Value;

            if (mention != null)
                levels!.Preferences.LevelUpMention = mention.Value;

            DiscordEmbedBuilder embed = new();
            embed.Title = $"{ctx.Member.Username}#{ctx.Member.Discriminator}";
            embed.AddField("Level Up Dm", levels!.Preferences.LevelUpDm.ToString(), true);
            embed.AddField("Level Up Mention", levels!.Preferences.LevelUpMention.ToString(), true);

            DiscordWebhookBuilder webhook = new();
            webhook.AddEmbed(embed);
            await ctx.EditResponseAsync(webhook);

            StreamWriter sw = new($"./data/levels/{ctx.Member.Id}.json");
            sw.Write(JsonSerializer.Serialize(levels, new JsonSerializerOptions() { WriteIndented = true }));
            sw.Close();
        }
    }
}
