using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class VideoPostVM
    {
        public int VideoPostId { get; set; }
        public string Uploader { get; set; }
        public DateTime UploadedDate { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
        public string YouTubeVideoId { get; set; }
        public int CountryId { get; set; }
        public string CountryKR { get; set; }
        public string CountryEN { get; set; }
    }
}