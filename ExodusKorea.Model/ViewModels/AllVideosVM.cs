using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{
    public class AllVideosVM
    {
        public string CountryKR { get; set; }
        public IEnumerable<VideoPostVM> VideoPosts { get; set; }
    }
}