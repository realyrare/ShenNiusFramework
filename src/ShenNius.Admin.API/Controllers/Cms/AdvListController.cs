using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Admin.API.Controllers;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Domain.Repository;
using System.Threading.Tasks;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Domain.Services.Cms;

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

namespace ShenNius.Admin.API.Controllers.Cms
{

    public class AdvListController : ApiTenantBaseController<AdvList, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, AdvListInput, AdvListModifyInput>
    {
       
        private readonly IUploadHelper _uploadHelper;
        private readonly IAdvListService _advListService;

        public AdvListController(IBaseServer<AdvList> service, IMapper mapper, IUploadHelper  uploadHelper, IAdvListService advListService) : base(service, mapper)
        {
            this._uploadHelper = uploadHelper;
            this._advListService = advListService;
        }
        [HttpGet]
        public override  Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        {
            return _advListService.GetPagesAsync(keywordListTenantQuery);
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