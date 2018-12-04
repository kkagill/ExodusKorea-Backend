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
    public class CountryInfoRepository : EntityBaseRepository<object>, ICountryInfoRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;

        public CountryInfoRepository(ExodusKoreaContext context,
                              IConfiguration config)
            : base(context, config)
        {
            _context = context;
            _config = config;
        }

        public async Task<CountryInfoKOTRA> GetCountryInfoCanada()
        {
            var result = await _context.CountryInfoKOTRA
                .SingleOrDefaultAsync(x => x.CountryId == 2);

            return result;
        }
    }
}
