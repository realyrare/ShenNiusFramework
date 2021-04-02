using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/*************************************
* 类名：IndexController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/2 17:11:25
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Blog.API.Controllers.Cms
{
    public class IndexController : ControllerBase
    {

        private readonly IAdvListService _advlistService;
        private readonly IArticleService _articleService;
        private readonly IColumnService _columnService;
        protected ICacheHelper _cache;
        // private readonly ICmsMessageService _messageService;
        private readonly ISiteService _cmsSiteService;
        public IndexController(IAdvListService advlistService
            , IArticleService articleService
            , IColumnService columnService,
            ICacheHelper cache, ISiteService cmsSiteService)
        {
            _advlistService = advlistService;
            _articleService = articleService;
            _columnService = columnService;
            // _messageService = messageService;
            _cmsSiteService = cmsSiteService;
            _cache = cache;
        }
        [HttpGet]
        private async Task<List<Column>> GetColumn()
        {
           return await _columnService.GetListAsync();
        }
        [HttpGet("all-column")]
        public ApiResult GetAllColumn()
        {
            return new ApiResult(GetColumn());
        }
        /// <summary>
        /// 请求站点信息
        /// </summary>
        /// <param name="globalSiteGuid">站点id</param>
        /// <returns></returns>
        [HttpGet("getsiteinfo")]
        public ApiResult GetSiteInfo(string globalSiteId)
        {
            var model = _cmsSiteService.GetModelAsync(d => d.Id.Equals(globalSiteId));
            if (model == null)
            {
                throw new FriendlyException("请求的站点信息为空");
            }
            return new ApiResult(model);
        }
        [HttpGet("right")]
        public async Task<IActionResult> GetRightData(string spell)
        {
            var monthArticle = await _articleService.GetArtcileByCondition((ca, cc) => ca.Audit == true && SqlFunc.DateIsSame(DateTime.Now, ca.CreateTime, DateType.Month), 1, 10);
            var currentSpellArticle = await _articleService.GetArtcileByCondition((ca, cc) => ca.Audit == true && cc.EnTitle.Trim().Equals(spell), 1, 10);
            var allChildColumn=(await GetColumn()).Where(d => d.ParentId != 0).ToList();
            return Ok(new { monthArticle = monthArticle.Items, currentSpellArticle = currentSpellArticle.Items, allChildColumn = allChildColumn });
        }
        //[HttpGet("index")]
        //public async Task<IActionResult> GetIndex()
        //{
        //    var recArticleList = await _articleService.GetArtcileByCondition((ca, cc) => ca.IsTop == true && ca.Audit == true, 1, 6);
        //    var categoryArticleList = await _articleService.GetArtcileByCondition((ca, cc) => ca.Audit == true, 1, 12);
        //    return Ok(new
        //    {
        //        recArticleList = recArticleList.Items,
        //        categoryArticleList = categoryArticleList.Items
        //    });
        //}
        //[HttpGet("getalltags")]
        //public async Task<IActionResult> GetAllTags(string spell)
        //{
        //    // 进来可能是大类或子类，1、大类下面有子类，2、大类下面没有子类 3、进来的是子类
        //    var model = GetColumn().Where(d => d.EnTitle.Equals(spell)).FirstOrDefault();
        //    List<int> columnListId = new List<int>();
        //    if (model.ParentId == 0)
        //    {
        //        //查下该大类有没有子类
        //        columnListId = GetColumn().Where(d => d.ParentId == model.Id).Select(d => d.Id).ToList();
        //        //没有子类直接赋值大类id
        //        if (columnListId.Count == 0)
        //        {
        //            columnListId.Add(model.Id);
        //        }
        //    }
        //    else
        //    {
        //        //进来了子类
        //        columnListId.Add(model.Id);
        //    }
        //    var list = await _articleService.GetAllTags(columnListId);
        //    return Json(list);
        //}
        ///// <summary>
        ///// 根据ID查询案例/新闻详情
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("detail")]
        //public async Task<IActionResult> GetDetail(int id = 0, string parentColumnSpell = null, string childColumnspell = null)
        //{

        //    List<CmsColumn> columnList = GetColumn();

        //    if (string.IsNullOrEmpty(parentColumnSpell) && string.IsNullOrEmpty(childColumnspell))
        //    {
        //        return Json(new ApiError("栏目不能为空"));
        //    }
        //    string columnUrl = string.IsNullOrEmpty(childColumnspell) ? parentColumnSpell : childColumnspell;

        //    var model = await _cache.GetOrCreateAsync($"article:{id}", async x => await _articleService.GetArtcileDetailAsync((ca, cc) => ca.Id == id && ca.Audit == true && cc.EnTitle.Trim() == columnUrl.Trim()));
        //    if (model == null)
        //    {
        //        return Json(new ApiError("没有数据"));
        //    }
        //    CmsArtcileDto upArticle = await _articleService.GetUpArticleAsync(id, model.ColumnId);
        //    var nextArticle = await _articleService.GetNextArticleAsync(id, model.ColumnId);
        //    var sameColumnArticle = await _articleService.GetArtcileByCondition((ca, cc) => ca.ColumnId == model.ColumnId && ca.Audit == true, 1, 6);
        //    #region 增加点击量
        //    await _articleService.UpdateArticleViews(model);
        //    #endregion
        //    return Ok(new { articleDetailModel = model, upArticle, nextArticle, sameColumnArticle = sameColumnArticle.Items });
        //}
        ///// <summary>
        ///// 文章列表
        ///// </summary>
        ///// <param name="parentColumnSpell"></param>
        ///// <param name="childColumnSpell"></param>
        ///// <param name="keyword"></param>
        ///// <param name="page"></param>
        ///// <returns></returns>
        //[HttpGet("list")]
        //public async Task<IActionResult> GetList(string parentColumnSpell, string childColumnSpell, string keyword, int page = 1)
        //{
        //    Page<CmsArtcileDto> query = null; CmsColumn columnModel = null;
        //    List<int> allChildColumnIdList = new List<int>();
        //    var pageParam = new PageParm() { limit = 15, page = page, types = 1 };
        //    var columnList = GetColumn();
        //    //keyword
        //    Expression<Func<CmsArticle, CmsColumn, bool>> expression = null;
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        keyword = HttpUtility.UrlDecode(keyword);
        //        expression = (ca, cc) => ca.KeyWord.Contains(keyword) || ca.Tag.Contains(keyword);
        //    }
        //    //先判断大类url是否为空
        //    if (!string.IsNullOrEmpty(parentColumnSpell))
        //    {
        //        var parentColumnModel = columnList.Where(d => d.ParentId == 0 && d.EnTitle.Equals(parentColumnSpell.Trim())).FirstOrDefault();
        //        if (parentColumnModel != null)
        //        {
        //            columnModel = parentColumnModel;
        //            allChildColumnIdList = columnList.Where(d => d.ParentId == parentColumnModel.Id && d.ParentId != 0).Select(d => d.Id).ToList();
        //            //如果这个大栏目没有子栏目时就根据大栏目去查找对应的文章
        //            if (allChildColumnIdList.Count == 0)
        //            {
        //                expression = (ca, cc) => ca.ColumnId == columnModel.Id;
        //            }
        //        }
        //    }
        //    //判断allChildColumnIdList是否有值
        //    if (allChildColumnIdList.Count > 0 && allChildColumnIdList.Any())
        //    {
        //        expression = (ca, cc) => allChildColumnIdList.Contains(ca.ColumnId);
        //    }
        //    //判断子类url是否为空
        //    if (!string.IsNullOrEmpty(childColumnSpell))
        //    {
        //        var childColumnModel = columnList.Where(d => d.ParentId > 0 && d.EnTitle.Equals(childColumnSpell.Trim())).FirstOrDefault();
        //        if (childColumnModel != null)
        //        {
        //            columnModel = childColumnModel;
        //            expression = (ca, cc) => ca.ColumnId == childColumnModel.Id;
        //        }
        //    }
        //    if (expression == null)
        //    {
        //        //可以设置为推荐的文章
        //        query = await _articleService.GetArtcileByCondition(expression, pageParam.page, pageParam.limit);
        //    }
        //    else
        //    {
        //        query = await _articleService.GetArtcileByCondition(expression, pageParam.page, pageParam.limit);
        //    }
        //    return Json(new
        //    {
        //        ArticleList = query.Items,
        //        total = query.TotalItems,
        //        Title = columnModel?.Title,
        //        Keywords = columnModel?.KeyWord,
        //        Description = columnModel?.Summary
        //    });
        //}
        ///// <summary>
        /////  用户留言，提交需求
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost("addmsg")]
        //public IActionResult OnGetMessage(CmsMessage model)
        //{
        //    var apiRes = new ApiResult<string>() { StatusCode = (int)ApiEnum.Status };
        //    try
        //    {
        //        model.IP = Utils.GetIp();
        //        var list = _messageService.GetListAsync(m => m.IP == model.IP, m => m.AddDate, DbOrderEnum.Asc).Result.Data;
        //        if (list.Count > 3)
        //        {
        //            return new JsonResult(new ApiResult<string>() { StatusCode = (int)ApiEnum.HttpRequestError, Message = "您提交的次数过多，请稍后重试！~" });
        //        }
        //        model.UserName = model.Email;
        //        model.AddDate = DateTime.Now;
        //        _messageService.AddAsync(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        apiRes.Message = ex.Message;
        //    }
        //    return new JsonResult(new ApiResult<string>() { StatusCode = apiRes.StatusCode, Message = apiRes.Message });
        //}
        //[HttpGet("load-message")]
        //public async Task<IActionResult> LoadMessage(int articleId, string siteGuid)
        //{
        //    var result = await _messageService.GetListAsync(x => x.ColumnId == articleId && x.SiteGuid.Equals(siteGuid), x => x.AddDate, DbOrderEnum.Desc);
        //    return Ok(result.Data);
        //}
        //[HttpGet("getadvlist")]
        //public async Task<IActionResult> GetAdvList()
        //{
        //    var advList = await _advlistService.GetListAsync(x => x.Status == true, x => x.Sort, DbOrderEnum.Desc);
        //    return Ok(advList.Data);
        //}
    }
}