using System.Text.Json.Serialization;

namespace EagleBot.Common
{
    public class TagIndex
    {
        [JsonPropertyName("index")]
        public Index[] Index { get; set; }
    }

    public class Index
    {
        [JsonPropertyName("identifier")]
        public String Identifier { get; set; }

        [JsonPropertyName("aliases")]
        public String[] Aliases { get; set; }

        [JsonPropertyName("url")]
        public String Url { get; set; }

        [JsonPropertyName("isEmbed")]
        public Boolean IsEmbed { get; set; }

        [JsonPropertyName("isPaged")]
        public Boolean IsPaged { get; set; }
    }
}
