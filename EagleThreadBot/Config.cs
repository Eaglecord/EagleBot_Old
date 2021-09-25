﻿using System;

using Newtonsoft.Json;

namespace EagleThreadBot
{
	internal class Config
	{
		[JsonProperty("token")]
		public String Token { get; set; }

		[JsonProperty("guildId")]
		public UInt64 GuildId { get; set; }

		[JsonProperty("channels")]
		public UInt64[] AllowedChannels { get; set; }
	}
}
