﻿using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class MinimumCostOfLivingVM
    {     
        public string Country { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public decimal Rent { get; set; }
        public decimal Transportation { get; set; }
        public decimal Food { get; set; }
        public decimal Cell { get; set; }
        public decimal Internet { get; set; }
        public decimal Other { get; set; }
        public string Etc { get; set; }
        public string NickName { get; set; }
        public decimal Total { get; set; }
        public DateTime DateCreated { get; set; }
        public string AuthorCountryEN { get; set; }
    }
}