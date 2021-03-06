﻿using ExodusKorea.Model.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExodusKorea.Data.Interfaces
{
    public interface IHomeRepository : IEntityBaseRepository<object>
    {
        Task<IEnumerable<VideoPost>> GetAllNewVideos();
        Task<IEnumerable<VideoPost>> GetPopularVideos();
        Task<IEnumerable<VideoPost>> GetAllVideos();
        Task<IEnumerable<Category>> GetAllCategories();
        Task<IEnumerable<Career>> GetAllCareers();
    }

    public interface ICardDetailRepository : IEntityBaseRepository<object>
    {
        Task<int> GetCountryIdByVideoPostId(int videoPostId);
        Task<Country> GetCountryByVideoPostId(int videoPostId);
        Task<VideoPost> GetVideoPostByVideoPostId(int videoPostId);
        //Task<SalaryInfo> GetSalaryInfoByVideoPostId(int videoPostId);
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
        Task<IEnumerable<JobSite>> GetJobSitesByCountryId(int countryId);
    }

    public interface ICountryInfoRepository : IEntityBaseRepository<object>
    {
        IEnumerable<PromisingField> GetAllPromisingFields();
        IEnumerable<SettlementGuide> GetAllSettlementGuides();
        IEnumerable<LivingCondition> GetAllLivingConditions();
        IEnumerable<ImmigrationVisa> GetAllImmigrationVisas();
    }

    public interface IAccountRepository : IEntityBaseRepository<ApplicationUser>
    {
        Task<DateTime> GetUserRecentVisit(string userId);
        Task<int> GetVisitCountByUserId(string userId);
        Task LogWithdrewUser(string reason, ApplicationUser user);
        Task DeleteMyVideosAsync(string userId);
    }
    public interface IVideoPostRepository : IEntityBaseRepository<VideoPost>
    {
        Task<IEnumerable<Country>> GetAllCountries();
    }
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
    public interface IUploadVideoRepository : IEntityBaseRepository<UploadVideo> { }
    public interface ISalaryInfoRepository : IEntityBaseRepository<SalaryInfo> { }
    public interface IUploaderRepository : IEntityBaseRepository<Uploader> { }
    public interface IRankingRepository : IEntityBaseRepository<object>
    {
        Task<IEnumerable<Uploader>> GetAllUploaderVideoPosts();
        Task<IEnumerable<Country>> GetAllCountries();
        Task<IEnumerable<JobsInDemand>> GetJobsInDemandByCountryIds(List<Country> countries);
        Task<IEnumerable<JobsInDemand>> GetAllJobInDemands();
    }
    public interface IAdminRepository : IEntityBaseRepository<object>
    {
        Task<IEnumerable<Country>> GetCountries();
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Career>> GetCareers();
        Task<IEnumerable<Uploader>> GetUploaders();
        Task<IEnumerable<SalaryInfo>> GetSalaryInfoOccupations(string country);
    }
}