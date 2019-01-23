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
        public string Description { get; set; }
        public bool HasVideoPost { get; set; }
    }
}