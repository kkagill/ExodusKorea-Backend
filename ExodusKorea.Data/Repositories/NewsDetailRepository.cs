﻿using Microsoft.Extensions.Configuration;
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
    public class NewsDetailRepository : EntityBaseRepository<NewsDetail>, INewsDetailRepository
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;

        public NewsDetailRepository(ExodusKoreaContext context,
                                   IConfiguration config,
                                   IOptions<AppSettings> appSettings)
            : base(context, config, appSettings)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings.Value;
        }
    }
}
