using AutoMapper;
using FytSoa.Core.Model.Cms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.ImgUpload;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Domain.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

    public class AdvListController : ApiTenantBaseController<AdvList, DetailSiteQuery, DeletesSiteInput, KeyListSiteQuery, AdvListInput, AdvListModifyInput>
    {
        private readonly IBaseServer<AdvList> _service;
        private readonly QiNiuOssModel _qiNiuOssModel;
        private readonly QiniuCloud _qiniuCloud;

        public AdvListController(IBaseServer<AdvList> service, IMapper mapper, IOptionsMonitor<QiNiuOssModel> qiNiuOssModel, QiniuCloud qiniuCloud) : base(service, mapper)
        {
            _service = service;
            this._qiNiuOssModel = qiNiuOssModel.CurrentValue;
            this._qiniuCloud = qiniuCloud;
        }

        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListSiteQuery keywordListSiteQuery)
        {
            Expression<Func<AdvList, bool>> whereExpression = null;
            if (keywordListSiteQuery.SiteId > 0)
            {
                whereExpression = d => d.SiteId == keywordListSiteQuery.SiteId;
            }
            if (!string.IsNullOrEmpty(keywordListSiteQuery.Key))
            {
                whereExpression = d => d.Title.Contains(keywordListSiteQuery.Key);
            }
            var res = await _service.GetPagesAsync(keywordListSiteQuery.Page, keywordListSiteQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpPost, AllowAnonymous]
        public ApiResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _qiniuCloud.UploadFile(files, "advList/");
            var url = _qiNiuOssModel.ImgDomain + data;
            //TinyMCE 指定的返回格式
            return new ApiResult(data: url);
        }
    }
}