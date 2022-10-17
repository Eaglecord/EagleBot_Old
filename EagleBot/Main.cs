using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;

using EagleBot.Commands;
using EagleBot.Common;
using EagleBot.SlashCommands;

namespace EagleBot
{
    partial class EagleBot
    {
        internal static async Task Main(string[] args)
        {
            if (!Directory.Exists("./data"))
            {
                Directory.CreateDirectory("./data");
                if (!File.Exists("./data/config.json"))
                    File.Create("./data/config.json");
                if (!File.Exists("./data/indexCache.json"))
                    File.Create("./data/indexCache.json");
                if (!File.Exists("./data/invocations.json"))
                    File.Create("./data/invocations.json");
                if (!Directory.Exists("./data/levels"))
                    Directory.CreateDirectory("./data/levels");
            }
            Commands.RegisterCommands<HypixelCommands>();
            await Client.ConnectAsync();
            SlashCommands.RegisterCommands<TagCommand>(Configuration.GuildId);
            SlashCommands.RegisterCommands<TagsCommand>(Configuration.GuildId);
            SlashCommands.RegisterCommands<SuggestCommand>(Configuration.GuildId);
            SlashCommands.RegisterCommands<CreateCommand>(Configuration.GuildId);
            SlashCommands.RegisterCommands<CacheCommand>(Configuration.GuildId);
            SlashCommands.RegisterCommands<SelfRoleCommand>(Configuration.GuildId);
            if (Configuration.Leveling.Enabled)
            {
                SlashCommands.RegisterCommands<LevelCommand>(Configuration.GuildId);
                SlashCommands.RegisterCommands<PreferencesCommand>(Configuration.GuildId);
                Client.MessageCreated += MessageSentOnServer;
            }
            if (Configuration.Pins.Enabled)
            {
                SlashCommands.RegisterCommands<PinCommand>(Configuration.GuildId);
                SlashCommands.RegisterCommands<PinsCommand>(Configuration.GuildId);
            }
            SlashCommands.SlashCommandErrored += SlashCommandErrored;
            Client.ComponentInteractionCreated += ComponentInteractionCreated;
            await Task.Delay(-1);
        }
        private async static Task SlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
        {
            await e.Context.FollowUpAsync(new()
            {
                IsEphemeral = true,
                Content = $"An error occured. Please create an issue at <{Configuration.RepositoryUrl}> if this was unexpected."
            });
            Console.WriteLine($"{e.Exception}: {e.Exception.Message}\n{e.Exception.StackTrace}");
        }
        private async static Task ComponentInteractionCreated(DiscordClient s, ComponentInteractionCreateEventArgs e)
        {
            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            if (e.Id.StartsWith("sr.add-"))
            {
                UInt64 roleId = Convert.ToUInt64(e.Id.Substring(7));
                DiscordRole role = e.Guild.GetRole(roleId);
                DiscordMember member = await e.Guild.GetMemberAsync(e.User.Id);
                await member.GrantRoleAsync(role);
            } 
            else if (e.Id.StartsWith("sr.remove-"))
            {
                UInt64 roleId = Convert.ToUInt64(e.Id.Substring(10));
                DiscordRole role = e.Guild.GetRole(roleId);
                DiscordMember member = await e.Guild.GetMemberAsync(e.User.Id);
                await member.RevokeRoleAsync(role);
            }
            else
            { 
                return;
            }
        }
        private async static Task MessageSentOnServer(DiscordClient s, MessageCreateEventArgs e)
        {
            if ((!e.Author.IsBot && e.Message.Content.Length >= Configuration.Leveling.MinMessageLenght) && !Configuration.Leveling.NoXpChannels.Contains(e.Channel.Id))
            {
                await Task.Run(() => Leveling.NewMemberEvent(e));
                await Task.Run(() => Leveling.MessageSentEvent(s, e));
            }
        }
    }
}