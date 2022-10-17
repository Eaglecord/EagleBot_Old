using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace EagleBot.SlashCommands
{
    public class SelfRoleCommand : ApplicationCommandModule
    {
        [SlashRequirePermissions(Permissions.Administrator)]
        [SlashCommand("self-role", "Create a self role message.")]
        public async Task CreateSelfRoleCommand(InteractionContext ctx,
            [Option("Description", "Role description.")]
            String description,
            [Option("Role", "Role to be assigned.")]
            DiscordRole role,
            [Option("Embed", "Send message as embed.")]
            Boolean isEmbed)
        {
            if (role == null)
            {
                await ctx.CreateResponseAsync("`Role` was null.");
                return;
            }
            String content = $"**Auto role for {role.Mention}**\n\n{description}";
            DiscordMessageBuilder message = new();
            if (isEmbed)
            {
                DiscordEmbedBuilder embed = new();
                embed.Description = content;
                message.Embed = embed;
            }
            else
            {
                message.Content = content;
            }
            message.AddComponents(new DiscordComponent[]
            {
                new DiscordButtonComponent(ButtonStyle.Success, "sr.add-" + role.Id.ToString(), "Get role", false, null),
                new DiscordButtonComponent(ButtonStyle.Danger, "sr.remove-" + role.Id.ToString(), "Remove role", false, null)
            });
            await ctx.Channel.SendMessageAsync(message);
        }
    }
}
