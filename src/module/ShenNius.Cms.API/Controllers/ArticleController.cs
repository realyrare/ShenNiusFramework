using AutoMapper;
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
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Domain.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：ArticleController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:23:48
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{
    public class ArticleController : ApiTenantBaseController<Article, DetailSiteQuery, DeletesSiteInput, KeyListSiteQuery, ArticleInput, ArticleModifyInput>
    {
        private readonly IBaseServer<Article> _service;
        private readonly QiniuCloud _qiniuCloud;
        private readonly QiNiuOssModel _qiNiuOssModel;

        public ArticleController(IBaseServer<Article> service, IMapper mapper, IOptionsMonitor<QiNiuOssModel> qiNiuOssModel, QiniuCloud qiniuCloud) : base(service, mapper)
        {
            _service = service;
            _qiniuCloud = qiniuCloud;
            _qiNiuOssModel = qiNiuOssModel.CurrentValue;
        }

        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListSiteQuery keywordListSiteQuery)
        {
            Expression<Func<Article, bool>> whereExpression = d=>d.Status==true;
            if (keywordListSiteQuery.TenantId > 0)
            {
                whereExpression = d => d.TenantId == keywordListSiteQuery.TenantId;
            }
            if (!string.IsNullOrEmpty(keywordListSiteQuery.Key))
            {
                whereExpression = d => d.Title.Contains(keywordListSiteQuery.Key);
            }
            var res = await _service.GetPagesAsync(keywordListSiteQuery.Page, keywordListSiteQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpPost, AllowAnonymous]
        public IActionResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _qiniuCloud.UploadFile(files, "article/");
            var url = _qiNiuOssModel.ImgDomain + data;
            //TinyMCE 指定的返回格式
            return Ok(new { location = url });
        }
    }
}