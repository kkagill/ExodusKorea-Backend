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
    public class AdminRepository : EntityBaseRepository<object>, IAdminRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;

        public AdminRepository(ExodusKoreaContext context,
                              IConfiguration config)
            : base(context, config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var result = await _context.Country.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var result = await _context.Category.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Uploader>> GetUploaders()
        {
            var result = await _context.Uploader.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SalaryInfo>> GetSalaryInfoOccupations(string country)
        {
            var result = await _context.SalaryInfo
                .Where(x => x.Country.Equals(country))
                .ToListAsync();

            return result;
        }
    }
}
