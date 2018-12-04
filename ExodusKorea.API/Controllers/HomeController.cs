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
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repository;
        private readonly ICurrencyRatesService _currencyRate;
        private readonly IYouTubeService _youTube;

        public HomeController(IHomeRepository repository,
                              ICurrencyRatesService currencyRate,
                              IYouTubeService youTube)
        {
            _repository = repository;
            _currencyRate = currencyRate;
            _youTube = youTube;
        }

        [HttpGet]
        [Route("recommended-video")]
        public async Task<IActionResult> GetRecommendedVideo()
        {
            var allVideoPosts = await _repository.GetAllNewVideos();

            if (allVideoPosts == null)
                return NotFound();

            var random = new Random();
            var convertedToList = allVideoPosts.OrderBy(x => random.Next()).Take(1).ToList();
            var recommendedVideoVM = Mapper.Map<VideoPost, VideoPostVM>(convertedToList[0]);         

            return new OkObjectResult(recommendedVideoVM);
        }

        [HttpGet]
        [Route("currency")]
        public async Task<IActionResult> GetCurrency()
        {
            var krwRate = await _currencyRate.GetKRWRateByCountry("CAD");

            if (string.IsNullOrEmpty(krwRate))
                return NotFound();

            return new OkObjectResult(new CurrencyInfoVM
            {
                Country = "캐나다",
                BaseCurrency = "CAD",
                KrwRate = krwRate,
                Now = DateTime.Now
            });
        }     

        [HttpGet]
        [Route("new-videos")]
        public async Task<IActionResult> GetNewVideos()
        {
            var allVideoPosts = await _repository.GetAllNewVideos();

            if (allVideoPosts == null)
                return NotFound();

            var allVideoVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(allVideoPosts); 

            return new OkObjectResult(allVideoVM);
        }

        [HttpGet]
        [Route("all-videos")]
        public async Task<IActionResult> GetAllVideos()
        {
            var allVideoPosts = await _repository.GetAllVideos();

            if (allVideoPosts == null)
                return NotFound();

            var allVideoPostsVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(allVideoPosts);           

            return new OkObjectResult(allVideoPostsVM);
        }

        //[HttpGet]
        //[Route("all-videos")]
        //public async Task<IActionResult> GetAllVideos()
        //{
        //    var allVideoPosts = await _repository.GetAllVideos();

        //    if (allVideoPosts == null)
        //        return NotFound();

        //    var random = new Random();
        //    var allVideosVM = new List<AllVideosVM>();
        //    var allCategories = await _repository.GetAllCategories();

        //    if (allCategories == null)
        //        return NotFound();

        //    var allVideoPostsVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(allVideoPosts);                  

        //    foreach (var c in allCategories)
        //        allVideosVM.Add(new AllVideosVM
        //        {
        //            Category = c.Name,
        //            VideoPosts = allVideoPostsVM                     
        //                .Where(x => x.CategoryId == c.CategoryId)
        //                .OrderBy(x => random.Next())
        //                .Take(4)
        //        });

        //    return new OkObjectResult(allVideosVM);
        //}
    }
}
