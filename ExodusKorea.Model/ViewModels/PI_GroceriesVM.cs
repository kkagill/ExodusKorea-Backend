using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class PI_GroceriesVM
    {
        public decimal Milk { get; set; }
        public decimal Eggs { get; set; }
        public decimal ChickenBreasts { get; set; }
        public decimal Apple { get; set; }
        public decimal Potatoes { get; set; }
        public decimal Water { get; set; }
        public decimal Cigarettes { get; set; }
    }
}