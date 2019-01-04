using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class JobSite
    {
        public int JobSiteId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }       
        public string Category { get; set; }

        public int CountryId { get; set; }
    }
}