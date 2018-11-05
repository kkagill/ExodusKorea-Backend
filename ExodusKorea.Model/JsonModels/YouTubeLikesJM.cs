using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExodusKorea.Model.JsonModels
{
    public partial class YouTubeLikesJM
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }      

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }
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

    public partial class YouTubeLikesJM
    {
        public static YouTubeLikesJM FromJson(string json) => JsonConvert.DeserializeObject<YouTubeLikesJM>(json, Converter.Settings);
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