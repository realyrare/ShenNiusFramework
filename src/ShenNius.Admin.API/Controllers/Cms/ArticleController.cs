using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
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

namespace ShenNius.Admin.API.Controllers.Cms
{
    public class ArticleController : ApiTenantBaseController<Article, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, ArticleInput, ArticleModifyInput>
    {

        private readonly IUploadHelper _uploadHelper;
        private readonly IArticleService _articleService;

        public ArticleController(IBaseServer<Article> service, IMapper mapper, IUploadHelper uploadHelper, IArticleService articleService) : base(service, mapper)
        {
            _uploadHelper = uploadHelper;
            _articleService = articleService;
        }

        [HttpGet]
        public override Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            return _articleService.GetPagesAsync(query);
        }
        [HttpPost, AllowAnonymous]
        public IActionResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _uploadHelper.Upload(files, "article/");
            //TinyMCE 指定的返回格式
            return Ok(new { location = data });
        }
    }
}