using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System;
using ExodusKorea.Model.JsonModels;
using ExodusKorea.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using ExodusKorea.Model;
using ExodusKorea.Model.ViewModels;
using System.Collections.Generic;

namespace ExodusKorea.API.Services
{
    public class YoutubeService : IYouTubeService
    {
        private readonly YoutubeData _apiKey;

        public YoutubeService(IOptionsSnapshot<YoutubeData> youtubeDataAccessor)
        {
            _apiKey = youtubeDataAccessor.Value;
        }

        public async Task<YouTubeChannelInfoVM> GetYouTubeChannelInfoByChannelId(string channelId)
        {
            var httpClient = new HttpClient();
            var res = await httpClient
                .GetAsync($"https://www.googleapis.com/youtube/v3/channels?part=snippet&id={channelId}&key={_apiKey.Key}");
         
            if (res.StatusCode != HttpStatusCode.OK)
                return null;

            var jsonRes = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<YouTubeChannelInfoJM>(jsonRes);
            YouTubeChannelInfoVM youTubeChannelInfoVM = null;

            if (result != null)
                foreach (var c in result.Items)
                    youTubeChannelInfoVM = new YouTubeChannelInfoVM
                    {
                        ThumbnailUrl = c.Snippet.Thumbnails.High.Url.AbsoluteUri
                    };

            return youTubeChannelInfoVM;
        }

        public async Task<YouTubeInfoVM> GetYouTubeInfoByVideoId(string videoId)
        {
            var httpClient = new HttpClient();
            var res = await httpClient
                .GetAsync($"https://www.googleapis.com/youtube/v3/videos?id={videoId}&key={_apiKey.Key}&part=snippet,statistics");
            
            if (res.StatusCode != HttpStatusCode.OK)
                return null;

            var jsonRes = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<YouTubeInfoJM>(jsonRes);
            YouTubeInfoVM youTubeInfoVM = null;

            if (result != null)
                foreach (var c in result.Items)
                    youTubeInfoVM = new YouTubeInfoVM
                    {
                        Likes = c.Statistics.LikeCount,
                        Title = c.Snippet.Title,
                        Owner = c.Snippet.ChannelTitle,
                        ChannelId = c.Snippet.ChannelId
                    };

            return youTubeInfoVM;
        }

        public async Task<YouTubeCommentVM> GetYouTubeCommentsByVideoId(string videoId)
        {
            var httpClient = new HttpClient();
            var res = await httpClient
                .GetAsync($"https://www.googleapis.com/youtube/v3/commentThreads?part=snippet,replies&videoId={videoId}&maxResults=100&key={_apiKey.Key}");
          
            if (res.StatusCode != HttpStatusCode.OK)
                return null;             

            var jsonRes = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<YouTubeCommentsJM>(jsonRes);
            var youTubeCommentVM = new YouTubeCommentVM();            

            if (result != null)
                foreach (var c in result.Items)
                    youTubeCommentVM.Comments.Add(new Model.ViewModels.Comment
                    {
                        AuthorDisplayName = c.Snippet.TopLevelComment.Snippet.AuthorDisplayName,
                        TextDisplay = c.Snippet.TopLevelComment.Snippet.TextDisplay,
                        UpdatedAt = c.Snippet.TopLevelComment.Snippet.UpdatedAt,
                        Likes = c.Snippet.TopLevelComment.Snippet.LikeCount.ToString(),
                        TotalReplyCount = c.Snippet.TotalReplyCount,
                        //ParentId = c.Snippet.TopLevelComment.Id,
                        Replies = AddReplies(c.Replies)
                    });

            int count = 2;
            while (result.NextPageToken != null && count > 0)
            {
                var resNext = await httpClient
                    .GetAsync($"https://www.googleapis.com/youtube/v3/commentThreads?pageToken={result.NextPageToken}&part=snippet,replies&videoId={videoId}&maxResults=100&key={_apiKey.Key}");

                if (resNext.StatusCode != HttpStatusCode.OK)
                    return youTubeCommentVM;

                var jsonNextRes = await resNext.Content.ReadAsStringAsync();
                var nextResult = JsonConvert.DeserializeObject<YouTubeCommentsJM>(jsonNextRes);

                if (nextResult != null)
                    foreach (var c in nextResult.Items)
                        youTubeCommentVM.Comments.Add(new Model.ViewModels.Comment
                        {
                            AuthorDisplayName = c.Snippet.TopLevelComment.Snippet.AuthorDisplayName,
                            TextDisplay = c.Snippet.TopLevelComment.Snippet.TextDisplay,
                            UpdatedAt = c.Snippet.TopLevelComment.Snippet.UpdatedAt,
                            Likes = c.Snippet.TopLevelComment.Snippet.LikeCount.ToString(),
                            TotalReplyCount = c.Snippet.TotalReplyCount,
                            //ParentId = c.Snippet.TopLevelComment.Id,
                            Replies = AddReplies(c.Replies)
                        });

                result.NextPageToken = nextResult.NextPageToken;
                count--;             
            }

            return youTubeCommentVM;
        }

        //public async Task<YouTubeCommentVM> GetYouTubeRepliesByParentId(string parentId)
        //{
        //    var httpClient = new HttpClient();
        //    var res = await httpClient
        //        .GetAsync($"https://www.googleapis.com/youtube/v3/comments?part=snippet&parentId={parentId}&maxResults=100&key={_apiKey.Key}");

        //    if (res.StatusCode != HttpStatusCode.OK)
        //        return null;

        //    var jsonRes = await res.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<YouTubeRepliesJM>(jsonRes);
        //    var youTubeCommentVM = new YouTubeCommentVM();

        //    if (result != null)
        //        foreach (var c in result.Items)
        //            youTubeCommentVM.Comments.Add(new Model.ViewModels.Comment
        //            {
        //                AuthorDisplayName = c.Snippet.AuthorDisplayName,
        //                TextDisplay = c.Snippet.TextDisplay,
        //                UpdatedAt = c.Snippet.UpdatedAt,
        //                Likes = c.Snippet.LikeCount.ToString()
        //            });          

        //    return youTubeCommentVM;
        //}

        private List<Reply> AddReplies(Replies replies) 
        {
            var mappedReplies = new List<Reply>();

            if (replies != null)
                foreach (var r in replies.Comments)
                    mappedReplies.Add(new Reply
                    {
                        AuthorDisplayName = r.Snippet.AuthorDisplayName,
                        TextDisplay = r.Snippet.TextDisplay,
                        Likes = r.Snippet.LikeCount.ToString(),
                        UpdatedAt = r.Snippet.UpdatedAt
                    });

            return mappedReplies;
        }
    }
}
