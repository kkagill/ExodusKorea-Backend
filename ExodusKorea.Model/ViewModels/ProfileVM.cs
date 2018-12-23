using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class ProfileVM
    {               
        public string Email { get; set; }
        public string NickName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateVisitedRecent { get; set; }
        public int VisitCount { get; set; }
        public bool HasCanceledSubscription { get; set; }
    }
}