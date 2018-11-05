using ExodusKorea.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExodusKorea.Data.Interfaces
{
    public interface IHomeRepository : IEntityBaseRepository<object>
    {
        Task<IEnumerable<NewVideo>> GetAllNewVideos();
    }

    public interface ICardDetailRepository : IEntityBaseRepository<object>
    {
        Task<string> GetCountryById(int newVideoId);
        Task<CountryInfo> GetCountryInfoByCountry(string country);
        Task<PriceInfo> GetPriceInfoByCountry(string country);
    }

    public interface INewVideoRepository : IEntityBaseRepository<NewVideo> { }
    public interface IVideoCommentRepository : IEntityBaseRepository<VideoComment> {}
    public interface IVideoCommentReplyRepository : IEntityBaseRepository<VideoCommentReply> { }
}