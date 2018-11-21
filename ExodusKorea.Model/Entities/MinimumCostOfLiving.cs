using System;

namespace ExodusKorea.Model.Entities
{
    public class MinimumCostOfLiving
    {
        public int MinimumCostOfLivingId { get; set; }
        public int CountryInfoId { get; set; }
        public int CityId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal Rent { get; set; }
        public decimal Transportation { get; set; }
        public decimal Food { get; set; }
        public decimal Cell { get; set; }
        public decimal Internet { get; set; }
        public string Etc { get; set; }
        public string IpAddress { get; set; }
        public string NickName { get; set; }
        public decimal Total { get; set; }
        public DateTime DateCreated { get; set; }
    }
}