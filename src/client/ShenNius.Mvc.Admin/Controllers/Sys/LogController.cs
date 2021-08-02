using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class LogController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Detail()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Echarts()
        {
            return View();
        }
    }
}
