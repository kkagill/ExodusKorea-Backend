using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}