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
        private readonly IMinimumCostOfLivingRepository _mcolRepository;
        private readonly IMyVideosRepository _mvRepository;
        private readonly ICurrencyRatesService _currencyRate;
        private readonly IYouTubeService _youTube;
        private readonly IClientIPService _clientIP;
        private readonly IMessageService _email;
        private readonly UserManager<ApplicationUser> _userManager;

        public CardDetailController(ICardDetailRepository repository,
                                    IVideoCommentRepository vcRepository,
                                    IVideoCommentReplyRepository vcrRepository,
                                    IVideoPostLikeRepository vplRepository,
                                    IVideoCommentLikeRepository vclRepository,
                                    IVideoCommentReplyLikeRepository vcrlRepository,                                 
                                    IVideoPostRepository vpRepository,
                                    INotificationRepository nRepository,
                                    IMinimumCostOfLivingRepository mcolRepository,
                                    IMyVideosRepository mvRepository,
                                    ICurrencyRatesService currencyRate,
                                    IYouTubeService youTube,
                                    IClientIPService clientIP,
                                    IMessageService email,
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
            _mcolRepository = mcolRepository;
            _mvRepository = mvRepository;
            _currencyRate = currencyRate;
            _youTube = youTube;
            _clientIP = clientIP;
            _email = email;
            _userManager = userManager;
        }

        #region Country Info
        [HttpGet]
        [Route("{videoPostId}/country-info", Name = "CountryInfo")]
        public async Task<IActionResult> GetCountryInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var countryId = await _repository.GetCountryIdByVideoPostId(videoPostId);
            var countryInfo = await _repository.GetCountryInfoByCountryId(countryId);

            if (countryInfo == null)
                return NotFound();

            var countryInfoVM = Mapper.Map<CountryInfo, CountryInfoVM>(countryInfo);
          
            return new OkObjectResult(countryInfoVM);
        }
        #endregion

        #region Salary Info
        [HttpGet]
        [Route("{videoPostId}/salary-info", Name = "SalaryInfo")]
        public IActionResult GetSalaryInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var videoPost = _vpRepository.GetSingle(vp => vp.VideoPostId == videoPostId, vp => vp.SalaryInfo);

            if (videoPost == null)
                return NotFound();

            var salaryInfoVM = Mapper.Map<SalaryInfo, SalaryInfoVM>(videoPost.SalaryInfo);

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

            var videoPost = await _repository.GetVideoPostByVideoPostId(videoPostId);
            var kPriceInfo = await _repository.GetPriceInfoByCountry("kr");
            var priceInfo = await _repository.GetPriceInfoByCountry(videoPost.Country.NameEN);

            if (kPriceInfo == null || priceInfo == null)
                return NotFound();

            var priceInfoVM = new PriceInfoVM
            {
                CountryKR = videoPost.Country.NameKR,
                CountryEN = videoPost.Country.NameEN,
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
                    Currency = await _repository.GetBaseCurrencyByCountry(country),
                    Rent = Mapper.Map<PI_Rent, PI_RentVM>(rent),
                    Restaurant = Mapper.Map<PI_Restaurant, PI_RestaurantVM>(restaurant),
                    Groceries = Mapper.Map<PI_Groceries, PI_GroceriesVM>(groceries),
                    Etc = Mapper.Map<PI_Etc, PI_EtcVM>(etc)
                });
            }      

            return new OkObjectResult(priceInfoDetailVM);
        }
        #endregion

        #region Minimum Cost of Living Info
        [HttpGet]
        [Route("{videoPostId}/minimum-col-info", Name = "MinimumCoLInfo")]
        public async Task<IActionResult> MinimumCoLInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var video = _vpRepository.GetSingle(vp => vp.VideoPostId == videoPostId, vp => vp.Country);

            if (video == null)
                return NotFound();

            var countryInfo = await _repository.GetCountryInfoByCountry(video.Country.NameEN);
            var minimumCoLs = _mcolRepository.FindBy(mcol => mcol.CountryInfoId == countryInfo.CountryInfoId);
            var cities = await _repository.GetAllCitiesByCountryId(countryInfo.CountryId);
            var minimumCostOfLivingInfoVM = new MinimumCostOfLivingInfoVM
            {
                CountryId = video.CountryId,
                CountryKR = video.Country.NameKR,
                CountryEN = video.Country.NameEN,
                BaseCurrency = await _repository.GetBaseCurrencyByCountry(video.Country.NameEN),
                CityMinimums = CalculateCityMinimums(cities.ToList(), minimumCoLs.ToList())
            };

            return new OkObjectResult(minimumCostOfLivingInfoVM);
        }

        [HttpGet]
        [Route("{country}/minimum-col-detail", Name = "GetMinimumCostOfLivingDetail")]
        public IActionResult GetMinimumCostOfLivingDetail(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                return NotFound();

            var minimumCols = _mcolRepository.FindBy(mcol => mcol.Country.Equals(country));          
            var minimumColsVM = Mapper.Map<IEnumerable<MinimumCostOfLiving>, IEnumerable<MinimumCostOfLivingVM>>(minimumCols);

            minimumColsVM = minimumColsVM.OrderByDescending(x => x.DateCreated);

            return new OkObjectResult(minimumColsVM);
        }

        [HttpGet]
        [Route("{countryId}/cities-by-country", Name = "GetAllCitiesByCountryId")]
        public async Task<IActionResult> GetAllCitiesByCountryId(int countryId)
        {
            if (countryId <= 0)
                return NotFound();

            var cities = await _repository.GetAllCitiesByCountryId(countryId);

            if (cities == null)
                return NotFound();

            return new OkObjectResult(cities);
        }

        [HttpGet("{id}/minimum-col", Name = "GetMinimumCostOfLiving")]
        public IActionResult GetMinimumCostOfLiving(int? id)
        {
            if (id == null)
                return NotFound();

            var minimumCoL = _mcolRepository.GetSingle(mcol => mcol.MinimumCostOfLivingId == id);

            if (minimumCoL != null)
            {
                var minimumCoLVM = Mapper.Map<MinimumCostOfLiving, MinimumCostOfLivingVM>(minimumCoL);

                return new OkObjectResult(minimumCoL);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("add-minimum-col")]
        [Authorize]
        public async Task<IActionResult> AddMinimumCoL([FromBody] MinimumCostOfLivingVM vm)
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

            var ipAddress = _clientIP.GetClientIP();
            //var countryCode = await _clientIP.GetCountryCodeByIP(ipAddress);

            MinimumCostOfLiving newMinimumCostOfLiving = new MinimumCostOfLiving
            {
                CountryInfoId = await _repository.GetCountryInfoIdByCountry(vm.Country),
                CityId = vm.CityId > 0 ? vm.CityId : 0,
                Country = vm.Country,
                City = vm.CityId == 0 ? vm.City : await _repository.GetCityById(vm.CityId),
                Rent = vm.Rent,
                Transportation = vm.Transportation,
                Food = vm.Food,
                Cell = vm.Cell,
                Internet = vm.Internet,
                Etc = vm.Etc,
                IpAddress = ipAddress,
                NickName = user.NickName,
                Total = vm.Rent + vm.Transportation + vm.Food + vm.Cell + vm.Internet,
                DateCreated = DateTime.Now,
                //AuthorCountryEN = countryCode
            };

            await _mcolRepository.AddAsync(newMinimumCostOfLiving);
            await _mcolRepository.CommitAsync();

            var minimumCostOfLivingVM = Mapper.Map<MinimumCostOfLiving, MinimumCostOfLivingVM>(newMinimumCostOfLiving);

            return CreatedAtRoute("GetMinimumCostOfLiving", new
            {
                controller = "CardDetail",
                id = newMinimumCostOfLiving.MinimumCostOfLivingId
            }, null);
        }
        #endregion

        #region Career Info
        [HttpGet]
        [Route("{videoPostId}/career-info", Name = "CareerInfo")]
        public async Task<IActionResult> GetCareerInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var career = await _repository.GetCareerByVideoPostId(videoPostId);
            
            if (career == null)
                return NotFound();

            var careerInfoVM = Mapper.Map<Career, CareerInfoVM>(career);

            return new OkObjectResult(careerInfoVM);
        }
        #endregion

        #region Job Sites
        [HttpGet]     
        [Route("{videoPostId}/job-sites", Name = "JobSites")]
        public async Task<IActionResult> GetJobSites(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var videoPost = _vpRepository.GetSingle(vp => vp.VideoPostId == videoPostId);

            if (videoPost == null)
                return NotFound();

            var jobSites = await _repository.GetJobSitesByCountryId(videoPost.CountryId);

            if (jobSites == null)
                return NotFound();

            var jobSitesVM = Mapper.Map<IEnumerable<JobSite>, IEnumerable<JobSiteVM>>(jobSites);

            return new OkObjectResult(jobSitesVM);
        }
        #endregion       

        #region Currency Info
        [HttpGet]
        [Route("{videoPostId}/currency-info", Name = "CurrencyInfo")]
        public async Task<IActionResult> GetCurrencyInfo(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var country = await _repository.GetCountryByVideoPostId(videoPostId);
            var krwRate = await _currencyRate.GetKRWRateByCountry(country.BaseCurrency);

            if (string.IsNullOrEmpty(krwRate))
                return NotFound();

            return new OkObjectResult(new CurrencyInfoVM
            {
                Country = country.NameKR,
                BaseCurrency = country.BaseCurrency,
                KrwRate = krwRate,
                Now = DateTime.Now
            });
        }
        #endregion       

        #region Video Comments
        [HttpGet]
        [Route("{videoPostId}/{videoId}/{isGoogleDriveVideo}/video-comments-combined", Name = "GetVideoCommentsCombined")]
        public async Task<IActionResult> GetVideoCommentsCombined(int videoPostId, string videoId, byte isGoogleDriveVideo)
        {
            if (videoPostId <= 0 || string.IsNullOrWhiteSpace(videoId))
                return NotFound();

            var videoComments = _vcRepository
                .FindBy(nv => nv.VideoPostId == videoPostId, vcr => vcr.VideoCommentReplies);
            var videoCommentVM = Mapper.Map<IEnumerable<VideoComment>, IEnumerable<VideoCommentVM>>(videoComments);

            if (isGoogleDriveVideo == 1)
            {
                videoCommentVM = videoCommentVM.OrderByDescending(x => x.DateCreated);
                return new OkObjectResult(videoCommentVM);
            }                

            var youTubeComments = await _youTube.GetYouTubeCommentsByVideoId(videoId);   
            // Add youtube comments on top of comments from database   
            if (youTubeComments.Comments.Count > 0)
                foreach (var yc in youTubeComments.Comments)
                    videoCommentVM = videoCommentVM.Append(new VideoCommentVM
                    {
                        AuthorDisplayName = yc.AuthorDisplayName,
                        Comment = yc.TextDisplay,
                        DateCreated = yc.UpdatedAt.DateTime,
                        Likes = Convert.ToInt32(yc.Likes),
                        IsYouTubeComment = true,
                        TotalReplyCount = yc.TotalReplyCount,
                        YouTubeCommentReplies = MappReplies(yc.Replies)
                        //ParentId = yc.ParentId
                    });         

            videoCommentVM = videoCommentVM.OrderByDescending(x => x.DateCreated);

            return new OkObjectResult(videoCommentVM);
        }

        //[HttpGet("{parentId}/youtube-replies", Name = "GetYouTubeReplies")]
        //public async Task<IActionResult> GetYouTubeReplies(string parentId)
        //{
        //    if (string.IsNullOrWhiteSpace(parentId))
        //        return NotFound();

        //    var youTubeReplies = await _youTube.GetYouTubeRepliesByParentId(parentId);
        //    var videoCommentVM = new List<VideoCommentVM>();

        //    if (youTubeReplies.Comments.Count > 0)
        //    {
        //        youTubeReplies.Comments.OrderBy(x => x.UpdatedAt);

        //        foreach (var yc in youTubeReplies.Comments)
        //            videoCommentVM.Add(new VideoCommentVM
        //            {
        //                AuthorDisplayName = yc.AuthorDisplayName,
        //                Comment = yc.TextDisplay,
        //                DateCreated = yc.UpdatedAt.DateTime,
        //                Likes = Convert.ToInt32(yc.Likes),
        //                IsYouTubeComment = true
        //            });

        //        return new OkObjectResult(videoCommentVM);
        //    }               
        //    else
        //        return new NoContentResult();
        //}

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
         
            var ipAddress = _clientIP.GetClientIP();          
            //var countryCode = await _clientIP.GetCountryCodeByIP(ipAddress);

            VideoComment newVideoComment = new VideoComment
            {
                Comment = vm.Comment,
                DateCreated = DateTime.Now,
                VideoPostId = vm.VideoPostId,
                AuthorDisplayName = user.NickName.Trim(),
                UserId = user.Id,
                IPAddress = ipAddress,
                //Country = countryCode,
                IsSharer = await CheckSharer(user, vm.VideoPostId)
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

            var ipAddress = _clientIP.GetClientIP();
            //var countryCode = await _clientIP.GetCountryCodeByIP(ipAddress);

            VideoCommentReply newVideoCommentReply = new VideoCommentReply
            {
                Comment = vm.Comment,
                DateCreated = DateTime.Now,
                VideoCommentId = vm.VideoCommentId,
                AuthorDisplayName = user.NickName.Trim(),
                UserId = user.Id,
                IPAddress = ipAddress,
                //Country = countryCode,
                IsSharer = await CheckSharer(user, vm.VideoPostId)
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

            var ipAddress = _clientIP.GetClientIP();
            //var countryCode = await _clientIP.GetCountryCodeByIP(ipAddress);

            VideoCommentReply newVideoCommentReply = new VideoCommentReply
            {
                Comment = vm.Comment,
                DateCreated = DateTime.Now,
                VideoCommentId = vm.VideoCommentId,
                AuthorDisplayName = user.NickName.Trim(),
                UserId = user.Id,
                RepliedTo = vm.AuthorDisplayName,
                IPAddress = ipAddress,
                //Country = countryCode,
                IsSharer = await CheckSharer(user, vm.VideoPostId)
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

        #region Video Post Owner, Title, Likes
        [HttpGet]
        [Route("{videoPostId}/{videoId}/{isGoogleDriveVideo}/video-post-info", Name = "GetVideoPostInfo")]
        public async Task<IActionResult> GetVideoPostInfo(int videoPostId, string videoId, byte isGoogleDriveVideo)
        {
            if (videoPostId <= 0 || string.IsNullOrWhiteSpace(videoId))
                return NotFound();

            var videoPost = _vpRepository.GetSingle(vp => vp.VideoPostId == videoPostId, vp => vp.Uploader);

            if (videoPost == null)
                return NotFound();

            VideoPostInfoVM videoPostInfoVM = null;

            if (isGoogleDriveVideo == 1)
            {
                videoPostInfoVM = new VideoPostInfoVM
                {
                    Likes = videoPost.Likes,
                    Owner = videoPost.Uploader.Name,
                    Title = videoPost.Title
                };

                return new OkObjectResult(videoPostInfoVM);
            }              

            var youTubeInfoVM = await _youTube.GetYouTubeInfoByVideoId(videoId);

            if (youTubeInfoVM == null)
            {
                string subject = "[엑소더스 코리아] 404 유튜브 오류";
                string body = "VideoPostId: " + videoPost.VideoPostId + "\r\n\r\n"
                               + "YouTubeVideoId: " + videoPost.YouTubeVideoId + "\r\n\r\n";

                await _email.SendEmailAsync("admin@exoduscorea.com", "admin@exoduscorea.com", subject, body, null);

                return NotFound("youtube");
            }             

            videoPostInfoVM = new VideoPostInfoVM
            {
                //Likes = videoPost.Likes + youTubeInfoVM.Likes,
                Likes = videoPost.Likes,
                Owner = youTubeInfoVM.Owner,
                Title = youTubeInfoVM.Title
            };

            return new OkObjectResult(videoPostInfoVM);
        }

        [HttpGet("{id}/video-post-like", Name = "FindUserLikedPost")]
        [Authorize]
        public async Task<IActionResult> FindUserLikedPost(int id)
        {
            if (id <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var videoPostLikes = _vplRepository
                .GetSingle(vpl => vpl.UserId.Equals(user.Id) && vpl.VideoPostId == id);

            if (videoPostLikes == null)
                return new NoContentResult();

            return new OkResult();        
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

            var commentLikes = _vclRepository
                .GetSingle(vcl => vcl.UserId.Equals(user.Id) && vcl.VideoCommentId == id);

            if (commentLikes == null)
                return new NoContentResult();

            return new OkResult();
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

            var commentReplyLikes = _vcrlRepository
                .GetSingle(vcrl => vcrl.UserId.Equals(user.Id) && vcrl.VideoCommentReplyId == id);

            if (commentReplyLikes == null)
                return new NoContentResult();

            return new OkResult();      
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
        [HttpGet]
        [Route("notifications-for-user", Name = "GetNotificationsForUser")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsForUser()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var notifications = _nRepository
                .FindBy(nv => nv.UserId.Equals(user.Id) && nv.DateCreated >= DateTime.Now.Date.AddDays(-14));
            var notificationsVM = Mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationVM>>(notifications);

            notificationsVM = notificationsVM.OrderByDescending(x => x.DateCreated);

            return new OkObjectResult(notificationsVM);
        }

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
               IsGoogleDriveVideo = vm.IsGoogleDriveVideo,
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

        [HttpPut]
        [Route("{id}/update-has-read")]
        [Authorize]
        public async Task<IActionResult> UpdateHasRead(long id)
        {
            if (id <= 0)
                return NotFound();

            var notification = _nRepository.GetSingle(n => n.NotificationId == id);

            if (notification == null)
                return NotFound();
            else
                notification.HasRead = true;

            _nRepository.Update(notification);
            await _nRepository.CommitAsync();

            return new NoContentResult();
        }
        #endregion

        #region Follow
        [HttpGet("{id}/my-videos", Name = "GetMyVideoById")]
        public IActionResult GetMyVideoById(int? id)
        {
            if (id == null)
                return NotFound();

            var myVideos = _mvRepository.GetSingle(c => c.MyVideosId == id);

            if (myVideos != null)
                return new OkObjectResult(myVideos);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("add-my-videos")]
        [Authorize]
        public async Task<IActionResult> AddToMyVideos([FromBody] AddToMyVideosVM vm)
        {
            if (vm.VideoPostId <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var duplicate = _mvRepository
                .GetSingle(mv => mv.VideoPostId == vm.VideoPostId && mv.ApplicationUserId.Equals(user.Id));

            if (duplicate != null)
                return BadRequest("duplicate");

            MyVideos myVideos = new MyVideos
            {
                ApplicationUserId = user.Id,
                VideoPostId = vm.VideoPostId            
            };

            await _mvRepository.AddAsync(myVideos);
            await _mvRepository.CommitAsync();

            return CreatedAtRoute("GetMyVideoById", new
            {
                controller = "CardDetail",
                id = myVideos.MyVideosId
            }, myVideos);
        }
        
        [HttpGet]
        [Authorize]
        [Route("my-videos", Name = "GetMyVideos")]
        public async Task<IActionResult> GetMyVideos()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var myVideos = _mvRepository.FindBy(mv => mv.ApplicationUserId == user.Id);
            var allVideos = _vpRepository.FindBy(vp => !vp.IsDisabled, vp => vp.Country, vp => vp.Uploader);

            if (myVideos == null || allVideos == null)
                return NotFound();

            var myVideoPosts = new List<VideoPost>();

            foreach (var av in allVideos)
                if (myVideos.Any(mv => mv.VideoPostId == av.VideoPostId))
                    myVideoPosts.Add(av);

            var allMyVideosVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(myVideoPosts);

            return new OkObjectResult(allMyVideosVM);
        }
        #endregion

        #region Private Functions
        private IEnumerable<YouTubeCommentReplyVM> MappReplies(List<Reply> replies)
        {
            var ycr = new List<YouTubeCommentReplyVM>();

            if (replies.Count > 0)
                foreach (var r in replies)
                    ycr.Add(new YouTubeCommentReplyVM
                    {
                        AuthorDisplayName = r.AuthorDisplayName,
                        Comment = r.TextDisplay,
                        Likes = Convert.ToInt32(r.Likes),
                        DateCreated = r.UpdatedAt.DateTime
                    });

            return ycr.AsEnumerable().OrderBy(x => x.DateCreated);
        }

        private async Task<bool> CheckSharer(ApplicationUser user, int videoPostId)
        {            
            var sharerId = _vpRepository.GetSingle(vp => vp.VideoPostId == videoPostId).SharerId;

            if (user.Id.Equals(sharerId))
            {
                var userRole = await _userManager.GetRolesAsync(user);
                return userRole.SingleOrDefault().ToLower().Equals("sharer") ? true : false;
            }

            return false;
        }

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

        private List<CityMinimumVM> CalculateCityMinimums(List<City> cities, List<MinimumCostOfLiving> mcol)
        {
            var random = new Random();
            var randomCities = cities.OrderBy(x => random.Next()).Take(2).ToList();
            var cityMinimumVM = new List<CityMinimumVM>();

            foreach (var c in randomCities)
            {
                var col = mcol.Where(x => x.CityId == c.CityId).ToList();
                var count = col.Count;

                cityMinimumVM.Add(new CityMinimumVM
                {
                    City = c.Name,
                    AvgCostOfLiving = count == 0 ? 0 : col.Sum(x => x.Total) / count
                });
            }

            return cityMinimumVM;
        }
        #endregion
    }
}
