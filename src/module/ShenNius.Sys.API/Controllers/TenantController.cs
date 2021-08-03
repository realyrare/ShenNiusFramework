/*************************************
* 类名：TenantController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:22:57
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Domain.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.ImgUpload;
using Microsoft.Extensions.Options;

namespace ShenNius.Sys.API.Controllers
{
    public class TenantController : ApiBaseController<Tenant, DetailQuery, DeletesInput, KeyListQuery, TenantInput, TenantModifyInput>
    {
        private readonly IBaseServer<Tenant> _service;
        private readonly ICacheHelper _cacheHelper;
        private readonly QiNiuOssModel _qiNiuOssModel;
        private readonly QiniuCloud _qiniuCloud;
        public TenantController(IBaseServer<Tenant> service, IMapper mapper, ICacheHelper cacheHelper, IOptionsMonitor<QiNiuOssModel> qiNiuOssModel, QiniuCloud qiniuCloud) : base(service, mapper)
        {
            _service = service;
            _cacheHelper = cacheHelper;
            this._qiNiuOssModel = qiNiuOssModel.CurrentValue;
            this._qiniuCloud = qiniuCloud;
        }
        [HttpGet]
        public override async Task<ApiResult> Detail([FromQuery] DetailQuery detailQuery)
        {
            var res = await _service.GetModelAsync(d => d.Id == detailQuery.Id && d.IsDel == false);
            return new ApiResult(data: res);
        }
        [HttpDelete]
        public override async Task<ApiResult> Deletes([FromBody] DeletesInput deletesInput)
        {
            foreach (var item in deletesInput.Ids)
            {
                await _service.UpdateAsync(d => new Tenant() { IsDel = false }, d => d.Id == item);
            }
            return new ApiResult();
        }
        /// <summary>
        /// 设置当前站点
        /// </summary>
        /// <param name="TenantCurrentInput"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> SetCurrent([FromBody] TenantCurrentInput TenantCurrentInput)
        {
            //把之前缓存存储的站点拿出来设置为不是当前的。
            var model = await _service.GetModelAsync(d => d.Id == TenantCurrentInput.Id && d.IsDel == false && d.IsCurrent == false);
            if (model == null)
            {
                throw new FriendlyException("当前站点实体信息为空!");
            }
            var currentTenant = _cacheHelper.Get<Tenant>(KeyHelper.Cms.CurrentTenant);
            if (currentTenant != null)
            {
                currentTenant.IsCurrent = false;
                //不要使用全部更新  有可能缓存的实体比较旧
                await _service.UpdateAsync(d => new Tenant() { IsCurrent = false }, d => d.Id == currentTenant.Id);
            }

            model.IsCurrent = true;
            await _service.UpdateAsync(model);
            //这里最好更新下缓存
            _cacheHelper.Set(KeyHelper.Cms.CurrentTenant, model);
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
                    _cacheHelper.Set(KeyHelper.Cms.CurrentTenant, item);
                }
            }
            return new ApiResult(data: res);
        }
        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListQuery keyListQuery)
        {
            Expression<Func<Tenant, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(keyListQuery.Key))
            {
                whereExpression = d => d.Title.Contains(keyListQuery.Key);
            }
            var res = await _service.GetPagesAsync(keyListQuery.Page, keyListQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
       
        [HttpPost, AllowAnonymous]
        public ApiResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _qiniuCloud.UploadFile(files, "tenant/");
            var url = _qiNiuOssModel.ImgDomain + data;
            return new ApiResult(data: url);
        }
    }
}