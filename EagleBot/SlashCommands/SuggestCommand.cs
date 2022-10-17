using System;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace EagleBot.SlashCommands
{
	public class SuggestCommand : ApplicationCommandModule
	{
		[SlashCommand("suggest", "Creates a suggestion thread for your idea.")]
		public async Task SuggestThreadCommand(InteractionContext ctx,
			[Option("Subject", "Specifies the subject for your suggestion thread.")]
			String subject)
		{
			if (!EagleBot.Configuration.TownhallChannels.Contains(ctx.Channel.Id))
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "Suggestion threads cannot be created in this channel."
				});
			await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
			DiscordThreadChannel thread = await ctx.Channel.CreateThreadAsync(subject, AutoArchiveDuration.Day, ChannelType.PublicThread);
			DiscordMessage message = await thread.SendMessageAsync(subject + " " + ctx.Member.Mention);
			if (EagleBot.Configuration.ThreadPings)
				foreach (var roleid in EagleBot.Configuration.ThreadRoles)
					await thread.SendMessageAsync($"<@&{roleid}>");
			await message.PinAsync();
			await ctx.FollowUpAsync(new()
			{
				IsEphemeral = true,
				Content = $"Thread about `{subject}` was created successfully."
			});
		}
	}
}