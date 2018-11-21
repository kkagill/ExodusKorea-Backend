using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExodusKorea.Data;
using ExodusKorea.Model.Entities;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Data.Repositories;

namespace ExodusKorea.Data.Repositories
{
    public class CardDetailRepository : EntityBaseRepository<object>, ICardDetailRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;

        public CardDetailRepository(ExodusKoreaContext context,
                                    IConfiguration config)
            : base(context, config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> GetCountryById(int videoPostId)
        {
            var result = await _context.VideoPosts
                .SingleOrDefaultAsync(x => x.VideoPostId == videoPostId);

            return result.CountryInEng;
        }

        public async Task<SalaryInfo> GetSalaryInfoById(int videoPostId)
        {
            var result = await _context.SalaryInfo
                     .SingleOrDefaultAsync(x => x.VideoPostId == videoPostId);

            return result;
        }

        public async Task<PriceInfo> GetPriceInfoByCountry(string country)
        {    
            var result = await _context.PriceInfo
                     .SingleOrDefaultAsync(x => x.Country.Equals(country));          

            return result;
        }

        public async Task<IEnumerable<string>> GetMajorCitiesByCountry(string country)
        {
            var result = await _context.PI_Rent
                   .Where(x => x.Country.Equals(country))
                   .Select(x => x.City)
                   .ToListAsync();

            return result;
        }

        public async Task<PI_Rent> GetPI_RentByCity(string city)
        {
             var result = await _context.PI_Rent
                     .SingleOrDefaultAsync(x => x.City.Equals(city));

            return result;
        }

        public async Task<PI_Restaurant> GetPI_RestaurantByCity(string city)
        {
            var result = await _context.PI_Restaurant
                    .SingleOrDefaultAsync(x => x.City.Equals(city));

            return result;
        }

        public async Task<PI_Groceries> GetPI_GroceriesByCity(string city)
        {
            var result = await _context.PI_Groceries
                        .SingleOrDefaultAsync(x => x.City.Equals(city));

            return result;
        }

        public async Task<PI_Etc> GetPI_EtcByCity(string city)
        {
            var result = await _context.PI_Etc
                     .SingleOrDefaultAsync(x => x.City.Equals(city));

            return result;
        }

        public async Task<CountryInfo> GetCountryInfoByCountry(string country)
        {
            var result = await _context.CountryInfo
                .SingleOrDefaultAsync(x => x.CountryInEng.Equals(country));

            return result;
        }     

        public async Task<IEnumerable<City>> GetAllCitiesByCountry(string country)
        {
            var result = await _context.City
                .Where(x => x.Country.Equals(country))
                .ToListAsync();

            return result;
        }

        public async Task<int> GetCountryIdByCountry(string country)
        {
            var result = await _context.CountryInfo
                .SingleOrDefaultAsync(x => x.CountryInEng.Equals(country));
                
            return result.CountryInfoId;
        }

        public async Task<string> GetCityById(int city)
        {
            var result = await _context.City
                .SingleOrDefaultAsync(x => x.CityId == city);

            return result.Name;
        }

        // This is used when we know Hospital/Clinic doesn't send multiple ExamTypes per HL7 Message
        //public stp_GetWaitTimeAndCount GetWaitTimeAndCount(string code, string offset)
        //{
        //    stp_GetWaitTimeAndCount result = null;            

        //    try
        //    {
        //        using (var connection = Connection())
        //        {
        //            connection.Open();

        //            if (code != null)
        //            {
        //                DynamicParameters param = new DynamicParameters();
        //                param.Add("@Code", code);
        //                param.Add("@Offset", offset);
        //                var objDetails = SqlMapper.QueryMultiple(connection, "dbo.stp_GetWaitTimeAndCount", param, commandType: CommandType.StoredProcedure);
        //                result = new stp_GetWaitTimeAndCount()
        //                {
        //                    Count = objDetails.Read<int>().SingleOrDefault(),
        //                    Time = objDetails.Read<int>().ToList()
        //                };
        //            }

        //            connection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.HResult == -2146232798)
        //            return null;

        //        throw ex;
        //    }

        //    return result;
        //}    
    }
}
