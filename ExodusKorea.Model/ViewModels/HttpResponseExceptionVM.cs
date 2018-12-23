using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class HttpResponseExceptionVM
    {               
        public int? Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string Error { get; set; }
        public string IpAddress { get; set; }
        public string Message { get; set; }
        public string PageUrl { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
    }
}