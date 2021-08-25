using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using System.Linq;
using System.Threading.Tasks;

/*************************************
* 类名：AppUserController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/23 11:01:15
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Shop.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [MultiTenant]
    [Log]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            var res = await _appUserService.GetPagesAsync(query.Page, query.Limit, d => d.TenantId == query.TenantId && d.Status == true, d => d.Id, false);
            var tenantService = HttpContext.RequestServices.GetService(typeof(ITenantService)) as ITenantService;
            var tenantList = await tenantService.GetListAsync(d => d.Status);
            foreach (var item in res.Items)
            {
                item.TenantName = tenantList.Where(d => d.Id == item.TenantId).Select(d => d.Name).FirstOrDefault();
            }
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
    }
}