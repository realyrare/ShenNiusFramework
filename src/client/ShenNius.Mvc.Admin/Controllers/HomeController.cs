﻿using Microsoft.AspNetCore.Mvc;
using ShenNius.Sys.API;

namespace ShenNius.Mvc.Admin.Controllers
{
    [LogIgnore]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {        
            return View();
        }
        [HttpGet("error.html")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
