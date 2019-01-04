using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class YouTubeCommentReplyVM
    {
        public string AuthorDisplayName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public int Likes { get; set; }
    }
}