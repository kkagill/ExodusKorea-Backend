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

namespace ExodusKorea.API.Services
{
    public class YouTubeCommentService : IYouTubeCommentService
    {
        private readonly YoutubeData _apiKey;

        public YouTubeCommentService(IOptionsSnapshot<YoutubeData> youtubeDataAccessor)
        {
            _apiKey = youtubeDataAccessor.Value;
        }

        public async Task<YouTubeCommentVM> GetYouTubeCommentsByVideoId(string videoId)
        {
            var httpClient = new HttpClient();
            var res = await httpClient
                .GetAsync($"https://www.googleapis.com/youtube/v3/commentThreads?part=snippet&videoId={videoId}&maxResults=100&key={_apiKey.Key}");
          
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
                        Likes = c.Snippet.TopLevelComment.Snippet.LikeCount.ToString()
                    });

            while (result.NextPageToken != null)
            {
                var resNext = await httpClient
                    .GetAsync($"https://www.googleapis.com/youtube/v3/commentThreads?pageToken={result.NextPageToken}&part=snippet&videoId={videoId}&maxResults=100&key={_apiKey.Key}");

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
                            Likes = c.Snippet.TopLevelComment.Snippet.LikeCount.ToString()
                        });

                result.NextPageToken = nextResult.NextPageToken;
            }

            return youTubeCommentVM;
        }
    }
}
