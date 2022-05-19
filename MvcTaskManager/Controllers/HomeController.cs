using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MvcTaskManager.Controllers
{
    public class HomeController : Controller
    {
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
        [Route("home/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}



