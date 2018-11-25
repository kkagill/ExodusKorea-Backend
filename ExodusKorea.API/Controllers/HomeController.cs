﻿using System;
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
        [Route("main-currencies")]
        public async Task<IActionResult> GetMainCurrencies()
        {
            var mainCurrencies = await _currencyRate.GetMainCurrencies();

            if (mainCurrencies == null)
                return NotFound();

            mainCurrencies.Today = DateTime.Now;

            return new OkObjectResult(mainCurrencies);
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

        [HttpGet("{country}/videos-by-country", Name = "GetVideosByCountry")]
        public async Task<IActionResult> GetVideosByCountry(string country)
        {
            var videos = await _repository.GetVideosByCountry(country);

            if (videos == null)
                return NotFound();

            var random = new Random();
            videos = videos.OrderBy(x => random.Next()).Take(4);

            var randomVideosVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(videos);

            return new OkObjectResult(randomVideosVM);
        }
    }
}
