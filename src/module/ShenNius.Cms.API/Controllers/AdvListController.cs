using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Domain.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Configs;

/*************************************
* 类 名： AdvListController
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 18:09:15
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{

    public class AdvListController : ApiTenantBaseController<AdvList, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, AdvListInput, AdvListModifyInput>
    {
        private readonly IBaseServer<AdvList> _service;       
        private readonly IUploadHelper _uploadHelper;

        public AdvListController(IBaseServer<AdvList> service, IMapper mapper, IUploadHelper  uploadHelper) : base(service, mapper)
        {
            _service = service;
            this._uploadHelper = uploadHelper;
        }
        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        {
            Expression<Func<AdvList, bool>> whereExpression = d=>d.Status==true;
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
        public ApiResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _uploadHelper.Upload(files, "advList/");
            return new ApiResult(data: data);
        }
    }
}