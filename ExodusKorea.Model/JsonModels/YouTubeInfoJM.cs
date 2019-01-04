using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExodusKorea.Model.JsonModels
{
    public partial class YouTubeInfoJM
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public Item_Info[] Items { get; set; }
    }

    public partial class Item_Info
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public Snippet_Info Snippet { get; set; }

        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }
    }

    public partial class Snippet_Info
    {
        [JsonProperty("publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("thumbnails")]
        public Thumbnails Thumbnails { get; set; }

        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonProperty("categoryId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CategoryId { get; set; }

        [JsonProperty("liveBroadcastContent")]
        public string LiveBroadcastContent { get; set; }

        [JsonProperty("localized")]
        public Localized Localized { get; set; }
    }

    public partial class Localized
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class Thumbnails
    {
        [JsonProperty("default")]
        public Default Default { get; set; }

        [JsonProperty("medium")]
        public Default Medium { get; set; }

        [JsonProperty("high")]
        public Default High { get; set; }

        [JsonProperty("standard")]
        public Default Standard { get; set; }
    }

    public partial class Default
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

    public partial class Statistics
    {
        [JsonProperty("viewCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ViewCount { get; set; }

        [JsonProperty("likeCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LikeCount { get; set; }

        [JsonProperty("dislikeCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long DislikeCount { get; set; }

        [JsonProperty("favoriteCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long FavoriteCount { get; set; }

        [JsonProperty("commentCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CommentCount { get; set; }
    }

    public partial class YouTubeInfoJM
    {
        public static YouTubeInfoJM FromJson(string json) => JsonConvert.DeserializeObject<YouTubeInfoJM>(json, Converter.Settings);
    }  

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}