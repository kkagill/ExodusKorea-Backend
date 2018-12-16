using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class MyVideos
    {
        public int MyVideosId { get; set; }
        public string ApplicationUserId { get; set; }
        public int VideoPostId { get; set; }
        public VideoPost VideoPost { get; set; }
    }
}