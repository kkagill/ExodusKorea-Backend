using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExodusKorea.API.Services;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.JsonModels;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class CardDetailController : Controller
    {
        private readonly ICardDetailRepository _repository;
        private readonly ICurrencyRatesService _currencyRate;
        private readonly IYouTubeCommentService _youTubeComment;

        public CardDetailController(ICardDetailRepository repository,
                                    ICurrencyRatesService currencyRate,
                                    IYouTubeCommentService youTubeComment)
        {
            _repository = repository;
            _currencyRate = currencyRate;
            _youTubeComment = youTubeComment;
        }

        [HttpGet]
        [Route("{videoId}/country-info", Name = "CountryInfo")]
        public async Task<IActionResult> GetCountryInfo(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
                return BadRequest();

            var country = await _repository.GetCountryByVideoId(videoId);
            var countryInfo = await _repository.GetCountryInfoByCountry(country);

            if (countryInfo == null)
                return BadRequest();

            var countryInfoVM = Mapper.Map<CountryInfo, CountryInfoVM>(countryInfo);       

            return new OkObjectResult(countryInfoVM);
        }

        [HttpGet]
        [Route("{videoId}/price-info", Name = "PriceInfo")]
        public async Task<IActionResult> GetPriceInfo(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
                return BadRequest();

            var country = await _repository.GetCountryByVideoId(videoId);
            var kPriceInfo = await _repository.GetPriceInfoByCountry("대한민국");            
            var priceInfo = await _repository.GetPriceInfoByCountry(country);

            if (kPriceInfo == null || priceInfo == null)
                return BadRequest();

            var priceInfoVM = new PriceInfoVM
            {
                Country = country,
                CostOfLiving = ComparePrices(kPriceInfo.CostOfLivingIndex, priceInfo.CostOfLivingIndex),
                CostOfLivingIcon = ComparePricesIcon(kPriceInfo.CostOfLivingIndex, priceInfo.CostOfLivingIndex),
                Rent = ComparePrices(kPriceInfo.RentIndex, priceInfo.RentIndex),
                RentIcon = ComparePricesIcon(kPriceInfo.RentIndex, priceInfo.RentIndex),
                Groceries = ComparePrices(kPriceInfo.GroceriesIndex, priceInfo.GroceriesIndex),
                GroceriesIcon = ComparePricesIcon(kPriceInfo.GroceriesIndex, priceInfo.GroceriesIndex),
                RestaurantPrice = ComparePrices(kPriceInfo.RestaurantPriceIndex, priceInfo.RestaurantPriceIndex),
                RestaurantPriceIcon = ComparePricesIcon(kPriceInfo.RestaurantPriceIndex, priceInfo.RestaurantPriceIndex)
            };

            return new OkObjectResult(priceInfoVM);
        }

        [HttpGet]
        [Route("{videoId}/currency-info", Name = "CurrencyInfo")]
        public async Task<IActionResult> GetCurrencyInfo(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
                return BadRequest();

            var country = await _repository.GetCountryByVideoId(videoId);
            var baseCurrency = GetBaseCurrency(country);
            var krwRate = await _currencyRate.GetKRWRateByCountry(baseCurrency);
          
            if (string.IsNullOrEmpty(krwRate))
                return BadRequest();

            return new OkObjectResult(new CurrencyInfoVM
            {
                Country = country,
                BaseCurrency = baseCurrency,
                KrwRate = krwRate,
                Now = DateTime.Now
            });
        }

        [HttpGet]
        [Route("{videoId}/youtube-comments", Name = "YouTubeComments")]
        public async Task<IActionResult> GetYouTubeComments(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
                return BadRequest();
            
            var comments = await _youTubeComment.GetYouTubeCommentsByVideoId(videoId);

            if (comments == null)
                return BadRequest();          

            return new OkObjectResult(comments);
        }

        #region Private Functions
        private string ComparePrices(decimal korea, decimal country)
        {
            var difference = korea - country;
            var result = "";

            if (Math.Abs(difference) > 0 && Math.Abs(difference) <= 5)
                result = "비슷";
            else if (country > korea)
                result = "높음";
            else if (country < korea)
                result = "낮음";

            return result;
        }

        private string ComparePricesIcon(decimal korea, decimal country)
        {
            var difference = korea - country;
            var result = "";

            if (Math.Abs(difference) > 0 && Math.Abs(difference) <= 5)
                result = "minus";
            else if (country > korea)
                result = "chevron-up";
            else if (country < korea)
                result = "chevron-down";

            return result;
        }

        private string GetBaseCurrency(string country)
        {
            var baseCurrency = "";

            switch (country)
            {
                case "캐나다":
                    baseCurrency = "CAD";
                    break;
                case "미국":
                    baseCurrency = "USD";
                    break;
            }

            return baseCurrency;
        }
        #endregion
    }
}
