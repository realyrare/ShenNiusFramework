using AutoMapper;
using FytSoa.Core.Model.Cms;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Service.Cms;
using ShenNius.Share.Service.Repository;
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

        public AdvListController(IBaseServer<AdvList> service, IMapper mapper) : base(service, mapper)
        {
            _service = service;
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
    }
}