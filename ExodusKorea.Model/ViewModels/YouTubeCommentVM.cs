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

        public string Likes { get; set; }
        public List<Comment> Comments { get; set; }       
    }

    public class Comment
    {
        public Comment()
        {
            Replies = new List<Reply>();
        }

        public string AuthorDisplayName { get; set; }
        public string TextDisplay { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Likes { get; set; }
        public long TotalReplyCount { get; set; }
        public string ParentId { get; set; }
        public List<Reply> Replies { get; set; }
    }

    public class Reply
    {
        public string AuthorDisplayName { get; set; }
        public string TextDisplay { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Likes { get; set; }
    }
}