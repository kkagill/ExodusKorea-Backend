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
    public class SearchVideoController : Controller
    {
        private readonly IVideoPostRepository _vpRepository;
        private readonly IHomeRepository _hRepository;

        public SearchVideoController(IVideoPostRepository vpRepository,
                                     IHomeRepository hRepository)
        {
            _vpRepository = vpRepository;
            _hRepository = hRepository;
        }

        [HttpGet]
        [Route("all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var allCategories = await _hRepository.GetAllCategories();

            if (allCategories == null)
                return NotFound();    

            return new OkObjectResult(allCategories);
        }

        [HttpGet]
        [Route("all-careers")]
        public async Task<IActionResult> GetAllCareers()
        {
            var allCareers = await _hRepository.GetAllCareers();

            if (allCareers == null)
                return NotFound();

            allCareers = allCareers.OrderBy(x => x.Name);

            return new OkObjectResult(allCareers);
        }

        [HttpGet]
        [Route("all-search-result")]
        public IActionResult GetAllSearchResult()
        {
            var allVideoPosts = _vpRepository.AllIncluding(vp => vp.Country);

            if (allVideoPosts == null)
                return NotFound();

            var allVideoVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(allVideoPosts);

            return new OkObjectResult(allVideoVM);
        }

        [HttpGet("{categoryId}/search-result-category", Name = "GetSearchResultByCategory")]
        public IActionResult GetSearchResultByCategory(int categoryId)
        {
            var videoPosts = _vpRepository
                .FindBy(vp => vp.CategoryId == categoryId && vp.CountryId == 2, vp => vp.Country);
             
            if (videoPosts == null)
                return NotFound();
         
            var videoPostsVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(videoPosts);         

            return new OkObjectResult(videoPostsVM);
        }

        [HttpGet("{careerId}/search-result-career", Name = "GetSearchResultByCareer")]
        public IActionResult GetSearchResultByCareer(int careerId)
        {
            var videoPosts = _vpRepository.FindBy(vp => vp.CareerId == careerId, vp => vp.Country);

            if (videoPosts == null)
                return NotFound();

            var videoPostsVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(videoPosts);
            
            return new OkObjectResult(videoPostsVM);
        }
    }
}
