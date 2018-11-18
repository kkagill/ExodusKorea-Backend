using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class NotificationVM
    {
        public long NotificationId { get; set; }
        public long VideoCommentId { get; set; }
        public long VideoCommentReplyId { get; set; }
        public int VideoPostId { get; set; }
        public string YouTubeVideoId { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public bool HasRead { get; set; }
    }
}