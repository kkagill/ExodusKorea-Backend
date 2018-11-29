using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class CountryInfoVM
    {
        public string CountryKR { get; set; }
        public string CountryEN { get; set; }
        public string CapitalCity { get; set; }
        public string MajorCities { get; set; }
        public string Population { get; set; }
        public string Languages { get; set; }
        public string PerCapitaGDP { get; set; }
        public string Currency { get; set; }
        public string CountryLink { get; set; }
    }
}