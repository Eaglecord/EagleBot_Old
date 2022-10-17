using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EagleBot.SlashCommands
{
    public class PinCommand : ApplicationCommandModule
    {
        [SlashRequirePermissions(Permissions.ManageMessages)]
        [SlashCommand("pin", "Pins a message to the pins channel.")]
        public async Task Pin(InteractionContext ctx, 
            [Option("id", "id of the message to pin.")] String messageId, 
            [Option("silent", "Whether or not the pin is made silently.")] Boolean silent = false)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new() { IsEphemeral = silent });
            DiscordChannel channel = ctx.Guild.GetChannel(EagleBot.Configuration.Pins.PinsChannel);
            DiscordMessage message = ctx.Channel.GetMessageAsync(UInt64.Parse(messageId)).Result;
            DiscordEmbedBuilder embed = new();
            embed.Title = $"#{message.Channel.Name} - {message.Author.Username}";
            embed.Description = $"{message.Content}\n[View]({message.JumpLink})";
            await channel.SendMessageAsync(embed: embed);
            DiscordEmbedBuilder response = new();
            DiscordWebhookBuilder webhook = new();
            response.Title = $"Pinned {message.Author.Username}'s message.";
            response.Description = $"[View]({message.JumpLink})";
            webhook.AddEmbed(embed: response);
            await ctx.EditResponseAsync(webhook);
        }
    }
}
