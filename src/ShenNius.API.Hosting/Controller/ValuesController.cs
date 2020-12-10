using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShenNius.API.Hosting.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 请求测试内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetContent2()
        {
            return Ok("test。。。");
        }
        [HttpPost]
        public IActionResult PostContent()
        {
            return Ok("009。。。");
        }
        [HttpPut]
        public IActionResult PutContent()
        {
            return Ok("hjhj。。。");
        }
        [HttpDelete]
        public IActionResult DelContent()
        {
            return Ok("123。。。");
        }
    }
}
