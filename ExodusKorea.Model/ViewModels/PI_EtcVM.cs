using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class PI_EtcVM
    {
        public decimal Bus { get; set; }
        public decimal Subway { get; set; }
        public decimal Gas { get; set; }
        public decimal Internet { get; set; }
    }
}