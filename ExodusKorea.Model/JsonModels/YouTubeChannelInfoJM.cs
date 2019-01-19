using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExodusKorea.Model.JsonModels
{
    public partial class YouTubeChannelInfoJM
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfoChannel PageInfo { get; set; }

        [JsonProperty("items")]
        public Item_ChannelInfo[] Items { get; set; }
    }

    public partial class Item_ChannelInfo
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public Snippet_ChannelInfo Snippet { get; set; }
    }

    public partial class Snippet_ChannelInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("customUrl")]
        public string CustomUrl { get; set; }

        [JsonProperty("publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonProperty("thumbnails")]
        public ThumbnailsChannel Thumbnails { get; set; }

        [JsonProperty("localized")]
        public LocalizedChannel Localized { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public partial class LocalizedChannel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class ThumbnailsChannel
    {
        [JsonProperty("default")]
        public DefaultChannel Default { get; set; }

        [JsonProperty("medium")]
        public DefaultChannel Medium { get; set; }

        [JsonProperty("high")]
        public DefaultChannel High { get; set; }
    }

    public partial class DefaultChannel
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

    public partial class PageInfoChannel
    {
        [JsonProperty("totalResults")]
        public long TotalResults { get; set; }

        [JsonProperty("resultsPerPage")]
        public long ResultsPerPage { get; set; }
    }

    public partial class YouTubeChannelInfoJM
    {
        public static YouTubeChannelInfoJM FromJson(string json) => JsonConvert.DeserializeObject<YouTubeChannelInfoJM>(json, Converter.Settings);
    }

    public static class SerializeChannel
    {
        public static string ToJson(this YouTubeChannelInfoJM self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class ConverterChannel
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