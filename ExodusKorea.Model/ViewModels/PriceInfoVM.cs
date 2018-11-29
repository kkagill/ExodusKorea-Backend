using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class PriceInfoVM
    {
        public string CountryKR { get; set; }
        public string CountryEN { get; set; }
        public string CostOfLiving { get; set; }
        public string CostOfLivingIcon { get; set; }
        public string Rent { get; set; }
        public string RentIcon { get; set; }
        public string Groceries { get; set; }
        public string GroceriesIcon { get; set; }
        public string RestaurantPrice { get; set; }
        public string RestaurantPriceIcon { get; set; }
    }
}