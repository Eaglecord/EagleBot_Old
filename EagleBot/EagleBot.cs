using System.Text.Json;

using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.CommandsNext;

using Microsoft.Extensions.Logging;

using EagleBot.Common;

namespace EagleBot
{
    partial class EagleBot
    {
        private static TagIndex _tagList;
        private static TagInvocations _tagInvocations;
        public static EagleBotConfiguration Configuration { get; set; }
            = JsonSerializer.Deserialize<EagleBotConfiguration>(new StreamReader("./data/config.json").ReadToEnd())!;
        public static DiscordClient Client { get; private set; } = new(new()
        {
            Token = Configuration.Token,
            TokenType = TokenType.Bot,
            MinimumLogLevel = LogLevel.Information
        });
        public static InteractivityExtension Interactivity { get; set; } = Client.UseInteractivity(new()
        {
            AckPaginationButtons = true,
            ButtonBehavior = ButtonPaginationBehavior.Ignore,
            PaginationBehaviour = PaginationBehaviour.WrapAround,
            PaginationDeletion = PaginationDeletion.DeleteMessage,
            Timeout = TimeSpan.FromMinutes(3)
        });
        public static SlashCommandsExtension SlashCommands { get; private set; } = Client.UseSlashCommands();
        public static CommandsNextExtension Commands { get; set; } = Client.UseCommandsNext(new()
        {
            StringPrefixes = Configuration.StringPrefixes,
            EnableDms = false,
            EnableMentionPrefix = false,
            EnableDefaultHelp = false
        });
        public static HttpClient HttpClient { get; set; } = new HttpClient();
        public static Random Random = new Random();
        public static TagIndex TagList
        {
            get
            {
                if (_tagList != null &&
                    File.Exists("./data/indexCache.json") &&
                    File.GetLastWriteTimeUtc("./data/indexCache.json") >= (DateTime.UtcNow + TimeSpan.FromMinutes(15)))
                    return _tagList;
                String index = HttpClient.GetStringAsync($"{Configuration.TagUrl}index.json")
                    .GetAwaiter().GetResult();
                File.WriteAllText("./data/indexCache.json", index);
                _tagList = JsonSerializer.Deserialize<TagIndex>(index)!;
                return _tagList;
            }
        }
        public static TagInvocations TagInvocations
        {
            get
            {
                StreamReader sr = new("./data/invocations.json");
                _tagInvocations = JsonSerializer.Deserialize<TagInvocations>(sr.ReadToEnd())!;
                sr.Close();
                return _tagInvocations;
            }
        }
    }
}
