using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExodusKorea.Model.Entities
{
    public class VideoPost
    {
        public int VideoPostId { get; set; }
        public DateTime UploadedDate { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
        public string YouTubeVideoId { get; set; }
        public string SharerId { get; set; }
        public bool IsDisabled { get; set; }
        public byte IsGoogleDriveVideo { get; set; }       

        public int CountryId { get; set; }     
        public int CareerId { get; set; }      
        public int CategoryId { get; set; }
        public int? SalaryInfoId { get; set; }
        public int? UploaderId { get; set; }
        public Country Country { get; set; }
        public Career Career { get; set; }
        public Category Category { get; set; }
        public SalaryInfo SalaryInfo { get; set; }
        public Uploader Uploader { get; set; }
        public ICollection<VideoPostLike> VideoPostLikes { get; set; }
    }
}