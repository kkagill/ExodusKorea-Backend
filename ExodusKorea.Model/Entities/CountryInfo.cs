﻿using System;

namespace ExodusKorea.Model.Entities
{
    public class CountryInfo
    {
        public int CountryInfoId { get; set; }
        public string CapitalCity { get; set; }
        public string MajorCities { get; set; }
        public string Population { get; set; }
        public string Languages { get; set; }
        public string PerCapitaGDP { get; set; }
        public string Currency { get; set; }
        public string CountryLink { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}