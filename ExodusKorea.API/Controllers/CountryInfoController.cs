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

        //[HttpGet]
        //[Route("{country}/promising-field", Name = "GetPromisingFieldByCountry")]
        //public async Task<IActionResult> GetPromisingFieldByCountry(string country)
        //{
        //    if (string.IsNullOrWhiteSpace(country))
        //        return NotFound();

        //    var content = await _repository.GetPromisingFieldByCountry(country);

        //    if (content == null)
        //        return NotFound();

        //    return new OkObjectResult(content);
        //}

        [HttpGet]
        [Route("promising-fields")]
        public IActionResult GetAllPromisingField()
        {         
            var allPromisingFields = _repository.GetAllPromisingFields();

            if (allPromisingFields == null)
                return NotFound();

            return new OkObjectResult(allPromisingFields);
        }

        [HttpGet]
        [Route("settlement-guides")]
        public IActionResult GetAllSettlementGuide()
        {
            var allSettlementGuides = _repository.GetAllSettlementGuides();

            if (allSettlementGuides == null)
                return NotFound();

            return new OkObjectResult(allSettlementGuides);
        }

        [HttpGet]
        [Route("living-conditions")]
        public IActionResult GetAllLivingConditions()
        {
            var allLivingConditions = _repository.GetAllLivingConditions();

            if (allLivingConditions == null)
                return NotFound();

            return new OkObjectResult(allLivingConditions);
        }

        [HttpGet]
        [Route("immigration-visas")]
        public IActionResult GetAllImmigrationVisas()
        {
            var allImmigrationVisas = _repository.GetAllImmigrationVisas();

            if (allImmigrationVisas == null)
                return NotFound();

            return new OkObjectResult(allImmigrationVisas);
        }
    }
}
