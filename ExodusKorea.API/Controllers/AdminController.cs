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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _repository;
        private readonly IVideoPostRepository _vpRepository;
        private readonly ISalaryInfoRepository _siRepository;
        private readonly IUploaderRepository _uRepository;
        private readonly IYouTubeService _youTube;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IAdminRepository repository,
                               IVideoPostRepository vpRepository,
                               ISalaryInfoRepository siRepository,
                               IUploaderRepository uRepository,
                               IYouTubeService youTube,
                               UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _vpRepository = vpRepository;
            _siRepository = siRepository;
            _uRepository = uRepository;
            _youTube = youTube;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("categories-countries-uploaders", Name = "GetCategoryCountryUploader")]
        public async Task<IActionResult> GetCategoryCountryUploader()
        {
            var categories = await _repository.GetCategories();
            var countries = await _repository.GetCountries();
            var uploaders = await _repository.GetUploaders();

            if (categories == null || countries == null || uploaders == null)
                return NotFound();

            var categoryCountryVM = new CategoryCountryUploaderVM
            {
                Categories = categories,
                Countries = countries,
                Uploaders = uploaders
            };

            return new OkObjectResult(categoryCountryVM);
        }

        [HttpGet("{country}/salary-info-occupations", Name = "GetSalaryInfoOccupations")]
        public async Task<IActionResult> GetSalaryInfoOccupations(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                return NotFound();

            var salaryInfoOccupations = await _repository.GetSalaryInfoOccupations(country.Trim());

            if (salaryInfoOccupations != null)
            {
                var salaryInfoVM = Mapper.Map<IEnumerable<SalaryInfo>, IEnumerable<SalaryInfoVM>>(salaryInfoOccupations);
                salaryInfoVM = salaryInfoVM.OrderBy(x => x.Occupation);

                return new OkObjectResult(salaryInfoVM);
            }
            else
                return NotFound();
        }

        [HttpGet("{id}/video-post", Name = "GetVideoPost")]
        public IActionResult GetVideoPost(int? id)
        {
            if (id == null)
                return NotFound();

            var videoPost = _vpRepository.GetSingle(c => c.VideoPostId == id);

            if (videoPost != null)
            {
                var videoPostVM = Mapper.Map<VideoPost, VideoPostVM>(videoPost);

                return new OkObjectResult(videoPostVM);
            }
            else
                return NotFound();
        }

        [HttpGet("{id}/salary-info", Name = "GetSalaryInfo")]
        public IActionResult GetSalaryInfo(int? id)
        {
            if (id == null)
                return NotFound();

            var salaryInfo = _siRepository.GetSingle(c => c.SalaryInfoId == id);

            if (salaryInfo != null)
            {
                var salaryInfoVM = Mapper.Map<SalaryInfo, SalaryInfoVM>(salaryInfo);

                return new OkObjectResult(salaryInfoVM);
            }
            else
                return NotFound();
        }

        [HttpGet("{id}/uploader", Name = "GetUploader")]
        public IActionResult GetUploader(int? id)
        {
            if (id == null)
                return NotFound();

            var uploader = _uRepository.GetSingle(c => c.UploaderId == id);

            if (uploader != null)
            {
                return new OkObjectResult(uploader);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("add-new-video")]
        public async Task<IActionResult> AddNewVideo([FromBody] NewVideoVM vm)
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

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                VideoPost newVideoPost = new VideoPost
                {
                    CareerId = 1,
                    CategoryId = vm.CategoryId,
                    CountryId = vm.CountryId,
                    UploaderId = vm.UploaderId,
                    SalaryInfoId = vm.SalaryInfoId,
                    Likes = vm.Likes,
                    Title = vm.Title,
                    UploadedDate = DateTime.UtcNow,
                    YouTubeVideoId = vm.YouTubeVideoId,
                    SharerId = vm.SharerId,                  
                    IsGoogleDriveVideo = vm.IsGoogleDriveVideo ? (byte)1 : (byte)0
                };

                await _vpRepository.AddAsync(newVideoPost);
                await _vpRepository.CommitAsync();

                var videoCommentVM = Mapper.Map<VideoPost, VideoPostVM>(newVideoPost);

                return CreatedAtRoute("GetVideoPost", new
                {
                    controller = "Admin",
                    id = videoCommentVM.VideoPostId
                }, videoCommentVM);
            }

            return StatusCode(401);
        }

        [HttpPost]
        [Route("add-new-uploader")]
        public async Task<IActionResult> AddNewUploader([FromBody] NewUploaderVM vm)
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

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var uploaders = _uRepository.GetAll();

                foreach (var u in uploaders)
                    if (u.Name.Trim().Equals(vm.Name.Trim()))
                        return BadRequest("duplicate");

                Uploader newUploader = new Uploader
                {                  
                    Name = vm.Name.Trim(),
                    YouTubeChannelThumbnailUrl = vm.ThumbnailUrl.Trim()
                };

                await _uRepository.AddAsync(newUploader);
                await _uRepository.CommitAsync();

                return CreatedAtRoute("GetUploader", new
                {
                    controller = "Admin",
                    id = newUploader.UploaderId
                }, newUploader);
            }

            return StatusCode(401);
        }

        [HttpPost]
        [Route("add-new-salary-info")]
        public async Task<IActionResult> AddNewSalaryInfo([FromBody] NewSalaryInfoVM vm)
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

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                SalaryInfo newSalaryInfo = new SalaryInfo
                {
                  Country = vm.Country,
                  Currency = vm.Currency,
                  High = vm.High,
                  Low = vm.Low,
                  Median = vm.Median,
                  Occupation = vm.Occupation,
                  IsDisplayable = true
                };

                await _siRepository.AddAsync(newSalaryInfo);
                await _siRepository.CommitAsync();

                var salaryInfoVM = Mapper.Map<SalaryInfo, SalaryInfoVM>(newSalaryInfo);

                return CreatedAtRoute("GetSalaryInfo", new
                {
                    controller = "Admin",
                    id = salaryInfoVM.SalaryInfoId
                }, salaryInfoVM);
            }

            return StatusCode(401);
        }

        [HttpGet]
        [Route("{youTubeId}/youtube-info-by-id", Name = "GetYouTubeInfo")]
        public async Task<IActionResult> GetYouTubeInfo(string youTubeId)
        {
            if (string.IsNullOrWhiteSpace(youTubeId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var youTubeInfoVM = await _youTube.GetYouTubeInfoByVideoId(youTubeId);

                if (youTubeInfoVM == null)
                    return NotFound();            

                var videoPostInfoVM = new VideoPostInfoVM
                {
                    Likes = (long)Math.Ceiling((double)youTubeInfoVM.Likes / 10),    
                    Title = youTubeInfoVM.Title,
                    Owner = youTubeInfoVM.Owner,
                    ChannelId = youTubeInfoVM.ChannelId
                };

                return new OkObjectResult(videoPostInfoVM);
            }

            return StatusCode(401);
        }

        [HttpGet]
        [Route("{channelId}/channel-info-by-id", Name = "GetChannelInfo")]
        public async Task<IActionResult> GetChannelInfo(string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var youTubeChannelInfoVM = await _youTube.GetYouTubeChannelInfoByChannelId(channelId);

                if (youTubeChannelInfoVM == null)
                    return NotFound();           

                return new OkObjectResult(youTubeChannelInfoVM);
            }

            return StatusCode(401);
        }

        [HttpDelete("{videoPostId}/disable-video")]
        public async Task<IActionResult> DisableVideoComment(int videoPostId)
        {
            if (videoPostId <= 0)
                return NotFound();

            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var videoPost = _vpRepository.GetSingle(vp => vp.VideoPostId == videoPostId);

                if (videoPost == null)
                    return NotFound();

                videoPost.UploaderId = null;
                videoPost.JobsInDemandId = null;
                videoPost.IsDisabled = true;

                _vpRepository.Update(videoPost);
                await _vpRepository.CommitAsync();

                return new NoContentResult();
            }
            else
                return BadRequest();
        }
    }
}
