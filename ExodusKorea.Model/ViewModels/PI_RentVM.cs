using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class PI_RentVM
    {
        public decimal OneBedRoomCenter { get; set; }
        public decimal TwoBedRoomCenter { get; set; }
        public decimal OneBedRoomOutside { get; set; }
        public decimal TwoBedRoomOutside { get; set; }
    }
}