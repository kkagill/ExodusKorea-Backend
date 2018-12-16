using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class SiteException
    {
        public long SiteExceptionId { get; set; }
        public string PageUrl { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Exception { get; set; }
        public string IpAddress { get; set; }
    }
}