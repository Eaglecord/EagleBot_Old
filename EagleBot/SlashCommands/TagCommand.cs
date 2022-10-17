using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

using EagleBot.Common;

namespace EagleBot.SlashCommands
{
	public class TagCommand : ApplicationCommandModule
	{		
		[SlashCommand("tag", "Fetches a tag from meta and posts it.")]
		public async Task Tag(InteractionContext ctx, 
			[Option("tag", "Tag ID as found on eaglecord meta repository.")]
				String tag,
			[Option("isEphemeral","Request the tag as ephemeral.")] 
				Boolean isEphemeral = false) 
		{
			await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new() { IsEphemeral = isEphemeral });
			TagIndex TagList = EagleBot.TagList;
			String Url = "";
			Boolean isEmbed = false, isPaged = false;
			Int32 i;
			foreach (Common.Index item in TagList.Index)
			{
				if (item.Identifier == tag
					|| item.Aliases.Contains(tag))
				{
					Url = item.Url;
					tag = item.Identifier;
					if (item.IsEmbed)
					{
						isEmbed = true;
						if (item.IsPaged)
							isPaged = true;
					}
					TagInvocations.Count(TagList.Index, tag, isEphemeral);
				}
			}
			if (Url == "")
			{
				await ctx.FollowUpAsync(new()
				{
					Content = $"Could not fetch url for {tag}. Try again.",
					IsEphemeral = true
				});
				return;
			}
			String Tag = "";
			if (isEmbed)
				Tag = $"[**{Url}**]({EagleBot.Configuration.TagUrl}{Url})\n\n";
			Tag += await EagleBot.HttpClient.GetStringAsync($"{EagleBot.Configuration.TagUrl}{Url}");
			String ephemeral = "";
			if ((Tag.Length >= 4000 || (isEphemeral && isPaged)) && !isEmbed)
			{
				if (isEphemeral)
					ephemeral = "Could not request tag as ephemeral.";
				isPaged = true;
				isEmbed = true;
			}
			else if ((Tag.Length >= 4096 || (isEphemeral && isPaged)) && isEmbed)
			{
				if (isEphemeral)
					ephemeral = "Could not request tag as ephemeral.";
				isPaged = true;
			}
			if (isEmbed)
			{
				DiscordEmbedBuilder embed = new();
				DiscordWebhookBuilder webhook = new();
				if (!isPaged)
				{
					embed.Description = Tag;
					webhook.AddEmbed(embed);
					await ctx.EditResponseAsync(webhook);
				}
				else
				{
					await ctx.EditResponseAsync(new() { Content = ephemeral });
					IEnumerable<Page> pages = EagleBot.Interactivity.GeneratePagesInEmbed(Tag, SplitType.Line, embed);
					await ctx.Channel?.SendPaginatedMessageAsync(ctx.Member, pages)!;
				}
			}
			else
				await ctx.EditResponseAsync(new() { Content = Tag });
		}
	}
}
