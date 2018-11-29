using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string NameKR { get; set; }
        public string NameEN { get; set; }
        public string BaseCurrency { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}