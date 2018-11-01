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
        Task<string> GetCountryByVideoId(string videoId);
        Task<CountryInfo> GetCountryInfoByCountry(string country);
        Task<PriceInfo> GetPriceInfoByCountry(string country);
    }
}