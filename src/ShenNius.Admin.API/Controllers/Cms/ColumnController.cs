﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using System.Threading.Tasks;

/*************************************
* 类名：ColumnController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:24:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Admin.API.Controllers.Cms
{
    public class ColumnController : ApiTenantBaseController<Column, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, ColumnInput, ColumnModifyInput>
    {
        private readonly IColumnService _columnService;
        public ColumnController(IBaseServer<Column> service, IMapper mapper, IColumnService columnService) : base(service, mapper)
        {
            _columnService = columnService;
        }
        [HttpGet]
        [AllowAnonymous]
        public ApiResult GetTestCache()
        {
            var str = _columnService.GetTest();
            return new ApiResult(str);
        }
        [HttpGet]
        public override Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        {
            return _columnService.GetListPagesAsync(keywordListTenantQuery);
        }
        [HttpPost]
        public override Task<ApiResult> Add([FromBody] ColumnInput columnInput)
        {
            return _columnService.AddToUpdateAsync(columnInput);
        }
        [HttpPut]
        public override Task<ApiResult> Modify([FromBody] ColumnModifyInput columnModifyInput)
        {
            return _columnService.ModifyAsync(columnModifyInput);
        }
        /// <summary>
        /// 所有父栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult> GetAllParentColumn()
        {
            return _columnService.GetAllParentColumnAsync();
        }
    }
}