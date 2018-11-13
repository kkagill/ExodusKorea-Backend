using System;

namespace ExodusKorea.Model.Entities
{
    public class PI_Restaurant
    {
        public int PI_RestaurantId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal MealPerOne { get; set; }
        public decimal BigMacMeal { get; set; }
        public decimal Cappuccino { get; set; }

        public int PriceInfoId { get;set; }
        public PriceInfo PriceInfo { get; set; }
    }
}