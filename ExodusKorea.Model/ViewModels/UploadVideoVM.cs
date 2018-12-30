using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class UploadVideoVM
    {
        [Required] 
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(11)]
        public string YoutubeAddress { get; set; }     
    }
}