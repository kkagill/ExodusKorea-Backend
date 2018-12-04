using System;

namespace ExodusKorea.Model.Entities
{
    public class CountryInfoKOTRA
    {
        public int CountryInfoKOTRAId { get; set; }
        public string PromosingField { get; set; }
        public string SettlementGuide { get; set; }
        public string LivingCondition { get; set; }
        public string ImmigrationVisa { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}