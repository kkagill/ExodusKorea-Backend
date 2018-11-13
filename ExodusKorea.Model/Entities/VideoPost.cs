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
        public string Country { get; set; }
        public string CountryInEng { get; set; }

        public ICollection<VideoPostLike> VideoPostLikes { get; set; }
    }
}