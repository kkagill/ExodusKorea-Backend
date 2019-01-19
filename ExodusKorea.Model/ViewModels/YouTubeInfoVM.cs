using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class YouTubeInfoVM
    {       
        public long Likes { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public string ChannelId { get; set; }
    }
}