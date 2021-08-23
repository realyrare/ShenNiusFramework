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
    public class AppUserAddressController : ControllerBase
    {
        private readonly IAppUserAddressService _appUserAddressService;

        public AppUserAddressController(IAppUserAddressService appUserAddressService)
        {
            this._appUserAddressService = appUserAddressService;
        }

        [HttpGet]
        public  Task<ApiResult> GetListPages(KeyListTenantQuery query)
        {
            return _appUserAddressService.GetListPageAsync(query);
        }
    }
}