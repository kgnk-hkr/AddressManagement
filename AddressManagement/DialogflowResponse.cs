﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using DialogflowWebhook.Responses;
//
//    var welcome = Welcome.FromJson(jsonString);

namespace DialogflowWebhook.Responses
{
	using System;
	using System.Collections.Generic;

	using System.Globalization;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public partial class DialogflowResponse
	{
		[JsonProperty("payload")]
		public Payload Payload { get; set; }
	}

	public partial class Payload
	{
		[JsonProperty("google")]
		public Google Google { get; set; }
	}

	public partial class Google
	{
		[JsonProperty("expectUserResponse")]
		public bool ExpectUserResponse { get; set; }

		[JsonProperty("richResponse")]
		public RichResponse RichResponse { get; set; }
	}

	public partial class RichResponse
	{
		[JsonProperty("items")]
		public Item[] Items { get; set; }
	}

	public partial class Item
	{
		[JsonProperty("simpleResponse")]
		public SimpleResponse SimpleResponse { get; set; }
	}

	public partial class SimpleResponse
	{
		[JsonProperty("textToSpeech")]
		public string TextToSpeech { get; set; }
	}

	public partial class Welcome
	{
		public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, DialogflowWebhook.Responses.Converter.Settings);
	}

	public static class Serialize
	{
		public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, DialogflowWebhook.Responses.Converter.Settings);
	}

	internal static class Converter
	{
		public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
			DateParseHandling = DateParseHandling.None,
			Converters =
			{
				new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
			},
		};
	}
}
