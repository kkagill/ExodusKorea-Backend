using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IAdminRepository repository,
                               IVideoPostRepository vpRepository,
                               ISalaryInfoRepository siRepository,
                               UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _vpRepository = vpRepository;
            _siRepository = siRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("categories-countries", Name = "GetCategoryCountry")]
        public async Task<IActionResult> GetCategoryCountry()
        {
            var categories = await _repository.GetCategories();
            var countries = await _repository.GetCountries();

            if (categories == null || countries == null)
                return NotFound();

            var categoryCountryVM = new CategoryCountryVM
            {
                Categories = categories,
                Countries = countries
            };

            return new OkObjectResult(categoryCountryVM);
        }

        [HttpGet("{nameKR}/salary-info-occupations", Name = "GetSalaryInfoOccupations")]
        public async Task<IActionResult> GetSalaryInfoOccupations(string nameKR)
        {
            if (string.IsNullOrWhiteSpace(nameKR))
                return NotFound();

            var salaryInfoOccupations = await _repository.GetSalaryInfoOccupations(nameKR.Trim());

            if (salaryInfoOccupations != null)
            {
                var salaryInfoVM = Mapper.Map<IEnumerable<SalaryInfo>, IEnumerable<SalaryInfoVM>>(salaryInfoOccupations);

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
                    SalaryInfoId = vm.SalaryInfoId,
                    Likes = 0,
                    Title = vm.Title,
                    UploadedDate = DateTime.UtcNow,
                    Uploader = vm.Uploader,
                    YouTubeVideoId = vm.YouTubeVideoId,
                    SharerId = vm.SharerId,
                    VimeoId = vm.VimeoId
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
    }
}
