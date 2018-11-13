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
        private readonly IVideoPostLikeRepository _vplRepository;
        private readonly IVideoCommentLikeRepository _vclRepository;
        private readonly IVideoCommentReplyLikeRepository _vcrlRepository;
        private readonly IVideoPostRepository _vpRepository;
        private readonly INotificationRepository _nRepository;
        private readonly ICurrencyRatesService _currencyRate;
        private readonly IYouTubeService _youTube;
        private readonly UserManager<ApplicationUser> _userManager;

        public CardDetailController(ICardDetailRepository repository,
                                    IVideoCommentRepository vcRepository,
                                    IVideoCommentReplyRepository vcrRepository,
                                    IVideoPostLikeRepository vplRepository,
                                    IVideoCommentLikeRepository vclRepository,
                                    IVideoCommentReplyLikeRepository vcrlRepository,                                 
                                    IVideoPostRepository vpRepository,
                                    INotificationRepository nRepository,
                                    ICurrencyRatesService currencyRate,
                                    IYouTubeService youTube,
                                    UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _vcRepository = vcRepository;
            _vcrRepository = vcrRepository;
            _vplRepository = vplRepository;
            _vclRepository = vclRepository;
            _vcrlRepository = vcrlRepository;
            _vpRepository = vpRepository;
            _nRepository = nRepository;
            _currencyRate = currencyRate;
            _youTube = youTube;
            _userManager = userManager;
        }

        #region Country Info
        [HttpGet]
        [Route("{videoPostId}/country-info", Name = "CountryInfo")]
        public async Task<IActionResult> GetCountryInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var country = await _repository.GetCountryById(videoPostId);
            var countryInfo = await _repository.GetCountryInfoByCountry(country);

            if (countryInfo == null)
                return NotFound();

            var countryInfoVM = Mapper.Map<CountryInfo, CountryInfoVM>(countryInfo);

            return new OkObjectResult(countryInfoVM);
        }
        #endregion

        #region Salary Info
        [HttpGet]
        [Route("{videoPostId}/salary-info", Name = "SalaryInfo")]
        public async Task<IActionResult> GetSalaryInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();
         
            var salaryInfo = await _repository.GetSalaryInfoById(videoPostId);

            if (salaryInfo == null)
                return NotFound();

            var salaryInfoVM = Mapper.Map<SalaryInfo, SalaryInfoVM>(salaryInfo);

            return new OkObjectResult(salaryInfoVM);
        }
        #endregion

        #region Price Info
        [HttpGet]
        [Route("{videoPostId}/price-info", Name = "PriceInfo")]
        public async Task<IActionResult> GetPriceInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var country = await _repository.GetCountryById(videoPostId);
            var kPriceInfo = await _repository.GetPriceInfoByCountry("southkorea");
            var priceInfo = await _repository.GetPriceInfoByCountry(country);

            if (kPriceInfo == null || priceInfo == null)
                return NotFound();

            var priceInfoVM = new PriceInfoVM
            {
                Country = GetCountryInKorean(country),
                CountryInEng = country,
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
        [Route("{country}/price-info-detail", Name = "PriceInfoDetail")]
        public async Task<IActionResult> GetPriceInfoDetail(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                return NotFound();
            
            var cities = await _repository.GetMajorCitiesByCountry(country);
        
            if (cities == null)
                return NotFound();

            var priceInfoDetailVM = new List<PriceInfoDetailVM>();
            
            foreach (var c in cities)
            {
                var rent = await _repository.GetPI_RentByCity(c);
                var restaurant = await _repository.GetPI_RestaurantByCity(c);
                var groceries = await _repository.GetPI_GroceriesByCity(c);
                var etc = await _repository.GetPI_EtcByCity(c);

                priceInfoDetailVM.Add(new PriceInfoDetailVM
                {
                    City = c,
                    Currency = GetBaseCurrency(country),
                    Rent = Mapper.Map<PI_Rent, PI_RentVM>(rent),
                    Restaurant = Mapper.Map<PI_Restaurant, PI_RestaurantVM>(restaurant),
                    Groceries = Mapper.Map<PI_Groceries, PI_GroceriesVM>(groceries),
                    Etc = Mapper.Map<PI_Etc, PI_EtcVM>(etc)
                });
            }      

            return new OkObjectResult(priceInfoDetailVM);
        }
        #endregion

        #region Currency Info
        [HttpGet]
        [Route("{videoPostId}/currency-info", Name = "CurrencyInfo")]
        public async Task<IActionResult> GetCurrencyInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var country = await _repository.GetCountryById(videoPostId);
            var baseCurrency = GetBaseCurrency(country);
            var krwRate = await _currencyRate.GetKRWRateByCountry(baseCurrency);

            if (string.IsNullOrEmpty(krwRate))
                return NotFound();

            return new OkObjectResult(new CurrencyInfoVM
            {
                Country = GetCountryInKorean(country),
                BaseCurrency = baseCurrency,
                KrwRate = krwRate,
                Now = DateTime.Now
            });
        }
        #endregion       

        #region Video Comments
        [HttpGet]
        [Route("{videoPostId}/{videoId}/video-comments-combined", Name = "GetVideoCommentsCombined")]
        public async Task<IActionResult> GetVideoCommentsCombined(int videoPostId, string videoId)
        {
            if (videoPostId <= 0 || string.IsNullOrWhiteSpace(videoId))
                return NotFound();

            var videoComments = _vcRepository
                .FindBy(nv => nv.VideoPostId == videoPostId, vcr => vcr.VideoCommentReplies);
            var youTubeComments = await _youTube.GetYouTubeCommentsByVideoId(videoId);       
            var videoCommentVM = Mapper.Map<IEnumerable<VideoComment>, IEnumerable<VideoCommentVM>>(videoComments);
            // Add youtube comments on top of comments from database   
            if (youTubeComments.Comments.Count > 0)
                foreach (var yc in youTubeComments.Comments)
                    videoCommentVM = videoCommentVM.Append(new VideoCommentVM
                    {
                        AuthorDisplayName = yc.AuthorDisplayName,
                        Comment = yc.TextDisplay,
                        DateCreated = yc.UpdatedAt.DateTime,
                        Likes = Convert.ToInt32(yc.Likes),
                        IsYouTubeComment = true
                    });         

            videoCommentVM = videoCommentVM.OrderByDescending(x => x.DateCreated);

            return new OkObjectResult(videoCommentVM);
        }

        [HttpGet("{id}/video-comment", Name = "GetVideoComment")]
        public IActionResult GetVideoComment(long? id)
        {
            if (id == null)
                return NotFound();

            var videoComment = _vcRepository.GetSingle(c => c.VideoCommentId == id, vcr => vcr.VideoCommentReplies);

            if (videoComment != null)
            {
                var videoCommentVM = Mapper.Map<VideoComment, VideoCommentVM>(videoComment);

                return new OkObjectResult(videoCommentVM);
            }
            else
                return NotFound();
        }

        [HttpGet("{id}/video-comment-reply", Name = "GetVideoCommentReply")]
        public IActionResult GetVideoCommentReply(long? id)
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
                VideoPostId = vm.VideoPostId,
                AuthorDisplayName = user.NickName.Trim(),
                UserId = user.Id
            };

            await _vcRepository.AddAsync(newVideoComment);
            await _vcRepository.CommitAsync();

            var videoCommentVM = Mapper.Map<VideoComment, VideoCommentVM>(newVideoComment);

            return CreatedAtRoute("GetVideoComment", new
            {
                controller = "CardDetail",
                id = videoCommentVM.VideoCommentId
            }, videoCommentVM);
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
                AuthorDisplayName = user.NickName.Trim(),
                UserId = user.Id
            };

            await _vcrRepository.AddAsync(newVideoCommentReply);
            await _vcrRepository.CommitAsync();

            var videoCommentReplyVM = Mapper.Map<VideoCommentReply, VideoCommentReplyVM>(newVideoCommentReply);

            return CreatedAtRoute("GetVideoCommentReply", new
            {
                controller = "CardDetail",
                id = videoCommentReplyVM.VideoCommentReplyId
            }, videoCommentReplyVM);
        }

        [HttpPost]
        [Route("add-comment-reply-reply")]
        [Authorize]
        public async Task<IActionResult> AddVideoCommentReplyReply([FromBody] VideoCommentReplyVM vm)
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
                AuthorDisplayName = user.NickName.Trim(),
                UserId = user.Id,
                RepliedTo = vm.AuthorDisplayName
            };

            await _vcrRepository.AddAsync(newVideoCommentReply);
            await _vcrRepository.CommitAsync();

            var videoCommentReplyVM = Mapper.Map<VideoCommentReply, VideoCommentReplyVM>(newVideoCommentReply);

            return CreatedAtRoute("GetVideoCommentReply", new
            {
                controller = "CardDetail",
                id = videoCommentReplyVM.VideoCommentReplyId
            }, videoCommentReplyVM);
        }
        #endregion

        #region Video Post Likes
        [HttpGet]
        [Route("{videoPostId}/{videoId}/video-post-likes", Name = "GetVideoPostLikesCombined")]
        public async Task<IActionResult> GetVideoPostLikesCombined(int videoPostId, string videoId)
        {
            if (videoPostId <= 0 || string.IsNullOrWhiteSpace(videoId))
                return NotFound();

            var videoPost = _vpRepository.GetSingle(x => x.VideoPostId == videoPostId);

            if (videoPost == null)
                return NotFound();

            var youTubeLikes = await _youTube.GetYouTubeLikesByVideoId(videoId);

            if (youTubeLikes == null)
                return NotFound();

            var likesCombined = videoPost.Likes + Convert.ToInt32(youTubeLikes);

            return new OkObjectResult(likesCombined);
        }
        #endregion

        #region Video Post Likes
        [HttpGet("{id}/video-post-like", Name = "FindUserLikedPost")]
        [Authorize]
        public async Task<IActionResult> FindUserLikedPost(int id)
        {
            if (id <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var videoPostLikes = _vplRepository.FindBy(c => c.UserId.Equals(user.Id));
            var flag = false;

            if (videoPostLikes != null)
            {
                foreach (var vcl in videoPostLikes)
                    if (vcl.VideoPostId == id)
                    {
                        flag = true;
                        break;
                    }
            }               
            else
                return NotFound();

            if (flag)
                return new OkObjectResult(videoPostLikes);
            else
                return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update-video-post-likes")]
        [Authorize]
        public async Task<IActionResult> UpdateVideoPostLikes(int id)
        {
            if (id <= 0)
                return NotFound();

            var videoPost = _vpRepository.GetSingle(vc => vc.VideoPostId == id);

            if (videoPost == null)
                return NotFound();
            else
                videoPost.Likes += 1;

            _vpRepository.Update(videoPost);
            await _vpRepository.CommitAsync();

            return new NoContentResult();
        }

        [HttpPost]
        [Route("add-video-post-like")]
        [Authorize]
        public async Task<IActionResult> AddVideoPostLike([FromBody] VideoPostVM vm)
        {
            if (vm.VideoPostId <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            VideoPostLike videoPostLike = new VideoPostLike
            {
                UserId = user.Id,
                VideoPostId = vm.VideoPostId
            };

            await _vplRepository.AddAsync(videoPostLike);
            await _vplRepository.CommitAsync();

            return CreatedAtRoute("FindUserLikedPost", new
            {
                controller = "CardDetail",
                id = videoPostLike.VideoPostId
            }, null);
        }
        #endregion

        #region Video Comment Likes
        [HttpGet("{id}/comment-like", Name = "FindUserLikedComment")]
        [Authorize]
        public async Task<IActionResult> FindUserLikedComment(long id)
        {
            if (id <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var commentLikes = _vclRepository.FindBy(c => c.UserId.Equals(user.Id));
            var hasLiked = false;

            if (commentLikes != null)
            {
                foreach (var vcl in commentLikes)
                    if (vcl.VideoCommentId == id)
                    {
                        hasLiked = true;
                        break;
                    }
            }              
            else
                return NotFound();

            if (hasLiked)
                return new OkResult();
            else
                return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update-comment-likes")]
        [Authorize]
        public async Task<IActionResult> UpdateCommentLikes(long id)
        {
            if (id <= 0)
                return NotFound();

            var comment = _vcRepository.GetSingle(vc => vc.VideoCommentId == id);

            if (comment == null)
                return NotFound();
            else
                comment.Likes += 1;

            _vcRepository.Update(comment);
            await _vcRepository.CommitAsync();

            return new NoContentResult();
        }

        [HttpPost]
        [Route("add-comment-like")]
        [Authorize]
        public async Task<IActionResult> AddCommentLike([FromBody] VideoCommentVM vm)
        {
            if (vm.VideoCommentId <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            CommentLike commentLike = new CommentLike
            {
                UserId = user.Id,
                VideoCommentId = vm.VideoCommentId
            };

            await _vclRepository.AddAsync(commentLike);
            await _vclRepository.CommitAsync();

            return CreatedAtRoute("FindUserLikedComment", new
            {
                controller = "CardDetail",
                id = commentLike.VideoCommentId
            }, null);
        }
        #endregion

        #region Video Comment Reply Likes
        [HttpGet("{id}/comment-reply-like", Name = "FindUserLikedCommentReply")]
        [Authorize]
        public async Task<IActionResult> FindUserLikedCommentReply(long id)
        {          
            if (id <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var commentReplyLikes = _vcrlRepository.FindBy(c => c.UserId.Equals(user.Id));
            var hasLiked = false;

            if (commentReplyLikes != null)
            {
                foreach (var vcl in commentReplyLikes)
                    if (vcl.VideoCommentReplyId == id)
                    {
                        hasLiked = true;
                        break;
                    }
            }
            else
                return NotFound();

            if (hasLiked)
                return new OkResult();
            else
                return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/update-comment-reply-likes")]
        [Authorize]
        public async Task<IActionResult> UpdateCommentReplyLikes(long id)
        {
            if (id <= 0)
                return NotFound();

            var commentReply = _vcrRepository.GetSingle(vc => vc.VideoCommentReplyId == id);

            if (commentReply == null)
                return NotFound();
            else
                commentReply.Likes += 1;

            _vcrRepository.Update(commentReply);
            await _vcrRepository.CommitAsync();

            return new NoContentResult();
        }

        [HttpPost]
        [Route("add-comment-reply-like")]
        [Authorize]
        public async Task<IActionResult> AddCommentReplyLike([FromBody] VideoCommentReplyVM vm)
        {
            if (vm.VideoCommentReplyId <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            CommentReplyLike commentReplyLike = new CommentReplyLike
            {
                UserId = user.Id,
                VideoCommentReplyId = vm.VideoCommentReplyId
            };

            await _vcrlRepository.AddAsync(commentReplyLike);
            await _vcrlRepository.CommitAsync();

            return CreatedAtRoute("FindUserLikedCommentReply", new
            {
                controller = "CardDetail",
                id = commentReplyLike.VideoCommentReplyId
            }, null);
        }
        #endregion

        #region Video Comment Delete
        [HttpDelete("{id}/delete-video-comment")]
        [Authorize]
        public async Task<IActionResult> DeleteVideoComment(long id)
        {
            if (id <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var comment = _vcRepository.GetSingle(s => s.VideoCommentId == id);

            if (comment == null)
                return NotFound();

            if (comment.UserId.Equals(user.Id))
            {
                IEnumerable<VideoCommentReply> videoCommentReplies = _vcrRepository.FindBy(e => e.VideoCommentId == id);

                foreach (var r in videoCommentReplies)
                    _vcrRepository.Delete(r);

                _vcRepository.Delete(comment);

                await _vcRepository.CommitAsync();

                return new NoContentResult();
            }
            else
                return BadRequest();
        }
        #endregion

        #region Video Comment Reply Delete
        [HttpDelete("{id}/delete-video-comment-reply")]
        [Authorize]
        public async Task<IActionResult> DeleteVideoCommentReply(long id)
        {
            if (id <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var commentReply = _vcrRepository.GetSingle(s => s.VideoCommentReplyId == id);

            if (commentReply == null)
                return NotFound();

            if (commentReply.UserId.Equals(user.Id))
            {
                _vcrRepository.Delete(commentReply);

                await _vcrRepository.CommitAsync();

                return new NoContentResult();
            }
            else
                return BadRequest();
        }
        #endregion

        #region Notification
        [HttpGet("{id}/notification", Name = "GetNotification")]
        public IActionResult GetNotification(long? id)
        {
            if (id == null)
                return NotFound();

            var notification = _nRepository.GetSingle(c => c.NotificationId == id);

            if (notification != null)
            {
                var notificationVM = Mapper.Map<Notification, NotificationVM>(notification);

                return new OkObjectResult(notificationVM);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("add-notification")]
        [Authorize]
        public async Task<IActionResult> AddNotification([FromBody] NotificationVM vm)
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

            Notification newNotification = new Notification
            {
               VideoCommentId = vm.VideoCommentId,
               VideoCommentReplyId = vm.VideoCommentReplyId,
               VideoPostId = vm.VideoPostId,
               YouTubeVideoId = vm.YouTubeVideoId,
               UserId = vm.UserId,
               NickName = user.NickName,
               Comment = vm.Comment,
               DateCreated = DateTime.Now
            };

            await _nRepository.AddAsync(newNotification);
            await _nRepository.CommitAsync();

            var notificationVM = Mapper.Map<Notification, NotificationVM>(newNotification);

            return CreatedAtRoute("GetNotification", new
            {
                controller = "CardDetail",
                id = newNotification.NotificationId
            }, null);
        }
        #endregion

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

        private string GetCountryInKorean(string country)
        {
            var countryKor = "";

            switch (country)
            {
                case "canada":
                    countryKor = "캐나다";
                    break;
                case "unitedstates":
                    countryKor = "미국";
                    break;
            }

            return countryKor;
        }

        private string GetBaseCurrency(string country)
        {
            var baseCurrency = "";

            switch (country)
            {
                case "canada":
                    baseCurrency = "CAD";
                    break;
                case "unitedstates":
                    baseCurrency = "USD";
                    break;
            }

            return baseCurrency;
        }
        #endregion
    }
}
