using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class CategoryCountryVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}