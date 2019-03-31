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
using Microsoft.Extensions.Options;
using ExodusKorea.Model;

namespace ExodusKorea.Data.Repositories
{
    public class VideoPostRepository : EntityBaseRepository<VideoPost>, IVideoPostRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;

        public VideoPostRepository(ExodusKoreaContext context,
                                   IConfiguration config,
                                   IOptions<AppSettings> appSettings)
            : base(context, config, appSettings)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings.Value;
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            var result = await _context.Country
              .ToListAsync();

            return result;
        }
    }
}
