using ExodusKorea.API.Services;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExodusKorea.API.Exceptions
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;   
        private readonly IMessageService _email;
        private readonly ExodusKoreaContext _context;
        //private AppSettings _appSettings;

        public CustomExceptionFilter(IHttpContextAccessor httpContextAccessor,
                                     IConfiguration config,                                  
                                     IMessageService email,
                                     ExodusKoreaContext context
                                   /*IOptions<AppSettings> appSettings*/)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _context = context;
            _email = email;
            //_appSettings = appSettings.Value;
        }
        public override void OnException(ExceptionContext context)
        {
            //bool isDemo = _appSettings.Environment == "Development" ? true : false;
            GlobalException exception = new GlobalException(_httpContextAccessor, _config, _context, _email);          
            exception.HandleException(context.Exception.ToString(), context.Exception.Message.ToString(), context.Exception.StackTrace);
        }
    }
}