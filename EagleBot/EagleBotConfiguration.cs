using System.Text.Json.Serialization;

namespace EagleBot
{
    public class EagleBotConfiguration
    {
        [JsonPropertyName("token")]
        public String Token { get; set; }

        [JsonPropertyName("guildId")]
        public UInt64 GuildId { get; set; }

        [JsonPropertyName("helpChannels")]
        public UInt64[] HelpChannels { get; set; } = { };

        [JsonPropertyName("tagUrl")]
        public String TagUrl { get; set; } = "";

        [JsonPropertyName("townhallChannels")]
        public UInt64[] TownhallChannels { get; set; } = { };

        [JsonPropertyName("threadRoles")]
        public UInt64[] ThreadRoles { get; set; } = { };

        [JsonPropertyName("threadPings")]
        public Boolean ThreadPings { get; set; }

        [JsonPropertyName("hypixelPing")]
        public UInt64 HypixelRole { get; set; }

        [JsonPropertyName("hypixelChannel")]
        public UInt64 HypixelChannel { get; set; }

        [JsonPropertyName("stringPrefixes")]
        public String[] StringPrefixes { get; set; } = { };

        [JsonPropertyName("repositoryUrl")]
        public String RepositoryUrl { get; set; } = "";

        [JsonPropertyName("leveling")]
        public LevelingModule Leveling { get; set; } = new LevelingModule();

        [JsonPropertyName("pins")]
        public PinsModule Pins { get; set; } = new PinsModule();
    }
    public class LevelingModule
    {
        [JsonPropertyName("enabled")]
        public Boolean Enabled { get; set; } = false;

        [JsonPropertyName("minMessageLength")]
        public Int16 MinMessageLenght { get; set; } = 0;

        [JsonPropertyName("xpRange")]
        public Range XpRange { get; set; } = new Range();

        [JsonPropertyName("noXpChannels")]
        public UInt64[] NoXpChannels { get; set; } = { };

        [JsonPropertyName("noXpRoles")]
        public UInt64[] NoXpRoles { get; set; } = { };

        [JsonPropertyName("levelUpAnnouncementsId")]
        public UInt64 LevelUpChannel { get; set; } = 0;

        [JsonPropertyName("levelRoleRewards")]
        public LevelRewards[] LevelRoleRewards { get; set; } = { };

        [JsonPropertyName("stackRewards")]
        public Boolean StackRewards { get; set; } = true;
    }
    public class Range
    {
        [JsonPropertyName("min")]
        public Int16 Min { get; set; } = 0;

        [JsonPropertyName("max")]
        public Int16 Max { get; set; } = 1;
    }
    public class LevelRewards
    {
        [JsonPropertyName("roleId")]
        public UInt64 RoleId { get; set; } = 0;

        [JsonPropertyName("requiredLevel")]
        public Int16 RequiredLevel { get; set; } = 999;
    }
    public class PinsModule
    {
        [JsonPropertyName("enabled")]
        public Boolean Enabled { get; set; } = false;

        [JsonPropertyName("pinsChannel")]
        public UInt64 PinsChannel { get; set; } = 0;
    }
}