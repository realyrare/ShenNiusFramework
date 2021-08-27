using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Output.MiniApp;
using System.Linq;
using System.Net;

/*************************************
* 类名：MiniAppBaseController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/27 15:34:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.MiniApp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MiniAppBaseController : Controller
    {
        public HttpMiniUser appUser { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["token"].FirstOrDefault() ?? context.HttpContext.Request.Query["token"].FirstOrDefault() ?? context.HttpContext.Request.Form["token"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new ApiResult()
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Msg = "未登录!缺少必要的参数：token"
                });
                return;
            }
            ICacheHelper cache = context.HttpContext.RequestServices.GetRequiredService(typeof(ICacheHelper)) as ICacheHelper;
            appUser = cache.Get<HttpMiniUser>(token);
            if (appUser == null)
            {
                context.Result = new JsonResult(new ApiResult()
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Msg = "未登录!缺少必要的参数：token失效了"
                });
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}