using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public HomeController(IHomeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("new-videos")]
        public async Task<IActionResult> GetNewVideos()
        {
            var allNewVideos = await _repository.GetAllNewVideos();

            if (allNewVideos == null)
                return BadRequest();

            var newVideosVM = Mapper.Map<IEnumerable<NewVideo>, IEnumerable<NewVideosVM>>(allNewVideos);

            foreach (var nv in newVideosVM)
            {
                // Todo: add comment to NewVideosVM 
                // 1. Get comments from youtube api 
                // 2. Get comments from VideoComment table
                // 3. Add those two
            }

            return new OkObjectResult(newVideosVM);
        }
    }
}
