using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ExodusKorea.API.Services;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.JsonModels;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class CardDetailController : Controller
    {
        private readonly ICardDetailRepository _repository;
        private readonly IVideoCommentRepository _vcRepository;
        private readonly IVideoCommentReplyRepository _vcrRepository;
        private readonly ICurrencyRatesService _currencyRate;
        private readonly IYouTubeService _youTube;
        private readonly UserManager<ApplicationUser> _userManager;

        public CardDetailController(ICardDetailRepository repository,
                                    IVideoCommentRepository vcRepository,
                                    IVideoCommentReplyRepository vcrRepository,
                                    ICurrencyRatesService currencyRate,
                                    IYouTubeService youTube,
                                    UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _vcRepository = vcRepository;
            _vcrRepository = vcrRepository;
            _currencyRate = currencyRate;
            _youTube = youTube;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("{newVideoId}/country-info", Name = "CountryInfo")]
        public async Task<IActionResult> GetCountryInfo(int newVideoId)
        {
            if (newVideoId <= 0)
                return NotFound();

            var country = await _repository.GetCountryById(newVideoId);
            var countryInfo = await _repository.GetCountryInfoByCountry(country);

            if (countryInfo == null)
                return NotFound();

            var countryInfoVM = Mapper.Map<CountryInfo, CountryInfoVM>(countryInfo);       

            return new OkObjectResult(countryInfoVM);
        }

        [HttpGet]
        [Route("{newVideoId}/price-info", Name = "PriceInfo")]
        public async Task<IActionResult> GetPriceInfo(int newVideoId)
        {
            if (newVideoId <= 0)
                return NotFound();

            var country = await _repository.GetCountryById(newVideoId);
            var kPriceInfo = await _repository.GetPriceInfoByCountry("대한민국");            
            var priceInfo = await _repository.GetPriceInfoByCountry(country);

            if (kPriceInfo == null || priceInfo == null)
                return NotFound();

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
                return NotFound();

            var country = await _repository.GetCountryById(newVideoId);
            var baseCurrency = GetBaseCurrency(country);
            var krwRate = await _currencyRate.GetKRWRateByCountry(baseCurrency);
          
            if (string.IsNullOrEmpty(krwRate))
                return NotFound();

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
                return NotFound();

            var likes = await _youTube.GetYouTubeLikesByVideoId(videoId);

            if (likes == null)
                return NotFound();

            return new OkObjectResult(likes);
        }  

        [HttpGet]
        [Route("{newVideoId}/{videoId}/video-comments-combined", Name = "GetVideoCommentsCombined")]
        public async Task<IActionResult> GetVideoCommentsCombined(int newVideoId, string videoId)
        {
            if (newVideoId <= 0 || string.IsNullOrWhiteSpace(videoId))
                return NotFound();

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

        [HttpGet("{id}", Name = "GetVideoComment")]
        public IActionResult GetVideoComment(int? id)
        {
            if (id == null)
                return NotFound();

            var videoComment = _vcRepository.GetSingle(c => c.VideoCommentId == id);

            if (videoComment != null)
            {
                var videoCommentVM = Mapper.Map<VideoComment, VideoCommentVM>(videoComment);

                return new OkObjectResult(videoComment);
            }
            else
                return NotFound();
        }

        [HttpGet("{id}", Name = "GetVideoCommentReply")]
        public IActionResult GetVideoCommentReply(int? id)
        {
            if (id == null)
                return NotFound();

            var videoCommentReply = _vcrRepository.GetSingle(c => c.VideoCommentReplyId == id);

            if (videoCommentReply != null)
            {
                var videoCommentReplyVM = Mapper.Map<VideoCommentReply, VideoCommentReplyVM>(videoCommentReply);

                return new OkObjectResult(videoCommentReplyVM);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("add-comment")]
        [Authorize]
        public async Task<IActionResult> AddVideoComment([FromBody] VideoCommentVM vm)
        {
            if (!ModelState.IsValid)
            {
                string errorMsg = null;
                foreach (var m in ModelState.Values)
                    foreach (var msg in m.Errors)
                        errorMsg = msg.ErrorMessage;
                return BadRequest(errorMsg);
            }

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            VideoComment newVideoComment = new VideoComment
            {
                Comment = vm.Comment,
                DateCreated = DateTime.Now,
                NewVideoId = vm.NewVideoId,
                AuthorDisplayName = user.NickName.Trim()
            };

            await _vcRepository.AddAsync(newVideoComment);
            await _vcRepository.CommitAsync();

            var videoCommentVM = Mapper.Map<VideoComment, VideoCommentVM>(newVideoComment);

            return CreatedAtRoute("GetVideoComment", 
                new { controller = "CardDetail", id = videoCommentVM.VideoCommentId }, videoCommentVM);
        }

        [HttpPost]
        [Route("add-comment-reply")]
        [Authorize]
        public async Task<IActionResult> AddVideoCommentReply([FromBody] VideoCommentReplyVM vm)
        {
            if (!ModelState.IsValid)
            {
                string errorMsg = null;
                foreach (var m in ModelState.Values)
                    foreach (var msg in m.Errors)
                        errorMsg = msg.ErrorMessage;
                return BadRequest(errorMsg);
            }

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            VideoCommentReply newVideoCommentReply = new VideoCommentReply
            {
                Comment = vm.Comment,
                DateCreated = DateTime.Now,
                VideoCommentId = vm.VideoCommentId,
                AuthorDisplayName = user.NickName.Trim()
            };

            await _vcrRepository.AddAsync(newVideoCommentReply);
            await _vcrRepository.CommitAsync();

            var videoCommentReplyVM = Mapper.Map<VideoCommentReply, VideoCommentReplyVM>(newVideoCommentReply);

            return CreatedAtRoute("GetVideoCommentReply", 
                new { controller = "CardDetail", id = videoCommentReplyVM.VideoCommentReplyId }, videoCommentReplyVM);
        }

        [HttpPut]
        [Route("{id}/update-comment-likes")]
        [Authorize]
        public async Task<IActionResult> UpdateCommentLikes(int id)
        {
            if (id <= 0)
                return NotFound();

            var videoComment = _vcRepository.GetSingle(vc => vc.VideoCommentId == id);

            if (videoComment == null)
                return NotFound();
            else
                videoComment.Likes += 1;           

            _vcRepository.Update(videoComment);
            await _vcRepository.CommitAsync();

            return new NoContentResult();
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
