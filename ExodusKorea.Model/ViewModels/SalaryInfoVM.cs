using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class SalaryInfoVM
    {
        public int SalaryInfoId { get; set; }
        public string Country { get; set; }
        public string Occupation { get; set; }
        public string Currency { get; set; }
        public decimal Low { get; set; }
        public decimal Median { get; set; }
        public decimal High { get; set; }
        public bool IsDisplayable { get; set; }
    }
}