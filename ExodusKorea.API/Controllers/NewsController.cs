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
    public class NewsController : Controller
    {
        private readonly INewsRepository _repository;
        private readonly INewsDetailRepository _ndRepository;
        private readonly IYouTubeService _youTube;

        public NewsController(INewsRepository repository,
                              INewsDetailRepository ndRepository,
                              IYouTubeService youTube)
        {
            _repository = repository;
            _ndRepository = ndRepository;
            _youTube = youTube;
        }

        //[HttpGet]
        //[Route("all-news")]
        //public IActionResult GetAllNews()
        //{
        //    var allNews = _repository.GetAll();

        //    if (allNews == null)
        //        return NotFound();

        //    var allNewsVM = Mapper.Map<IEnumerable<News>, IEnumerable<NewsVM>>(allNews);

        //    foreach (var an in allNewsVM)
        //    {
        //        var newsDetail = _ndRepository.FindBy(nd => nd.NewsId == an.NewsId);
        //        an.NewsDetails = Mapper.Map<IEnumerable<NewsDetail>, IEnumerable<NewsDetailVM>>(newsDetail);
        //        an.NewsDetails = an.NewsDetails.OrderByDescending(x => x.DateCreated);
        //    }

        //    return new OkObjectResult(allNewsVM);
        //}

        [HttpGet]
        [Route("main-news")]
        public IActionResult GetMainNews()
        {         
            var mainNews = _ndRepository.GetAll();

            if (mainNews == null)
                return NotFound();

            var mainNewsVM = Mapper.Map<IEnumerable<NewsDetail>, IEnumerable<NewsDetailVM>>(mainNews);
            mainNewsVM = mainNewsVM.OrderByDescending(x => x.DateCreated);
            mainNewsVM = mainNewsVM.Take(5);

            return new OkObjectResult(mainNewsVM);
        }

        [HttpGet]
        [Route("{newsDetailId}/news-detail", Name = "GetNewsDetail")]
        public IActionResult GetNewsDetail(int newsDetailId)
        {
            if (newsDetailId <= 0)
                return NotFound();

            var newsDetail = _ndRepository.GetSingle(nd => nd.NewsDetailId == newsDetailId);

            if (newsDetail == null)
                return NotFound();

            var newsDetailVM = Mapper.Map<NewsDetail, NewsDetailVM>(newsDetail);

            return new OkObjectResult(newsDetailVM);
        }

        [HttpGet]
        [Route("{newsId}/news-list", Name = "GetNewsList")]
        public IActionResult GetNewsList(int newsId)
        {
            if (newsId <= 0)
                return NotFound();

            var newsDetails = _ndRepository.FindBy(nd => nd.NewsId == newsId);

            if (newsDetails == null)
                return NotFound();

            var newsDetailsVM = Mapper.Map<IEnumerable<NewsDetail>, IEnumerable<NewsDetailVM>>(newsDetails);

            return new OkObjectResult(newsDetailsVM);
        }
    }
}