using System.Text.Json;
using System.Text.Json.Serialization;

namespace EagleBot.Common
{
    public partial class TagInvocations
    {
        [JsonPropertyName("tags")]
        public Tag[] Tags { get; set; }
    }
    public class Tag
    {
        [JsonPropertyName("identifier")]
        public String Identifier { get; set; }

        [JsonPropertyName("invocations")]
        public Invocations Invocations { get; set; }
    }
    public class Invocations
    {
        [JsonPropertyName("total")]
        public Int32 Total { get; set; }

        [JsonPropertyName("wasEphemeral")]
        public Int32 WasEphemeral { get; set; }
    }
    public partial class TagInvocations
    {
        public static void Serialize(TagInvocations _ti)
        {
            StreamWriter writer = new(File.OpenWrite("./data/invocations.json"));
            writer.BaseStream.SetLength(0);
            writer.Flush();
            writer.Write(JsonSerializer.Serialize(_ti, new JsonSerializerOptions() { WriteIndented = true }));
            writer.Close();
            writer.Dispose();
        }
        public static void Count(Index[] index, String usedTag, Boolean wasEphemeral)
        {
            TagInvocations invocations = EagleBot.TagInvocations;
            foreach (Tag tag in invocations.Tags)
            {
                if (tag.Identifier == usedTag)
                {
                    tag.Invocations.Total += 1;
                    if (wasEphemeral)
                        tag.Invocations.WasEphemeral += 1;
                    Serialize(invocations);
                    return;
                }
            }
            Match(index, invocations.Tags);
            Count(index, usedTag, wasEphemeral);
        }
        private static void Match(Index[] index, Tag[] invocations)
        {
            Tag[] tags = new Tag[index.Length];
            foreach (Index item in index)
            {
                Int32 i = Array.IndexOf(index, item);
                Tag tag = new() { Identifier = item.Identifier, Invocations = new() { Total = 0, WasEphemeral = 0 } };
                if (i < invocations.Length)
                {
                    if (item.Identifier == invocations[i].Identifier)
                        tag.Invocations = invocations[i].Invocations;
                }
                tags[i] = tag;
            }
            Serialize(new TagInvocations { Tags = tags });
        }
    }
}