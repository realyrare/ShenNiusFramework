﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
