using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class NewsVM
    {
        public int NewsId { get; set; }
        public string Topic { get; set; }
        public IEnumerable<NewsDetailVM> NewsDetails { get; set; }
    }
}