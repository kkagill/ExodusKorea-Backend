using System;

namespace ExodusKorea.Model.Entities
{
    public class PI_Etc
    {
        public int PI_EtcId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal Bus { get; set; }
        public decimal Subway { get; set; }
        public decimal Gas { get; set; }
        public decimal Internet { get; set; }

        public int PriceInfoId { get; set; }
        public PriceInfo PriceInfo { get; set; }
    }
}