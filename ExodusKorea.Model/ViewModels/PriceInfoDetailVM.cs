using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class PriceInfoDetailVM
    {
        public string City { get; set; }
        public string Currency { get; set; }
        public PI_RentVM Rent { get; set; }
        public PI_GroceriesVM Groceries { get; set; }
        public PI_RestaurantVM Restaurant { get; set; }
        public PI_EtcVM Etc { get; set; }
    }
}