using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace EagleBot.Commands
{
    public class HypixelCommands : BaseCommandModule
    {
        [Command("hypixelping")]
        [RequireRoles(RoleCheckMode.Any, roleIds: 745048861567090781)]
        public async Task HypixelPing(CommandContext ctx, [RemainingText] String message)
        {
            if (!(ctx.Channel.Id == EagleBot.Configuration.HypixelChannel))
            {
                await ctx.Message.DeleteAsync();
                return;
            }
            if (message == null)
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Mention} wants to play <@&{EagleBot.Configuration.HypixelRole}>.");
                return;
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"<@&{EagleBot.Configuration.HypixelRole}> {ctx.User.Mention}: `{message}`");
            }
            await ctx.Message.DeleteAsync();
        }
        [Command("hypixelgames")]
        [RequireRoles(RoleCheckMode.MatchIds, roleIds: 745048861567090781)]
        public async Task HypixelGames(CommandContext ctx)
        {
            if (!(ctx.Channel.Id == EagleBot.Configuration.HypixelChannel))
            {
                await ctx.Message.DeleteAsync();
                return;
            }
            await ctx.RespondAsync("HAH! Thats the WRONG COMMAND! Try again idiot.");
        }
    }
}
