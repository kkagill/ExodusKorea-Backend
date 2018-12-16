using System;
using System.Threading.Tasks;
using ExodusKorea.API.Exceptions;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    public class HttpResponseController : Controller
    {        
        private ILogDataService _logDataService;

        public HttpResponseController(ILogDataService logDataService)
        {
            _logDataService = logDataService;
        }

        [HttpPost]
        [Route("log-http-response")]
        public async Task<IActionResult> LogHttpResponseException([FromBody] HttpResponseExceptionVM vm)
        {            
            await _logDataService.LogHttpResponseException(vm);
           
            return new NoContentResult();
        }

        [HttpPost]
        [Authorize]
        [Route("log-loggedin-http-response")]
        public async Task<IActionResult> LogLoggedInHttpResponseException([FromBody] HttpResponseExceptionVM vm)
        {
            await _logDataService.LogHttpResponseException(vm);

            return new NoContentResult();
        }
    }
}
