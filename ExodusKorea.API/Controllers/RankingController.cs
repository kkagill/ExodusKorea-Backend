using AutoMapper;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class RankingController : Controller
    {
        private readonly IRankingRepository _repository;
        private readonly IVideoPostRepository _vpRepository;

        public RankingController(IRankingRepository repository,
                                 IVideoPostRepository vpRepository)
        {
            _repository = repository;
            _vpRepository = vpRepository;
        }

        [HttpGet("uploader-ranking", Name = "GetUploaderRanking")]
        public async Task<IActionResult> GetUploaderRanking()
        {
            var uploaders = await _repository.GetAllUploaderVideoPosts();

            if (uploaders == null)
                return NotFound();

            var uploaderRankingVM = new List<UploaderRankingVM>();

            foreach (var u in uploaders)
            {
                uploaderRankingVM.Add(new UploaderRankingVM
                {
                    Name = u.Name,
                    UploaderId = u.UploaderId,
                    SpecificInfo = GetSpecificInfo(u)
                });
            }

            uploaderRankingVM = uploaderRankingVM.Take(5).OrderByDescending(x => x.SpecificInfo.TotalScore).ToList();

            return new OkObjectResult(uploaderRankingVM);            
        }

        [HttpGet("{uploaderId}/uploader-videos", Name = "GetUploaderVideos")]
        public IActionResult GetUploaderVideos(int uploaderId)
        {
            var uploaderVideos = _vpRepository.FindBy(vp => vp.UploaderId == uploaderId, vp => vp.Uploader, vp => vp.Country);

            if (uploaderVideos == null)
                return NotFound();

            var uploaderVideosVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(uploaderVideos);

            return new OkObjectResult(uploaderVideosVM);
        }

        private SpecificInfoVM GetSpecificInfo(Uploader uploader)
        {
            int uploaderVpCount = uploader.VideoPosts.Count;
            int likesSum = 0;

            if (uploaderVpCount > 0)
                foreach (var vp in uploader.VideoPosts)
                    likesSum += vp.Likes;

            var specificInfoVM = new SpecificInfoVM
            {
                VideoCount = uploaderVpCount,
                LikesSum = likesSum,
                TotalScore = (uploaderVpCount * 100) + likesSum
            };

            return specificInfoVM;
        }
    }
}
