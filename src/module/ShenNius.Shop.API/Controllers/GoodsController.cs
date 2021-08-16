using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Shop;
using System.Threading.Tasks;

namespace ShenNius.Shop.API.Controllers
{
    /// <summary>
    /// 商品分类控制器
    /// </summary>
    public class GoodsController : ApiTenantBaseController<Goods, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, GoodsInput, GoodsModifyInput>
    {
        private readonly IGoodsService _goodsService;
        public GoodsController(IBaseServer<Goods> service, IMapper mapper, IGoodsService  goodsService) : base(service, mapper)
        {
            _goodsService = goodsService;
        }


        //[HttpGet]
        //public override Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        //{

        //}
        [HttpPost]
        public override Task<ApiResult> Add([FromBody] GoodsInput input)
        {
            return _goodsService.AddAsync(input);
        }
        //[HttpPut]
        //public override Task<ApiResult> Modify([FromBody] CategoryModifyInput input)
        //{

        //}
        [HttpPost]
        public Task<ApiResult> AddSpec([FromForm] SpecInput input)
        {
         return  _goodsService.AddSpecAsync(input);
        }
        [HttpPost]
        public Task<ApiResult> AddSpecValue([FromForm] SpecValuesInput input)
        {
            return _goodsService.AddSpecAsync(input);
        }
    }
}
