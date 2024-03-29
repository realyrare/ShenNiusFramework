﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Enums.Cms;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

/*************************************
* 类名：IndexController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/2 17:11:25
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Blog.API.Controllers.Cms
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {

        private readonly IAdvListService _advlistService;
        private readonly IArticleService _articleService;
        private readonly IColumnService _columnService;
        protected ICacheHelper _cache;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;
        public IndexController(IAdvListService advlistService
            , IArticleService articleService
            , IColumnService columnService,
            ICacheHelper cache, ITenantService tenantService, IMessageService messageService, IMapper mapper)
        {
            _advlistService = advlistService;
            _articleService = articleService;
            _columnService = columnService;
            _messageService = messageService;
            _mapper = mapper;
            _tenantService = tenantService;
            _cache = cache;
        }

        private async Task<List<Column>> GetColumnAsync(int siteId)
        {
            return await _columnService.GetListAsync(d => d.Status == true && d.TenantId == siteId);
        }
        [HttpGet]
        public async Task<ApiResult> GetAllColumn(int siteId)
        {
            var data = await GetColumnAsync(siteId);
            return new ApiResult(data);
        }
        /// <summary>
        /// 请求站点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetSiteInfo(int siteId)
        {
            var model = _tenantService.GetModelAsync(d => d.Id.Equals(siteId));
            if (model == null)
            {
                throw new FriendlyException("请求的站点信息为空");
            }
            return new ApiResult(model);
        }
        [HttpGet]
        public async Task<ApiResult> GetRightData(string spell, int siteId)
        {
            var monthArticle = await _articleService.GetArtcileByConditionAsync((ca, cc) => ca.Audit == true && SqlFunc.DateIsSame(DateTime.Now, ca.CreateTime, DateType.Month), 1, 10);
            var currentSpellArticle = await _articleService.GetArtcileByConditionAsync((ca, cc) => ca.Audit == true && cc.EnTitle.Trim().Equals(spell), 1, 10);
            var allChildColumn = (await GetColumnAsync(siteId)).Where(d => d.ParentId != 0).ToList();
            return new ApiResult(data: new { monthArticle = monthArticle.Items, currentSpellArticle = currentSpellArticle.Items, allChildColumn });
        }
        [HttpGet]
        public async Task<ApiResult> GetIndex(int siteId)
        {
            var recArticleList = await _articleService.GetArtcileByConditionAsync((ca, cc) => ca.IsTop == true && ca.Audit == true && ca.TenantId == siteId, 1, 6);
            var categoryArticleList = await _articleService.GetArtcileByConditionAsync((ca, cc) => ca.Audit == true && ca.TenantId == siteId, 1, 12);
            return new ApiResult(data: new
            {
                recArticleList = recArticleList.Items,
                categoryArticleList = categoryArticleList.Items
            });
        }
        [HttpGet]
        public async Task<ApiResult> GetAllTags(string spell, int siteId)
        {
            // 进来可能是大类或子类，1、大类下面有子类，2、大类下面没有子类 3、进来的是子类
            if (string.IsNullOrEmpty(spell))
            {
                throw new FriendlyException("栏目url不能为空");
            }
            var model = (await GetColumnAsync(siteId)).Where(d => d.EnTitle.Equals(spell)).FirstOrDefault();
            List<int> columnListId = new List<int>();
            if (model.ParentId == 0)
            {
                //查下该大类有没有子类
                columnListId = (await GetColumnAsync(siteId)).Where(d => d.ParentId == model.Id).Select(d => d.Id).ToList();
                //没有子类直接赋值大类id
                if (columnListId.Count == 0)
                {
                    columnListId.Add(model.Id);
                }
            }
            else
            {
                //进来了子类
                columnListId.Add(model.Id);
            }
            var list = await _articleService.GetAllTagsAsync(columnListId);
            return new ApiResult(list);
        }
        /// <summary>
        /// 根据ID查询案例/新闻详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetDetail(int id, int siteId, string parentColumnSpell = null, string childColumnspell = null)
        {

            List<Column> columnList = await GetColumnAsync(siteId);

            if (string.IsNullOrEmpty(parentColumnSpell) && string.IsNullOrEmpty(childColumnspell))
            {
                throw new FriendlyException("栏目不能为空");
            }
            string columnUrl = string.IsNullOrEmpty(childColumnspell) ? parentColumnSpell : childColumnspell;
            var model = await _articleService.GetArtcileDetailAsync((ca, cc) => ca.Id == id && ca.Audit == true && cc.EnTitle.Trim() == columnUrl.Trim());
            //var model=  await _cache.GetOrSetAsync($"article:{id}", 
            //async ()=> {
            //    return await _articleService.GetArtcileDetailAsync((ca, cc) => ca.Id == id && ca.Audit == true && cc.EnTitle.Trim() == columnUrl.Trim());
            //    },null);
            if (model == null)
            {
                throw new FriendlyException("文章详情没有查到数据");
            }
            var upArticle = await _articleService.GetNextOrUpArticleAsync((ca, cc) => ca.Id > id && ca.ColumnId == model.ColumnId && ca.TenantId == siteId);
            var nextArticle = await _articleService.GetNextOrUpArticleAsync((ca, cc) => ca.Id < id && ca.ColumnId == model.ColumnId && ca.TenantId == siteId);
            var sameColumnArticle = await _articleService.GetArtcileByConditionAsync((ca, cc) => ca.ColumnId == model.ColumnId && ca.Audit == true && ca.TenantId == siteId, 1, 6);

            return new ApiResult(new { articleDetailModel = model, upArticle, nextArticle, sameColumnArticle = sameColumnArticle.Items });
        }
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="parentColumnSpell"></param>
        /// <param name="childColumnSpell"></param>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetList(int siteId, string parentColumnSpell, string childColumnSpell, string keyword, int page = 1)
        {
            Column columnModel = null;
            List<int> allChildColumnIdList = new List<int>();
            var columnList = await GetColumnAsync(siteId);
            //keyword
            Expression<Func<Article, Column, bool>> expression = (ca, cc) => ca.TenantId == siteId;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = HttpUtility.UrlDecode(keyword);
                expression = (ca, cc) => ca.KeyWord.Contains(keyword) || ca.Tag.Contains(keyword);
            }
            //先判断大类url是否为空
            if (!string.IsNullOrEmpty(parentColumnSpell))
            {
                var parentColumnModel = columnList.Where(d => d.ParentId == 0 && d.EnTitle.Equals(parentColumnSpell.Trim())).FirstOrDefault();
                if (parentColumnModel != null)
                {
                    columnModel = parentColumnModel;
                    allChildColumnIdList = columnList.Where(d => d.ParentId == parentColumnModel.Id && d.ParentId != 0).Select(d => d.Id).ToList();
                    //如果这个大栏目没有子栏目时就根据大栏目去查找对应的文章
                    if (allChildColumnIdList.Count == 0)
                    {
                        expression = (ca, cc) => ca.ColumnId == columnModel.Id;
                    }
                }
            }
            //判断allChildColumnIdList是否有值
            if (allChildColumnIdList.Count > 0 && allChildColumnIdList.Any())
            {
                expression = (ca, cc) => allChildColumnIdList.Contains(ca.ColumnId);
            }
            //判断子类url是否为空
            if (!string.IsNullOrEmpty(childColumnSpell))
            {
                var childColumnModel = columnList.Where(d => d.ParentId > 0 && d.EnTitle.Equals(childColumnSpell.Trim())).FirstOrDefault();
                if (childColumnModel != null)
                {
                    columnModel = childColumnModel;
                    expression = (ca, cc) => ca.ColumnId == childColumnModel.Id;
                }
            }

            var query = await _articleService.GetArtcileByConditionAsync(expression, page, 15);

            return new ApiResult(new
            {
                ArticleList = query.Items,
                total = query.TotalItems,
                Title = columnModel?.Title,
                Keywords = columnModel?.Keyword,
                Description = columnModel?.Summary
            });
        }
        /// <summary>
        ///  用户留言，提交需求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> AddMsg([FromBody] MessageInput messageInput)
        {
            messageInput.IP = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();

            var list = await _messageService.GetListAsync(m => m.IP == messageInput.IP && m.TenantId == messageInput.TenantId && m.Types == messageInput.Types, m => m.CreateTime, true);
            if (list.Count() > 3)
            {
                throw new FriendlyException("您提交的次数过多，请稍后重试！~");
            }
            messageInput.Address = IpParseHelper.GetAddressByIP(messageInput.IP);
            var model = _mapper.Map<Message>(messageInput);
            await _messageService.AddAsync(model);
            return new ApiResult();
        }
        [HttpGet]
        public async Task<ApiResult> LoadMessage(int businessId, int siteId, string types)
        {
            var result = await _messageService.GetListAsync(x => x.BusinessId == businessId && x.TenantId.Equals(siteId) && x.Types.Equals(types), x => x.CreateTime, false);
            return new ApiResult(result);
        }
        [HttpGet]
        public async Task<ApiResult> GetAdvList(int siteId, AdvEnum type)
        {
            var advList = await _advlistService.GetListAsync(x => x.Status == true && x.TenantId == siteId && x.Type == type, x => x.Sort, false);
            return new ApiResult(advList);
        }
    }
}