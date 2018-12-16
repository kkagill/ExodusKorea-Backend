using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class HttpResponseException
    {
        public long HttpResponseExceptionId { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string IpAddress { get; set; }
    }
}