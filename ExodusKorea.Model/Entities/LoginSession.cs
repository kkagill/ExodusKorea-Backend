using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class LoginSession
    {
        public long LoginSessionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public string LoginType { get; set; }
    }
}