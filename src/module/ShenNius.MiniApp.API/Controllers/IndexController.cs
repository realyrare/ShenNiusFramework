/*************************************
* 类名：IndexController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/30 16:48:29
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Enums.Cms;
using System.Threading.Tasks;

namespace ShenNius.MiniApp.API.Controllers
{
    public class IndexController: MiniAppBaseController
    {
        private readonly IGoodsService _goodsService;
        private readonly IAdvListService _advListService;

        public IndexController(IGoodsService goodsService, IAdvListService advListService)
        {
            _goodsService = goodsService;
            _advListService = advListService;
        }
        [HttpGet("page")]
        public async Task<ApiResult> Page()
        {
            var query = new ListTenantQuery() { Page=1,Limit=4,TenantId= HttpWx.TenantId};
            var newest = await _goodsService.GetByWherePageAsync(query, d => d.Status);
            query.Limit = 10;
            var best = await _goodsService.GetByWherePageAsync(query, d => d.Status);
          
            var items = await _advListService.GetListAsync(d=>d.Type==AdvEnum.MiniApp);

            return new ApiResult(new { newest, best, items });
        }
    }
}