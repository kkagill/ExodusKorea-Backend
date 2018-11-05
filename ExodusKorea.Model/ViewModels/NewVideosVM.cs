using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class NewVideosVM
    {
        public int NewVideoId { get; set; }
        public string UploaderName { get; set; }
        public DateTime UploadedDate { get; set; }
        public string YouTubeVideoId { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public string CountryInEnglish { get; set; }
    }
}