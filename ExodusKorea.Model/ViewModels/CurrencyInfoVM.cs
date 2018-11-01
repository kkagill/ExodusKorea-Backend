using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class CurrencyInfoVM
    {
        public string Country { get; set; }
        public string BaseCurrency { get; set; }
        public string KrwRate { get; set; }
        public DateTime Now { get; set; }
    }
}