using System;

namespace ExodusKorea.Model.Entities
{
    public class PI_Groceries
    {
        public int PI_GroceriesId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal Milk { get; set; }
        public decimal Eggs { get; set; }
        public decimal ChickenBreasts { get; set; }
        public decimal Apple { get; set; }
        public decimal Potatoes { get; set; }
        public decimal Water { get; set; }
        public decimal Cigarettes { get; set; }

        public int PriceInfoId { get; set; }
        public PriceInfo PriceInfo { get; set; }
    }
}