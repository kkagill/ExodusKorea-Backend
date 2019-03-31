using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class JobsInDemandVM
    {
        public string CountryKR { get; set; }      
        public List<Detail> Details { get; set; }
    }

    public class Detail
    {
        public int JobsInDemandId { get; set; }
        public string TitleKR { get; set; }
        public string TitleEN { get; set; }
        public string Description { get; set; }
        public bool HasVideoPost { get; set; }
        public bool IsRecommended { get; set; }
        public string DifficultyLevel { get; set; }
        public string Link { get; set; }
        public string JobSite { get; set; }
        public decimal Salary { get; set; }
        public string Currency { get; set; }
    }
}