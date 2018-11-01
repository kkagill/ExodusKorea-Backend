using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace ExodusKorea.Model.JsonModels
{    
    public partial class YouTubeCommentsJM
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public ItemSnippet Snippet { get; set; }

        [JsonProperty("replies", NullValueHandling = NullValueHandling.Ignore)]
        public Replies Replies { get; set; }
    }

    public partial class Replies
    {
        [JsonProperty("comments")]
        public Comment[] Comments { get; set; }
    }

    public partial class Comment
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public TopLevelCommentSnippet Snippet { get; set; }
    }

    public partial class TopLevelCommentSnippet
    {
        [JsonProperty("authorDisplayName")]
        public string AuthorDisplayName { get; set; }

        [JsonProperty("authorProfileImageUrl")]
        public Uri AuthorProfileImageUrl { get; set; }

        [JsonProperty("authorChannelUrl")]
        public Uri AuthorChannelUrl { get; set; }

        [JsonProperty("authorChannelId")]
        public AuthorChannelId AuthorChannelId { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("textDisplay")]
        public string TextDisplay { get; set; }

        [JsonProperty("textOriginal")]
        public string TextOriginal { get; set; }

        [JsonProperty("parentId", NullValueHandling = NullValueHandling.Ignore)]
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

    public partial class AuthorChannelId
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class ItemSnippet
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("topLevelComment")]
        public Comment TopLevelComment { get; set; }

        [JsonProperty("canReply")]
        public bool CanReply { get; set; }

        [JsonProperty("totalReplyCount")]
        public long TotalReplyCount { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("totalResults")]
        public long TotalResults { get; set; }

        [JsonProperty("resultsPerPage")]
        public long ResultsPerPage { get; set; }
    }

    public partial class YouTubeCommentsJM
    {
        public static YouTubeCommentsJM FromJson(string json) => JsonConvert.DeserializeObject<YouTubeCommentsJM>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this YouTubeCommentsJM self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}