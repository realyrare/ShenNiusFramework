using AutoMapper;
using FytSoa.Core.Model.Cms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

/*************************************
* 类 名： AdvListController
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 18:09:15
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{

    public class AdvListController:ApiControllerBase

    {
        private readonly IAdvListService _advListService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public AdvListController(IAdvListService advListService, IMapper mapper, IMemoryCache cache)
        {
            _advListService = advListService;
            this._mapper = mapper;
            this._cache = cache;
        }
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult(await _advListService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<AdvList, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Title.Contains(key);
            }
            var res = await _advListService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _advListService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpPost]
        public async Task<ApiResult> Add([FromBody] AdvListInput advListInput)
        {
            advListInput.SiteId = _cache.Get<Site>(KeyHelper.Cms.CurrentSite).Id;
            var model = _mapper.Map<AdvList>(advListInput);
            var i = await _advListService.AddAsync(model);
            return new ApiResult(i);
        }
        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] AdvListModifyInput advListModifyInput)
        {
            advListModifyInput.SiteId = _cache.Get<Site>(KeyHelper.Cms.CurrentSite).Id;
            var model = _mapper.Map<AdvList>(advListModifyInput);
            var i = await _advListService.UpdateAsync(model, d => new { d.CreateTime });
            return new ApiResult(i);
        }
    }
}