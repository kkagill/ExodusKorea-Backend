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
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly INewsRepository _repository;
        private readonly INewsDetailRepository _ndRepository;
        private readonly IYouTubeService _youTube;
        private readonly IKotraNewsService _kotraNews;
        private readonly IMemoryCache _memoryCache;

        public NewsController(INewsRepository repository,
                              INewsDetailRepository ndRepository,
                              IYouTubeService youTube,
                              IKotraNewsService kotraNews,
                              IMemoryCache memoryCache)
        {
            _repository = repository;
            _ndRepository = ndRepository;
            _youTube = youTube;
            _kotraNews = kotraNews;
            _memoryCache = memoryCache;
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
        [Route("all-categories")]
        public IActionResult GetAllCategories()
        {
            var categories = new List<CategoryVM>();
            var allCategories = _repository.GetAll();

            foreach (var c in allCategories)
                categories.Add(new CategoryVM
                {
                    CategoryId = c.NewsId,
                    Name = c.Topic
                });
            
            return new OkObjectResult(categories);
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
        public async Task<IActionResult> GetNewsList(int newsId)
        {
            if (newsId < 0)
                return NotFound();

            var news = _repository.GetSingle(n => n.NewsId == newsId);      
            var cache = _memoryCache.Get<List<KotraNewsVM>>(news.Topic);

            if (cache != null)
                return new OkObjectResult(cache);

            var newsDetails = await _kotraNews.GetKotraNewsByCountry(news.Topic);

            if (newsDetails == null)
                return NotFound();

            return new OkObjectResult(newsDetails);
        }

        //[HttpGet]
        //[Route("{newsId}/news-list", Name = "GetNewsList")]
        //public IActionResult GetNewsList(int newsId)
        //{
        //    if (newsId < 0)
        //        return NotFound();

        //    if (newsId == 0)
        //    {
        //        var allNewsDetails = _ndRepository.GetAll();

        //        if (allNewsDetails == null)
        //            return NotFound();

        //        var allNewsDetailsVM = Mapper.Map<IEnumerable<NewsDetail>, IEnumerable<NewsDetailVM>>(allNewsDetails);

        //        foreach (var and in allNewsDetailsVM)
        //            and.NewsId = 0;

        //        allNewsDetailsVM = allNewsDetailsVM.OrderByDescending(x => x.DateCreated);

        //        return new OkObjectResult(allNewsDetailsVM);
        //    }

        //    var newsDetails = _ndRepository.FindBy(nd => nd.NewsId == newsId);

        //    if (newsDetails == null)
        //        return NotFound();

        //    var newsDetailsVM = Mapper.Map<IEnumerable<NewsDetail>, IEnumerable<NewsDetailVM>>(newsDetails);
        //    newsDetailsVM = newsDetailsVM.OrderByDescending(x => x.DateCreated);

        //    return new OkObjectResult(newsDetailsVM);
        //}

        [HttpPut]
        [Route("{newsDetailId}/update-views-count")]
        public async Task<IActionResult> UpdateViewsCount(int newsDetailId)
        {
            if (newsDetailId <= 0)
                return NotFound();

            var newsDetail = _ndRepository.GetSingle(vc => vc.NewsDetailId == newsDetailId);

            if (newsDetail == null)
                return NotFound();
            else
                newsDetail.Views += 1;

            _ndRepository.Update(newsDetail);
            await _ndRepository.CommitAsync();

            return new NoContentResult();
        }

        [HttpGet]
        [Route("popular-news")]
        public IActionResult GetPopularNews()
        {
            var allNewsDetails = _ndRepository.GetAll();
            
            if (allNewsDetails == null)
                return NotFound();

            var allNewsDetailsVM = Mapper.Map<IEnumerable<NewsDetail>, IEnumerable<NewsDetailVM>>(allNewsDetails);
            allNewsDetailsVM = allNewsDetailsVM.OrderByDescending(x => x.Views);
            allNewsDetailsVM = allNewsDetailsVM.Take(5);

            return new OkObjectResult(allNewsDetailsVM);
        }
    }
}
