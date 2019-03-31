using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class KotraNewsVM
    {       
        public string NewsTitl { get; set; }
        public string NewsWrterNm { get; set; }
        public DateTimeOffset NewsWrtDt { get; set; }
        public string NewsBdt { get; set; }
        public string OvrofInfo { get; set; }
    }
}