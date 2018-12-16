using ExodusKorea.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExodusKorea.API.Exceptions
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {     
        private readonly ILogDataService _logDataService;

        public CustomExceptionFilter(ILogDataService logDataService)
        {          
            _logDataService = logDataService;
        }
        public override void OnException(ExceptionContext context)
        {
            _logDataService.LogInternalServerException(context);
        }
    }
}