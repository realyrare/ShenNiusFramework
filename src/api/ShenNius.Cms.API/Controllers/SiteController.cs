/*************************************
* 类名：SiteController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:22:57
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Dtos.Output.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using ShenNius.Share.Service.Sys;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Cms.API.Controllers
{
    public class SiteController : ApiControllerBase
    {
        private readonly ISiteService _siteService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMemoryCache _cache;
        public SiteController(ISiteService siteService, IMapper mapper, ICurrentUserContext currentUserContext, IMemoryCache memoryCache)
        {
            _siteService = siteService;
            this._mapper = mapper;
            this._currentUserContext = currentUserContext;
            _cache = memoryCache;
        }

        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            foreach (var item in commonDeleteInput.Ids)
            {
                await _siteService.UpdateAsync(d => new Site() { IsDel = false}, d => d.Id == item);
            }
            return new ApiResult();
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="siteCurrentInput"></param>
        /// <returns></returns>
        [HttpPut]
       
        public async Task<ApiResult> SetCurrent([FromBody] SiteCurrentInput siteCurrentInput)
        {

            //把之前缓存存储的站点拿出来设置为不是当前的。
            var model = await _siteService.GetModelAsync(d => d.Id == siteCurrentInput.Id&&d.IsDel==false&&d.IsCurrent==false);
            if (model == null)
            {
                throw new FriendlyException("当前站点实体信息为空!");
            }
            var currentSite= _cache.Get<Site>(KeyHelper.Cms.CurrentSite);
            if (currentSite != null)
            {
                currentSite.IsCurrent = false;
                //不要使用全部更新  有可能缓存的实体比较旧
                await _siteService.UpdateAsync(d=>new Site() { IsCurrent=false},d=>d.Id== currentSite.Id);
            }
        
            model.IsCurrent = true;
            await _siteService.UpdateAsync(model);
            //这里最好更新下缓存
            _cache.Set(KeyHelper.Cms.CurrentSite, model);
            return new ApiResult();
        }
        [HttpGet]
        public async Task<ApiResult> GetList()
        {
            
            var res = await _siteService.GetListAsync(d=>d.IsDel==false);
            foreach (var item in res)
            {
                if (item.IsCurrent)
                {
                    _cache.Set(KeyHelper.Cms.CurrentSite, item);
                }  
            }
            return new ApiResult(data: res);
        }
        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<Site, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Title.Contains(key);
            }
            var res = await _siteService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _siteService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }

        [HttpPost]
        public async Task<ApiResult> Add([FromBody] SiteInput siteInput)
        {
            siteInput.UserId = _currentUserContext.Id;
            var model = _mapper.Map<Site>(siteInput);
            var res = await _siteService.AddAsync(model);
            return new ApiResult(data: res);
        }
        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] SiteModifyInput siteModifyInput)
        {
           
            var res = await _siteService.UpdateAsync(d => new Site()
            {
                Id = siteModifyInput.Id,
                IsCurrent = siteModifyInput.IsCurrent,
                Title = siteModifyInput.Title,
                Email = siteModifyInput.Email,
                WeiBo = siteModifyInput.WeiBo,
                WeiXin = siteModifyInput.WeiXin,
                UserId = siteModifyInput.UserId,
                Name = siteModifyInput.Name,
                Url = siteModifyInput.Url,
                Logo = siteModifyInput.Logo,
                Summary = siteModifyInput.Summary,
                Tel = siteModifyInput.Tel,
                Fax = siteModifyInput.Fax,
                QQ = siteModifyInput.QQ,
                Address = siteModifyInput.Address,
                Code = siteModifyInput.Code,
                Keyword = siteModifyInput.Keyword,
                Description = siteModifyInput.Description,
                Copyright = siteModifyInput.Copyright,
                Status = siteModifyInput.Status,
                CloseInfo = siteModifyInput.CloseInfo,
                ModifyTime = siteModifyInput.ModifyTime,

            }, d => d.Id == siteModifyInput.Id);
            return new ApiResult(data: res);
        }
    }
}