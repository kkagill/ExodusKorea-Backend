using System;

namespace ExodusKorea.Model.Entities
{
    public class NewVideo
    {
        public int NewVideoId { get; set; }
        public string UploaderName { get; set; }
        public DateTime UploadedDate { get; set; }
        public string YouTubeVideoId { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }       
        public int Likes { get; set; }
        public string CountryInEnglish { get; set; }
    }
}