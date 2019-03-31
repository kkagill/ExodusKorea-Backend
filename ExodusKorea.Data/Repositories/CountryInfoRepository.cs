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
    public class CountryInfoRepository : EntityBaseRepository<object>, ICountryInfoRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;

        public CountryInfoRepository(ExodusKoreaContext context,
                                   IConfiguration config,
                                   IOptions<AppSettings> appSettings)
            : base(context, config, appSettings)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<PromisingField> GetAllPromisingFields()
        {
            var result = _context.PromisingField.AsEnumerable();

            return result;
        }

        public IEnumerable<SettlementGuide> GetAllSettlementGuides()
        {
            var result = _context.SettlementGuide.AsEnumerable();

            return result;
        }

        public IEnumerable<LivingCondition> GetAllLivingConditions()
        {
            var result = _context.LivingCondition.AsEnumerable();

            return result;
        }

        public IEnumerable<ImmigrationVisa> GetAllImmigrationVisas()
        {
            var result = _context.ImmigrationVisa.AsEnumerable();

            return result;
        }
    }
}
