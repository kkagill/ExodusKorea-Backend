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
        private readonly IYouTubeService _youTube;

        public HomeController(IHomeRepository repository,
                              IYouTubeService youTube)
        {
            _repository = repository;
            _youTube = youTube;
        }

        [HttpGet]
        [Route("new-videos")]
        public async Task<IActionResult> GetNewVideos()
        {
            var allVideoPosts = await _repository.GetAllNewVideos();

            if (allVideoPosts == null)
                return NotFound();

            var allVideoVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(allVideoPosts);
            
            //foreach (var av in allVideoVM)
            //{
            //    var likes = await _youTube.GetYouTubeLikesByVideoId(av.YouTubeVideoId);
            //    av.Likes = av.Likes + Convert.ToInt32(likes);
            //}

            return new OkObjectResult(allVideoVM);
        }

        [HttpGet("{country}/videos-by-country", Name = "GetVideosByCountry")]
        public async Task<IActionResult> GetVideosByCountry(string country)
        {
            var videos = await _repository.GetVideosByCountry(country);

            if (videos == null)
                return NotFound();

            var random = new Random();
            videos = videos.OrderBy(x => random.Next()).Take(4);

            var randomVideosVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(videos);

            //foreach (var av in allVideoVM)
            //{
            //    var likes = await _youTube.GetYouTubeLikesByVideoId(av.YouTubeVideoId);
            //    av.Likes = av.Likes + Convert.ToInt32(likes);
            //}

            return new OkObjectResult(randomVideosVM);
        }
    }
}
