using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class VideoPost
    {
        public int VideoPostId { get; set; }
        public string Uploader { get; set; }
        public DateTime UploadedDate { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
        public string YouTubeVideoId { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<VideoPostLike> VideoPostLikes { get; set; }
    }
}