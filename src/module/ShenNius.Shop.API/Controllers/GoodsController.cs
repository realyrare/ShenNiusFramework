using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Infrastructure.FileManager;
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
        private readonly IUploadHelper _uploadHelper;

        public GoodsController(IBaseServer<Goods> service, IMapper mapper, IGoodsService  goodsService, IUploadHelper uploadHelper) : base(service, mapper)
        {
            _goodsService = goodsService;
            _uploadHelper = uploadHelper;
        }

        [HttpGet]
        public override  Task<ApiResult> GetListPages(KeyListTenantQuery query)
        {
            return  _goodsService.GetListPageAsync(query);
        }
        [HttpPost]
        public override Task<ApiResult> Add([FromBody] GoodsInput input)
        {
            return _goodsService.AddAsync(input);
        }
        [HttpPut]
        public override Task<ApiResult> Modify([FromBody] GoodsModifyInput input)
        {
            return _goodsService.ModifyAsync(input);
        }
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
        [HttpPost, AllowAnonymous]
        public IActionResult UploadImg()
        {
            var files = Request.Form.Files[0];
            var result = _uploadHelper.Upload(files, "goods/");
            //TinyMCE 指定的返回格式
            return Ok(new { location = result.Data });
        }
        [HttpPost]
        public ApiResult MultipleUploadImg()
        {
            var files = Request.Form.Files;
            var data = _uploadHelper.Upload(files, "goods/");
            //TinyMCE 指定的返回格式
            return data;
        }
    }
}
