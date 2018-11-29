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
        [Route("all-countries")]
        public IActionResult GetAllCountries()
        {
            var allCountries = _hRepository.GetAllCountries();

            if (allCountries == null)
                return NotFound();    

            return new OkObjectResult(allCountries);
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

        [HttpGet("{countryId}/search-result-country", Name = "GetSearchResultByCountryId")]
        public IActionResult GetSearchResultByCountry(int countryId)
        {
            var videoPosts = _vpRepository.FindBy(vp => vp.CountryId == countryId, vp => vp.Country);
             
            if (videoPosts == null)
                return NotFound();
         
            var videoPostsVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(videoPosts);         

            return new OkObjectResult(videoPostsVM);
        }          
    }
}
