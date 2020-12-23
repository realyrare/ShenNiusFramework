using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.OrderApi.Controllers
{
   public class OrderController:ApiControllerBase
    {
        /// <summary>
        /// 请求测试内容4444
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok("testddfdffdffddffdfd。。。");
        }
        [HttpPost]
        public IActionResult Login()
        {
            return Ok("testddfdffdffddffdfd。。。");
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddOrder()
        {
            return Ok("009。。。");
        }
    }
}
