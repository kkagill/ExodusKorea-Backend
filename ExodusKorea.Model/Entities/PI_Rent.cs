using System;

namespace ExodusKorea.Model.Entities
{
    public class PI_Rent
    {
        public int PI_RentId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal OneBedRoomCenter { get; set; }
        public decimal TwoBedRoomCenter { get; set; }
        public decimal OneBedRoomOutside { get; set; }
        public decimal TwoBedRoomOutside { get; set; }

        public int PriceInfoId { get; set; }
        public PriceInfo PriceInfo { get; set; }
    }
}