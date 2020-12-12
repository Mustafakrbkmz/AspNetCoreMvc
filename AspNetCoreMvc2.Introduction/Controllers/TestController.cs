using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc2.Introduction.Controllers
{
    public class TestController : Controller
    {
        [Route("/error")]
        public IActionResult Index()
        {
            return View();
        }
    }
}