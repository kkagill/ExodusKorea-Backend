using AutoMapper;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
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

        #region Uploader Ranking
        [HttpGet("uploader-ranking", Name = "GetUploaderRanking")]
        public async Task<IActionResult> GetUploaderRanking()
        {
            var uploaders = await _repository.GetAllUploaderVideoPosts();

            if (uploaders == null)
                return NotFound();

            var uploaderRankingVM = new List<UploaderRankingVM>();

            foreach (var u in uploaders)
            {    
                if (u.Name.Trim().Length > 10)
                {
                    var substringed = u.Name.Substring(0, 10);
                    var concat = string.Concat(substringed + "..");

                    uploaderRankingVM.Add(new UploaderRankingVM
                    {
                        Name = concat,
                        NameOriginal = u.Name,
                        ThumbnailDefaultUrl = u.YouTubeChannelThumbnailUrl,
                        UploaderId = u.UploaderId,
                        SpecificInfo = GetSpecificInfo(u)
                    });
                } 
                else
                    uploaderRankingVM.Add(new UploaderRankingVM
                    {
                        Name = u.Name,
                        NameOriginal = u.Name,
                        ThumbnailDefaultUrl = u.YouTubeChannelThumbnailUrl,
                        UploaderId = u.UploaderId,
                        SpecificInfo = GetSpecificInfo(u)
                    });      
            }

            uploaderRankingVM = uploaderRankingVM.OrderByDescending(x => x.SpecificInfo.TotalScore).ToList();              

            return new OkObjectResult(uploaderRankingVM);
        }

        [HttpGet("{uploaderId}/uploader-videos", Name = "GetUploaderVideos")]
        public IActionResult GetUploaderVideos(int uploaderId)
        {
            var uploaderVideos = _vpRepository
                .FindBy(vp => vp.UploaderId == uploaderId, vp => vp.Uploader, vp => vp.Country, vp => vp.Uploader, vp => vp.Category);

            if (uploaderVideos == null)
                return NotFound();

            var uploaderVideosVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(uploaderVideos);

            return new OkObjectResult(uploaderVideosVM);
        }       
        #endregion

        #region Job Ranking
        [HttpGet("jobsindemand-random-countries", Name = "GetJobsInDemandByRandomCountries")]
        public async Task<IActionResult> GetJobsInDemandByRandomCountries()
        {
            var countries = await _repository.GetAllCountries();

            if (countries == null)
                return NotFound();

            var randomCountries = countries.OrderBy(x => new Random().Next()).Take(2).ToList();
            var jobsInDemands = await _repository.GetJobsInDemandByCountryIds(randomCountries);

            if (jobsInDemands == null)
                return NotFound();

            var jobsInDemandVM = new List<JobsInDemandVM>();

            foreach (var rc in randomCountries)
                jobsInDemandVM.Add(new JobsInDemandVM
                {
                    CountryKR = rc.NameKR,
                    Details = GetDetails(jobsInDemands
                    .Where(x => x.CountryId == rc.CountryId)
                    .OrderBy(x => x.TitleKR)
                    .ToList())
                });

            return new OkObjectResult(jobsInDemandVM);
        }

        [HttpGet("jobsindemand-all-countries", Name = "GetJobsInDemandByAllCountries")]
        public async Task<IActionResult> GetJobsInDemandByAllCountries()
        {
            var countries = await _repository.GetAllCountries();

            if (countries == null)
                return NotFound();

            var jobsInDemands = await _repository.GetAllJobInDemands();

            if (jobsInDemands == null)
                return NotFound();

            var jobsInDemandVM = new List<JobsInDemandVM>();

            foreach (var c in countries)
                jobsInDemandVM.Add(new JobsInDemandVM
                {
                    CountryKR = c.NameKR,
                    Details = GetDetails(jobsInDemands
                    .Where(x => x.CountryId == c.CountryId)
                    .OrderBy(x => x.TitleKR)
                    .ToList())
                });

            return new OkObjectResult(jobsInDemandVM);
        }

        [HttpGet("{jobsInDemandId}/jobsindemand-videos", Name = "GetJobsInDemandVideos")]
        public IActionResult GetJobsInDemandVideos(int jobsInDemandId)
        {
            var jobsInDemandVideos = _vpRepository
                .FindBy(vp => vp.JobsInDemandId == jobsInDemandId, vp => vp.Uploader, vp => vp.JobsInDemand, vp => vp.Country, vp => vp.Category);

            if (jobsInDemandVideos == null)
                return NotFound();

            var jobsInDemandVideosVM = Mapper.Map<IEnumerable<VideoPost>, IEnumerable<VideoPostVM>>(jobsInDemandVideos);

            return new OkObjectResult(jobsInDemandVideosVM);
        }
        #endregion

        #region Private Functions
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

        private List<Detail> GetDetails(List<JobsInDemand> jobsInDemand)
        {
            var details = new List<Detail>();

            foreach (var j in jobsInDemand)
            {
                var videoPosts = _vpRepository.FindBy(vp => vp.JobsInDemandId == j.JobsInDemandId).ToList();

                details.Add(new Detail
                {
                    JobsInDemandId = j.JobsInDemandId,
                    TitleKR = j.TitleKR,
                    TitleEN = j.TitleEN,
                    Description = j.Description,
                    HasVideoPost = videoPosts.Count <= 0 ? false : true,
                    IsRecommended = j.IsRecommended,
                    DifficultyLevel = j.DifficultyLevel,
                    Link = j.Link,
                    JobSite = j.JobSite,
                    Salary = j.Salary,
                    Currency = j.Country.BaseCurrency
                });
            }              

            return details;
        }
        #endregion
    }
}
