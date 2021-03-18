using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.ImgUpload;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using System;
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

        public ArticleController(IArticleService articleService, IMapper mapper, IMemoryCache cache)
        {
            _articleService = articleService;
            this._mapper = mapper;
            this._cache = cache;
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
        [HttpPost]
        public ApiResult QiniuFile([FromBody] string filePath)
        {
            var i = QiniuCloud.UploadFile(filePath);
            return new ApiResult(i);
        }
    }
}