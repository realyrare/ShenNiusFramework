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
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Dtos.Output.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Cms.API.Controllers
{
    public class SiteController : ApiControllerBase
    {
        private readonly ISiteService _siteService;
        private readonly IMapper _mapper;

        public SiteController(ISiteService siteService, IMapper mapper)
        {
            _siteService = siteService;
            this._mapper = mapper;
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
       
        public async Task<ApiResult> SetCurrent([FromBody] int id)
        {
            
            await _siteService.UpdateAsync(d => new Site() { IsCurrent = true }, d => d.Id == id);            
            return new ApiResult();
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
            var model = _mapper.Map<Site>(siteInput);
            var res = await _siteService.AddAsync(model);
            return new ApiResult(data: res);
        }
        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] SiteModifyInput siteModifyInput)
        {
            var model = _mapper.Map<Site>(siteModifyInput);
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