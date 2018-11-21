using System.Collections.Generic;

namespace ExodusKorea.Model.ViewModels
{    
    public class MinimumCostOfLivingInfoVM
    {
        public string Country { get; set; }
        public string CountryInEng { get; set; }
        public string BaseCurrency { get; set; }
        public List<CityMinimumVM> CityMinimums { get; set; }
    }

    public class CityMinimumVM
    {
        public string City { get; set; }
        public decimal AvgCostOfLiving { get; set; }
    }
}