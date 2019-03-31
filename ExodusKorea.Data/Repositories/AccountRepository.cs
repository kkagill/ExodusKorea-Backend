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
    public class AccountRepository : EntityBaseRepository<ApplicationUser>, IAccountRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;

        public AccountRepository(ExodusKoreaContext context,
                                   IConfiguration config,
                                   IOptions<AppSettings> appSettings)
            : base(context, config, appSettings)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings.Value;
        }

        public async Task<DateTime> GetUserRecentVisit(string userId)
        {
            var result = await _context.LoginSession
                .Where(x => x.UserId == userId)
                .OrderByDescending(t => t.LoginTime)
                .FirstOrDefaultAsync();

            return result.LoginTime;
        }

        public async Task<int> GetVisitCountByUserId(string userId)
        {
            var result = await _context.LoginSession
                .Where(x => x.UserId == userId && x.LoginType.Equals("password"))
                .ToListAsync();
                
            return result.Count();
        }

        public async Task LogWithdrewUser(string reason, ApplicationUser user)
        {
            WithdrawUser withdrawUser = new WithdrawUser
            {               
                Email = user.Email,
                NickName = user.NickName,
                Reason = reason,
                DateWithdrew = DateTime.UtcNow
            };

            await _context.WithdrawUser.AddAsync(withdrawUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMyVideosAsync(string userId)
        {
            var myVideos = _context.MyVideos.Where(x => x.ApplicationUserId.Equals(userId));

            _context.RemoveRange(myVideos);
            await _context.SaveChangesAsync();
        }
    }
}
