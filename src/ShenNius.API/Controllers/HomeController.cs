using Microsoft.AspNetCore.Mvc;

namespace ShenNius.API.Controllers
{

    public class HomeController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult GetContent()
        {
            return Ok("testddfdffdffddffdfd。。。");
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
