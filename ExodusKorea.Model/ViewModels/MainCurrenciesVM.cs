using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class MainCurrenciesVM
    {
        public string USD { get; set; }
        public string CAD { get; set; }
        public string AUD { get; set; }
        public string NZD { get; set; }
        public DateTime Today { get; set; }
    }
}