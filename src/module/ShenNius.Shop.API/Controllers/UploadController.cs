using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Shop;
using System.Threading.Tasks;

namespace ShenNius.Shop.API.Controllers
{
    /// <summary>
    /// 商品分类控制器
    /// </summary>
    public class UploadController : ControllerBase
    {
        private readonly IUploadHelper _uploadHelper;
        public UploadController( IUploadHelper uploadHelper) 
        {
            _uploadHelper = uploadHelper;
        }       
        [HttpPost]
        public ApiResult File([FromBody] UploadInput input)
        {
            if (string.IsNullOrEmpty(input.Directory))
            {
                throw new FriendlyException("图片的上传目录不能为空！");
            }
            var files = Request.Form.Files;
            var data = _uploadHelper.Upload(files, input.Directory);
            return data;
        }
    }
}
