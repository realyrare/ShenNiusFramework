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
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Tenant;
using ShenNius.Share.Domain.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Cms.API.Controllers
{
    public class SiteController : ApiBaseController<Site, DetailQuery, DeletesInput, KeyListQuery, SiteInput, SiteModifyInput>
    {
        private readonly IBaseServer<Site> _service;
        private readonly ICacheHelper _cacheHelper;
        public SiteController(IBaseServer<Site> service, IMapper mapper, ICacheHelper cacheHelper) : base(service, mapper)
        {
            _service = service;
            _cacheHelper = cacheHelper;
        }
        [HttpDelete]
        public override async Task<ApiResult> Deletes([FromBody] DeletesInput deletesInput)
        {
            foreach (var item in deletesInput.Ids)
            {
                await _service.UpdateAsync(d => new Site() { IsDel = false }, d => d.Id == item);
            }
            return new ApiResult();
        }
        /// <summary>
        /// 设置当前站点
        /// </summary>
        /// <param name="siteCurrentInput"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> SetCurrent([FromBody] SiteCurrentInput siteCurrentInput)
        {
            //把之前缓存存储的站点拿出来设置为不是当前的。
            var model = await _service.GetModelAsync(d => d.Id == siteCurrentInput.Id && d.IsDel == false && d.IsCurrent == false);
            if (model == null)
            {
                throw new FriendlyException("当前站点实体信息为空!");
            }
            var currentSite = _cacheHelper.Get<Site>(KeyHelper.Cms.CurrentSite);
            if (currentSite != null)
            {
                currentSite.IsCurrent = false;
                //不要使用全部更新  有可能缓存的实体比较旧
                await _service.UpdateAsync(d => new Site() { IsCurrent = false }, d => d.Id == currentSite.Id);
            }

            model.IsCurrent = true;
            await _service.UpdateAsync(model);
            //这里最好更新下缓存
            _cacheHelper.Set(KeyHelper.Cms.CurrentSite, model);
            return new ApiResult();
        }
        [HttpGet]
        public async Task<ApiResult> GetList()
        {
            var res = await _service.GetListAsync(d => d.IsDel == false);
            foreach (var item in res)
            {
                if (item.IsCurrent)
                {
                    _cacheHelper.Set(KeyHelper.Cms.CurrentSite, item);
                }
            }
            return new ApiResult(data: res);
        }
        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListQuery keyListQuery)
        {
            Expression<Func<Site, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(keyListQuery.Key))
            {
                whereExpression = d => d.Title.Contains(keyListQuery.Key);
            }
            var res = await _service.GetPagesAsync(keyListQuery.Page, keyListQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        //[HttpPut]
        //public override async Task<ApiResult> Modify([FromBody] SiteModifyInput siteModifyInput)
        //{
        //    var res = await _service.UpdateAsync(d => new Site()
        //    {
        //        Id = siteModifyInput.Id,
        //        IsCurrent = siteModifyInput.IsCurrent,
        //        Title = siteModifyInput.Title,
        //        Email = siteModifyInput.Email,
        //        WeiBo = siteModifyInput.WeiBo,
        //        WeiXin = siteModifyInput.WeiXin,
        //        UserId = siteModifyInput.UserId,
        //        Name = siteModifyInput.Name,
        //        Url = siteModifyInput.Url,
        //        Logo = siteModifyInput.Logo,
        //        Summary = siteModifyInput.Summary,
        //        Tel = siteModifyInput.Tel,
        //        Fax = siteModifyInput.Fax,
        //        QQ = siteModifyInput.QQ,
        //        Address = siteModifyInput.Address,
        //        Code = siteModifyInput.Code,
        //        Keyword = siteModifyInput.Keyword,
        //        Description = siteModifyInput.Description,
        //        Copyright = siteModifyInput.Copyright,
        //        Status = siteModifyInput.Status,
        //        CloseInfo = siteModifyInput.CloseInfo,
        //        ModifyTime = siteModifyInput.ModifyTime,

        //    }, d => d.Id == siteModifyInput.Id);
        //    return new ApiResult(data: res);
        //}
    }
}