using ExodusKorea.Model.ViewModels.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class ApplicationUserVM
    {               
        public string Email { get; set; }
        public string NickName { get; set; }
    }
}