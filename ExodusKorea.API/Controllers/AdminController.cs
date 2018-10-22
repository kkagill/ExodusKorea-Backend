using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExodusKorea.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AdminController : Controller
    {
        public AdminController()
        {

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Get()
        {
            var list = new List<int>
            {
                1,
                2
            };

            return new OkObjectResult(list);
        }
    }
}
