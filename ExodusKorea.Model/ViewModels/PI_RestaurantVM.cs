using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class PI_RestaurantVM
    {
        public decimal MealPerOne { get; set; }
        public decimal BigMacMeal { get; set; }
        public decimal Cappuccino { get; set; }
    }
}