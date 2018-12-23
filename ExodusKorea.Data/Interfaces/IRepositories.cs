using ExodusKorea.Model.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExodusKorea.Data.Interfaces
{
    public interface IHomeRepository : IEntityBaseRepository<object>
    {
        Task<IEnumerable<VideoPost>> GetAllNewVideos();
        Task<IEnumerable<VideoPost>> GetAllVideos();
        Task<IEnumerable<Category>> GetAllCategories();
        Task<IEnumerable<Career>> GetAllCareers();
    }

    public interface ICardDetailRepository : IEntityBaseRepository<object>
    {
        Task<int> GetCountryIdByVideoPostId(int videoPostId);
        Task<Country> GetCountryByVideoPostId(int videoPostId);
        Task<VideoPost> GetVideoPostByVideoPostId(int videoPostId);
        Task<SalaryInfo> GetSalaryInfoByVideoPostId(int videoPostId);
        Task<CountryInfo> GetCountryInfoByCountryId(int countryId);
        Task<PriceInfo> GetPriceInfoByCountry(string country);
        Task<string> GetBaseCurrencyByCountry(string country);
        Task<IEnumerable<string>> GetMajorCitiesByCountry(string country);
        Task<PI_Rent> GetPI_RentByCity(string city);
        Task<PI_Restaurant> GetPI_RestaurantByCity(string city);
        Task<PI_Groceries> GetPI_GroceriesByCity(string city);
        Task<PI_Etc> GetPI_EtcByCity(string city);
        Task<IEnumerable<City>> GetAllCitiesByCountryId(int countryId);
        Task<CountryInfo> GetCountryInfoByCountry(string country);
        Task<int> GetCountryInfoIdByCountry(string country);
        Task<string> GetCityById(int city);
        Task<Career> GetCareerByVideoPostId(int videoPostId);
        IEnumerable<JobSite> GetAllJobSites();
    }

    public interface ICountryInfoRepository : IEntityBaseRepository<object>
    {
        Task<CountryInfoKOTRA> GetCountryInfoCanada();    
    }

    public interface IAccountRepository : IEntityBaseRepository<ApplicationUser>
    {
        Task<DateTime> GetUserRecentVisit(string userId);
        Task<int> GetVisitCountByUserId(string userId);
        Task LogWithdrewUser(string reason, ApplicationUser user);
        Task DeleteMyVideosAsync(string userId);
    }
    public interface IVideoPostRepository : IEntityBaseRepository<VideoPost> { }
    public interface IVideoCommentRepository : IEntityBaseRepository<VideoComment> { }
    public interface IVideoCommentReplyRepository : IEntityBaseRepository<VideoCommentReply> { }
    public interface IVideoPostLikeRepository : IEntityBaseRepository<VideoPostLike> { }
    public interface IVideoCommentLikeRepository : IEntityBaseRepository<CommentLike> { }
    public interface IVideoCommentReplyLikeRepository : IEntityBaseRepository<CommentReplyLike> { }
    public interface INotificationRepository : IEntityBaseRepository<Notification> { }
    public interface IMinimumCostOfLivingRepository : IEntityBaseRepository<MinimumCostOfLiving> { }
    public interface INewsRepository : IEntityBaseRepository<News> { }
    public interface INewsDetailRepository : IEntityBaseRepository<NewsDetail> { }
    public interface IMyVideosRepository : IEntityBaseRepository<MyVideos> { }
}