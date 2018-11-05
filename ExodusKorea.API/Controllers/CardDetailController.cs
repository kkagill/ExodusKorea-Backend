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
        private readonly IVideoCommentRepository _vcRepository;
        private readonly ICurrencyRatesService _currencyRate;
        private readonly IYouTubeService _youTube;

        public CardDetailController(ICardDetailRepository repository,
                                    IVideoCommentRepository vcRepository,
                                    ICurrencyRatesService currencyRate,
                                    IYouTubeService youTube)
        {
            _repository = repository;
            _vcRepository = vcRepository;
            _currencyRate = currencyRate;
            _youTube = youTube;
        }

        [HttpGet]
        [Route("{newVideoId}/country-info", Name = "CountryInfo")]
        public async Task<IActionResult> GetCountryInfo(int newVideoId)
        {
            if (newVideoId <= 0)
                return BadRequest();

            var country = await _repository.GetCountryById(newVideoId);
            var countryInfo = await _repository.GetCountryInfoByCountry(country);

            if (countryInfo == null)
                return BadRequest();

            var countryInfoVM = Mapper.Map<CountryInfo, CountryInfoVM>(countryInfo);       

            return new OkObjectResult(countryInfoVM);
        }

        [HttpGet]
        [Route("{newVideoId}/price-info", Name = "PriceInfo")]
        public async Task<IActionResult> GetPriceInfo(int newVideoId)
        {
            if (newVideoId <= 0)
                return BadRequest();

            var country = await _repository.GetCountryById(newVideoId);
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
        [Route("{newVideoId}/currency-info", Name = "CurrencyInfo")]
        public async Task<IActionResult> GetCurrencyInfo(int newVideoId)
        {
            if (newVideoId <= 0)
                return BadRequest();

            var country = await _repository.GetCountryById(newVideoId);
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
        [Route("{videoId}/youtube-likes", Name = "YouTubeLikes")]
        public async Task<IActionResult> GetYouTubeLikes(string videoId)
        {
            if (string.IsNullOrWhiteSpace(videoId))
                return BadRequest();

            var likes = await _youTube.GetYouTubeLikesByVideoId(videoId);

            if (likes == null)
                return BadRequest();

            return new OkObjectResult(likes);
        }  

        [HttpGet]
        [Route("{newVideoId}/{videoId}/video-comments-combined", Name = "GetVideoCommentsCombined")]
        public async Task<IActionResult> GetVideoCommentsCombined(int newVideoId, string videoId)
        {
            if (newVideoId <= 0 || string.IsNullOrWhiteSpace(videoId))
                return BadRequest();

            var videoComments = _vcRepository
                .FindBy(nv => nv.NewVideoId == newVideoId, vcr => vcr.VideoCommentReplies);           
            var youTubeComments = await _youTube.GetYouTubeCommentsByVideoId(videoId);

            if (videoComments == null)
                return NotFound();

            IEnumerable<VideoCommentVM> videoCommentVM =
                    Mapper.Map<IEnumerable<VideoComment>, IEnumerable<VideoCommentVM>>(videoComments);
            // Add youtube comments on top of comments from database
            if (videoComments != null)
            {
                foreach (var yc in youTubeComments.Comments)
                    videoCommentVM = videoCommentVM.Append(new VideoCommentVM
                    {
                        AuthorDisplayName = yc.AuthorDisplayName,
                        Comment = yc.TextDisplay,
                        DateCreated = yc.UpdatedAt.DateTime,
                        Likes = Convert.ToInt32(yc.Likes),
                        IsYouTubeComment = true
                    });
            }

            videoCommentVM = videoCommentVM.OrderByDescending(x => x.DateCreated);
            return new OkObjectResult(videoCommentVM);
        }

        [HttpPost]
        [Route("video-comment")]
        public async Task<IActionResult> AddVideoComment([FromBody] VideoCommentVM model)
        {
            if (!ModelState.IsValid)
            {
                string errorMsg = null;
                foreach (var m in ModelState.Values)
                    foreach (var msg in m.Errors)
                        errorMsg = msg.ErrorMessage;
                return BadRequest(errorMsg);
            }

            VideoComment newVideoComment = new VideoComment
            {
                Comment = model.Comment,
                DateCreated = DateTime.Now,
                NewVideoId = model.NewVideoId                
            };

            await _vcRepository.AddAsync(newVideoComment);
            await _vcRepository.CommitAsync();

            var videoComment = Mapper.Map<VideoComment, VideoCommentVM>(newVideoComment);

            return CreatedAtRoute("GetVideoComments", new { controller = "CardDetail", id = videoComment.VideoCommentId }, videoComment);
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
