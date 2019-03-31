using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class CategoryCountryCareerUploaderVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Career> Careers { get; set; }
        public IEnumerable<Uploader> Uploaders { get; set; }
    }
}