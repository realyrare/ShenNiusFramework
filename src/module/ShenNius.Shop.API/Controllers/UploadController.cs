using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;

namespace ShenNius.Shop.API.Controllers
{
    /// <summary>
    /// 商品分类控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
