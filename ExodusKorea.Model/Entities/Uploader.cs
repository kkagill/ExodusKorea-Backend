using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class Uploader
    {
        public int UploaderId { get; set; }
        public string Name { get; set; }
        public ICollection<VideoPost> VideoPosts { get; set; }
    }
}