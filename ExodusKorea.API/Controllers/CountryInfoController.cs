using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class CountryInfoController : Controller
    {
        private readonly ICountryInfoRepository _repository;

        public CountryInfoController(ICountryInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("country-info-canada")]
        public async Task<IActionResult> GetCountryInfoCanadaAsync()
        {
            var countryInfo = await _repository.GetCountryInfoCanada();

            if (countryInfo == null)
                return NotFound();

            return new OkObjectResult(countryInfo);
        }
    }
}
