﻿using ExodusKorea.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExodusKorea.Data.Interfaces
{
    public interface IHomeRepository : IEntityBaseRepository<object>
    {
        Task<IEnumerable<VideoPost>> GetAllNewVideos();
    }

    public interface ICardDetailRepository : IEntityBaseRepository<object>
    {
        Task<string> GetCountryById(int videoPostId);
        Task<SalaryInfo> GetSalaryInfoById(int videoPostId);
        Task<CountryInfo> GetCountryInfoByCountry(string country);
        Task<PriceInfo> GetPriceInfoByCountry(string country);
        Task<IEnumerable<string>> GetMajorCitiesByCountry(string country);
        Task<PI_Rent> GetPI_RentByCity(string city);
        Task<PI_Restaurant> GetPI_RestaurantByCity(string city);
        Task<PI_Groceries> GetPI_GroceriesByCity(string city);
        Task<PI_Etc> GetPI_EtcByCity(string city);
    }

    public interface IVideoPostRepository : IEntityBaseRepository<VideoPost> { }
    public interface IVideoCommentRepository : IEntityBaseRepository<VideoComment> {}
    public interface IVideoCommentReplyRepository : IEntityBaseRepository<VideoCommentReply> { }
    public interface IVideoPostLikeRepository : IEntityBaseRepository<VideoPostLike> { }
    public interface IVideoCommentLikeRepository : IEntityBaseRepository<CommentLike> { }
    public interface IVideoCommentReplyLikeRepository : IEntityBaseRepository<CommentReplyLike> { }
    public interface INotificationRepository : IEntityBaseRepository<Notification> { }
}