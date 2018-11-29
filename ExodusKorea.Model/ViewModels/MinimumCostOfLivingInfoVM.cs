using System.Collections.Generic;

namespace ExodusKorea.Model.ViewModels
{    
    public class MinimumCostOfLivingInfoVM
    {
        public int CountryId { get; set; }
        public string CountryKR { get; set; }
        public string CountryEN { get; set; }
        public string BaseCurrency { get; set; }
        public List<CityMinimumVM> CityMinimums { get; set; }
    }

    public class CityMinimumVM
    {
        public string City { get; set; }
        public decimal AvgCostOfLiving { get; set; }
    }
}