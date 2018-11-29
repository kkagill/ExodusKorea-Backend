using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class VideoCommentReplyVM : IValidatableObject
    {
        public long VideoCommentReplyId { get; set; }
        public long VideoCommentId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int Likes { get; set; }
        public string UserId { get; set; }
        public string RepliedTo { get; set; }
        public string CountryEN { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new VideoCommentReplyVMValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}