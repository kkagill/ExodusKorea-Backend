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
using ExodusKorea.Model;
using Microsoft.Extensions.Options;

namespace ExodusKorea.Data.Repositories
{
    public class HomeRepository : EntityBaseRepository<object>, IHomeRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;

        public HomeRepository(ExodusKoreaContext context,
                                   IConfiguration config,
                                   IOptions<AppSettings> appSettings)
            : base(context, config, appSettings)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings.Value;
        }

        public async Task<IEnumerable<VideoPost>> GetAllNewVideos()
        {
            //var twoWeeks = DateTime.Now.Date.AddDays(-21);
            var result = await _context.VideoPosts
                .Include(x => x.Country)
                .Include(x => x.Uploader)
                .Include(x => x.Category)
                .Where(x => !x.IsDisabled)
                //.Where(x => x.UploadedDate >= twoWeeks)
                .OrderByDescending(x => x.UploadedDate)
                .Take(12)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<VideoPost>> GetPopularVideos()
        {            
            var result = await _context.VideoPosts
                .Include(x => x.Country)
                .Include(x => x.Uploader)
                .Include(x => x.Category)
                .Where(x => !x.IsDisabled)
                .OrderByDescending(x => x.Likes)
                .Take(8)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<VideoPost>> GetAllVideos()
        {
            var result = await _context.VideoPosts
                .Include(x => x.Country)
                .Include(x => x.Uploader)
                .Include(x => x.Category)
                .Where(x => !x.IsDisabled)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var result = await _context.Category               
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Career>> GetAllCareers()
        {
            var result = await _context.Career
                .ToListAsync();

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
