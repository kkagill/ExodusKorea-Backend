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

        public async Task<string> GetCountryByVideoId(string videoId)
        {
            var result = await _context.NewVideos
                .SingleOrDefaultAsync(x => x.YouTubeVideoId == videoId);

            return result.Country;
        }

        public async Task<PriceInfo> GetPriceInfoByCountry(string country)
        {
            var result = await _context.PriceInfo
                .SingleOrDefaultAsync(x => x.Country == country);

            return result;
        }

        public async Task<CountryInfo> GetCountryInfoByCountry(string country)
        {
            var result = await _context.CountryInfo
                .SingleOrDefaultAsync(x => x.CountryName == country);

            return result;
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
