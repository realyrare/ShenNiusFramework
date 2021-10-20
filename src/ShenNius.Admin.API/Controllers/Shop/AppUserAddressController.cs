using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
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

namespace ShenNius.Admin.API.Controllers.Shop
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [MultiTenant]
    public class AppUserAddressController : ControllerBase
    {
        private readonly IAppUserAddressService _appUserAddressService;

        public AppUserAddressController(IAppUserAddressService appUserAddressService)
        {
            _appUserAddressService = appUserAddressService;
        }
        [HttpGet]
        public  Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            return _appUserAddressService.GetListPageAsync(query);
        }
    }
}