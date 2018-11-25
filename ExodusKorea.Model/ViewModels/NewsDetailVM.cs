using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class NewsDetailVM
    {
        public int NewsDetailId { get; set; }
        public string Subject { get; set; }
        public string Country { get; set; }
        public string Department { get; set; }
        public string Creator { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public int Views { get; set; }
        public string Thumbnail { get; set; }

        public int NewsId { get; set; }
    }
}