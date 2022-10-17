using System.Text.Json;
using System.Text.Json.Serialization;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace EagleBot.Common
{
    public class Levels
    {
        [JsonPropertyName("member")]
        public Member Member { get; set; } = new Member();

        [JsonPropertyName("levels")]
        public Rank Rank { get; set; } = new Rank();

        [JsonPropertyName("preferences")]
        public Preferences Preferences { get; set; } = new Preferences();
    }
    public class Member
    {
        [JsonPropertyName("id")]
        public UInt64 Id { get; set; } = 0;

        [JsonPropertyName("name")]
        public String Name { get; set; } = "";
    }
    public class Rank
    {
        [JsonPropertyName("level")]
        public Int16 Level { get; set; } = 0;

        [JsonPropertyName("xp")]
        public Int64 Xp { get; set; } = 0;

        [JsonPropertyName("totalXp")]
        public Int64 TotalXp { get; set; } = 0;

        [JsonPropertyName("requiredXp")]
        public Int64 RequiredXp { get; set; } = 50;

        [JsonPropertyName("messages")]
        public Int32 Messages { get; set; } = 0;

        [JsonPropertyName("leaderboard")]
        public Int16 Leaderboard { get; set; } = 0;
    }
    public class Preferences
    {
        [JsonPropertyName("levelUpMention")]
        public Boolean LevelUpMention { get; set; } = true;

        [JsonPropertyName("levelUpDm")]
        public Boolean LevelUpDm { get; set; } = false;
    }
    public class Leveling
    {
        public static async Task NewMemberEvent(MessageCreateEventArgs e)
        {
            if (File.Exists($"./data/levels/{e.Author.Id}.json"))
                return;

            StreamWriter sw = new StreamWriter(File.Create($"./data/levels/{e.Author.Id}.json"));
            sw.Write(JsonSerializer.Serialize(new Levels(), new JsonSerializerOptions() { WriteIndented = true }));
            sw.Close();
        }
        public static async Task MessageSentEvent(DiscordClient s, MessageCreateEventArgs e)
        {
            StreamReader sr = new StreamReader($"./data/levels/{e.Author.Id}.json");
            Levels levels = JsonSerializer.Deserialize<Levels>(sr.ReadToEnd())!;
            sr.Close();
            sr.Dispose();

            Int32 xp = EagleBot.Random.Next(
                EagleBot.Configuration.Leveling.XpRange.Min,
                EagleBot.Configuration.Leveling.XpRange.Max);

            levels!.Member.Id = e.Author.Id;
            levels!.Member.Name = $"{e.Author.Username}#{e.Author.Discriminator}";
            levels!.Rank.Messages++;
            levels!.Rank.TotalXp += xp;
            levels!.Rank.Xp += xp;

            if (levels.Rank.Xp >= levels.Rank.RequiredXp)
                await LevelUp(levels, e);

            //await UpdateLeaderboard(levels);

            StreamWriter sw = new StreamWriter($"./data/levels/{e.Author.Id}.json");
            sw.Write(JsonSerializer.Serialize(levels, new JsonSerializerOptions() { WriteIndented = true }));
            sw.Close();
        }
        private static async Task LevelUp(Levels levels, MessageCreateEventArgs e)
        {
            DiscordMember member = await e.Guild.GetMemberAsync(e.Author.Id);

            levels.Rank.Level++;
            levels.Rank.Xp -= levels.Rank.RequiredXp;
            levels.Rank.RequiredXp = (Int32)(50 + (10 * levels.Rank.Level) + ((1.1 * levels.Rank.Level)) * (1.1 * levels.Rank.Level));

            await GrantRewards(member, levels);
            await Announce(member, levels, e);
        }
        private static async Task GrantRewards(DiscordMember member, Levels levels)
        {
            foreach (var reward in EagleBot.Configuration.Leveling.LevelRoleRewards)
            {
                if (levels.Rank.Level >= reward.RequiredLevel)
                {
                    await member.GrantRoleAsync(member.Guild.GetRole(reward.RoleId));
                }
            }
        }
        private static async Task Announce(DiscordMember member, Levels levels, MessageCreateEventArgs e)
        {
            DiscordChannel channel;
            if (EagleBot.Configuration.Leveling.LevelUpChannel != 0)
            {
                channel = member.Guild.GetChannel(EagleBot.Configuration.Leveling.LevelUpChannel);
            }
            else
            {
                channel = e.Channel;
            }

            if (levels.Preferences.LevelUpMention)
            {
                await channel.SendMessageAsync($"Level Up! {member.Mention} current level is: {levels.Rank.Level}");
            }
            else if (levels.Preferences.LevelUpDm)
            {
                await member.SendMessageAsync($"{member.Guild.Name}\nLevel Up! Your current level is: {levels.Rank.Level}");
                await channel.SendMessageAsync($"Level Up! {member.Username}#{member.Discriminator} current level is: {levels.Rank.Level}");
            }
            else
            {
                await channel.SendMessageAsync($"Level Up! {member.Username}#{member.Discriminator} current level is: {levels.Rank.Level}");
            }
        }
        private static async Task UpdateLeaderboard(Levels levels)
        {

        }
    }
}
