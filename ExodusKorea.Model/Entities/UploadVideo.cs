using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class UploadVideo
    {
        public int UploadVideoId { get; set; }
        public string Email { get; set; }
        public string YoutubeAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string IpAddress { get; set; }
        public string Country { get; set; }
    }
}