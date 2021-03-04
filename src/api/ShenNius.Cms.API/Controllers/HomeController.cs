using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Cms.API.Controllers
{
    /// <summary>
    /// 产品控制器
    /// </summary>
      [Authorize]
    public class HomeController: ApiControllerBase
    {
        /// <summary>
        /// 请求测试内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
      
        public IActionResult GetContent()
        {
            return Ok("testddfdffdffddffdfd。。。");
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostContent()
        {
            return Ok("009。。。");
        }
        /// <summary>
        /// put 请求
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult PutContent()
        {
            return Ok("hjhj。。。");
        }
        /// <summary>
        /// put 请求
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
      
        public IActionResult DelContent()
        {
            return Ok("123。。。");
        }
    }
}
