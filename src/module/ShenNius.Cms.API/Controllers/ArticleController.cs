using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Domain.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ShenNius.Share.Models.Configs;

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
    public class ArticleController : ApiTenantBaseController<Article, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, ArticleInput, ArticleModifyInput>
    {
        private readonly IBaseServer<Article> _service;
        private readonly IUploadHelper _uploadHelper;
        private readonly QiNiuOss _qiNiuOssModel;

        public ArticleController(IBaseServer<Article> service, IMapper mapper, IOptionsMonitor<QiNiuOss> qiNiuOssModel, IUploadHelper uploadHelper) : base(service, mapper)
        {
            _service = service;
            _uploadHelper = uploadHelper;
            _qiNiuOssModel = qiNiuOssModel.CurrentValue;
        }

        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        {
            Expression<Func<Article, bool>> whereExpression = d=>d.Status==true;
            if (keywordListTenantQuery.TenantId > 0)
            {
                whereExpression = d => d.TenantId == keywordListTenantQuery.TenantId;
            }
            if (!string.IsNullOrEmpty(keywordListTenantQuery.Key))
            {
                whereExpression = d => d.Title.Contains(keywordListTenantQuery.Key);
            }
            var res = await _service.GetPagesAsync(keywordListTenantQuery.Page, keywordListTenantQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpPost, AllowAnonymous]
        public IActionResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _uploadHelper.Upload(files, "article/");
            var url = _qiNiuOssModel.ImgDomain + data;
            //TinyMCE 指定的返回格式
            return Ok(new { location = url });
        }
    }
}