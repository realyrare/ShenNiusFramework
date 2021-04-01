using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using ShenNius.Share.Service.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：ColumnController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:24:30
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{
    public class ColumnController : ApiTenantBaseController<Column, DetailSiteQuery, DeletesSiteInput, KeyListSiteQuery, ColumnInput, ColumnModifyInput>
    {
        private readonly IColumnService _columnService;
        public ColumnController(IBaseServer<Column> service, IMapper mapper, IColumnService columnService) : base(service, mapper)
        {
            _columnService = columnService;
        }

        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListSiteQuery keywordListSiteQuery)
        {
            Expression<Func<Column, bool>> whereExpression = null;
            if (keywordListSiteQuery.SiteId > 0)
            {
                whereExpression = d => d.SiteId == keywordListSiteQuery.SiteId;
            }
            if (!string.IsNullOrEmpty(keywordListSiteQuery.Key))
            {
                whereExpression = d => d.Title.Contains(keywordListSiteQuery.Key);
            }
            var res = await _columnService.GetPagesAsync(keywordListSiteQuery.Page, keywordListSiteQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        /// <summary>
        /// 所有父栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAllParentColumn()
        {
            var data = await _columnService.GetListAsync(d => d.ParentId == 0);
            return new ApiResult(data);
        }
    }
}