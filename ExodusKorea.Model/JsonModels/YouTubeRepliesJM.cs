using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace ExodusKorea.Model.JsonModels
{    
    public partial class YouTubeRepliesJM
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public Item_R[] Items { get; set; }
    }

    public partial class Item_R
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public Snippet Snippet { get; set; }
    }

    public partial class Snippet
    {
        [JsonProperty("authorDisplayName")]
        public string AuthorDisplayName { get; set; }

        [JsonProperty("authorProfileImageUrl")]
        public Uri AuthorProfileImageUrl { get; set; }

        [JsonProperty("authorChannelUrl")]
        public Uri AuthorChannelUrl { get; set; }

        [JsonProperty("authorChannelId")]
        public AuthorChannelId_R AuthorChannelId { get; set; }

        [JsonProperty("textDisplay")]
        public string TextDisplay { get; set; }

        [JsonProperty("textOriginal")]
        public string TextOriginal { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("canRate")]
        public bool CanRate { get; set; }

        [JsonProperty("viewerRating")]
        public string ViewerRating { get; set; }

        [JsonProperty("likeCount")]
        public long LikeCount { get; set; }

        [JsonProperty("publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public partial class AuthorChannelId_R
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class PageInfo_R
    {
        [JsonProperty("resultsPerPage")]
        public long ResultsPerPage { get; set; }
    }

    public partial class YouTubeRepliesJM
    {
        public static YouTubeRepliesJM FromJson(string json) => JsonConvert.DeserializeObject<YouTubeRepliesJM>(json, Converter.Settings);
    }
}