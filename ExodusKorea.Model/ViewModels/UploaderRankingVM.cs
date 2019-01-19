using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class UploaderRankingVM
    {
        public string Name { get; set; }
        public string ThumbnailDefaultUrl { get; set; }
        public int UploaderId { get; set; }
        public SpecificInfoVM SpecificInfo { get; set; }
    }

    public class SpecificInfoVM
    {       
        public int TotalScore { get; set; }
        public int VideoCount { get; set; }
        public long LikesSum { get; set; }
    }
}