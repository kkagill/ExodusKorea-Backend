using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class NewSalaryInfoVM
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public decimal High { get; set; }
        [Required]
        public decimal Low { get; set; }
        [Required]
        public decimal Median { get; set; }
        [Required]
        public string Occupation { get; set; }
    }
}