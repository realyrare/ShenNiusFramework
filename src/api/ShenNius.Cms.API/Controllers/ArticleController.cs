using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.ImgUpload;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：ArticleController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:23:48
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{
    public class ArticleController : ApiControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly QiniuCloud _qiniuCloud;
        private readonly QiNiuOssModel _qiNiuOssModel;

        public ArticleController(IArticleService articleService, IMapper mapper, IMemoryCache cache, IOptionsMonitor<QiNiuOssModel> qiNiuOssModel, QiniuCloud qiniuCloud)
        {
            _articleService = articleService;
            this._mapper = mapper;
            this._cache = cache;
            _qiniuCloud = qiniuCloud;
            _qiNiuOssModel = qiNiuOssModel.CurrentValue;
        }
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult(await _articleService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<Article, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Title.Contains(key);
            }
            var res = await _articleService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _articleService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpPost]
        public async Task<ApiResult> Add([FromBody] ArticleInput articleInput)
        {
            articleInput.SiteId = _cache.Get<Site>(KeyHelper.Cms.CurrentSite).Id;
            var model= _mapper.Map<Article>(articleInput);
            var i= await _articleService.AddAsync(model);
            return new ApiResult(i);
        }

        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] ArticleModifyInput articleModifyInput)
        {
            articleModifyInput.SiteId = _cache.Get<Site>(KeyHelper.Cms.CurrentSite).Id;
            var model = _mapper.Map<Article>(articleModifyInput);
            var i= await _articleService.UpdateAsync(model,d=>new  { d.CreateTime});
            return new ApiResult(i);
        }
        [HttpPost, AllowAnonymous]
        public IActionResult QiniuFile()
        {
            var files = Request.Form.Files[0];                     
            var data = _qiniuCloud.UploadFile(files, "article/");            
            var url = _qiNiuOssModel.ImgDomain + data;
            //TinyMCE 指定的返回格式
            return Ok(new { location= url });
        }
    }
}