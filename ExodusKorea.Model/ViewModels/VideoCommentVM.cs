using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class VideoCommentVM : IValidatableObject
    {
        public long VideoCommentId { get; set; }      
        public string VideoId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int Likes { get; set; }
        public bool IsYouTubeComment { get; set; }
        public long TotalReplyCount { get; set; }
        //public string ParentId { get; set; }
        public int VideoPostId { get; set; }
        public string UserId { get; set; }
        public string CountryEN { get; set; }
        public bool IsSharer { get; set; }

        public IEnumerable<VideoCommentReplyVM> VideoCommentReplies { get; set; }
        public IEnumerable<YouTubeCommentReplyVM> YouTubeCommentReplies { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new VideoCommentVMValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}