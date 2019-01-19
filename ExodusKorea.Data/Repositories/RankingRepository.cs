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
    public class RankingRepository : EntityBaseRepository<object>, IRankingRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;

        public RankingRepository(ExodusKoreaContext context,
                              IConfiguration config)
            : base(context, config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<Uploader>> GetAllUploaderVideoPosts()
        {
            var result = await _context.Uploader             
                .Include(x => x.VideoPosts)            
                .Where(x => x.VideoPosts.Any(vp => !vp.IsDisabled))
                .ToListAsync();

            return result;
        }
    }
}
