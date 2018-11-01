using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class YouTubeCommentVM
    {
        public YouTubeCommentVM()
        {
            Comments = new List<Comment>();
        }

        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public string AuthorDisplayName { get; set; }
        public string TextDisplay { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Likes { get; set; }
    }
}