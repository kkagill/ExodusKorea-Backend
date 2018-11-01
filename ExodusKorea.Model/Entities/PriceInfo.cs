using System;

namespace ExodusKorea.Model.Entities
{
    public class PriceInfo
    {
        public int PriceInfoId { get; set; }
        public string Country { get; set; }
        public decimal CostOfLivingIndex { get; set; }
        public decimal RentIndex { get; set; }
        public decimal GroceriesIndex { get; set; }
        public decimal RestaurantPriceIndex { get; set; }   
    }
}