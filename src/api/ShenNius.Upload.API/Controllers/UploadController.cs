using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ShenNiusSystem.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShenNius.Upload.API.Controllers
{
    /// <summary>
    /// 图片上传控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment )
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult UploadFile()
        {

            string path = _hostingEnvironment.ContentRootPath + "\\wwwroot\\Files";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var file = Request.Form.Files[0];
            string fileExt = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            //string filename = Guid.NewGuid().ToString() + "." + fileExt;
            //计算文件的MD5值
            string filename2 = Md5Crypt.GetStreamMd5(file.OpenReadStream()) + "." + fileExt;
            string fileFullName = path + "\\" + filename2;
            using (FileStream fs = System.IO.File.Create(fileFullName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            //添加到数据库
            return new JsonResult(new { IsSucceed = true, ResultMsg = "上传成功", ImgUrl = "/Files/" + filename2 });
        }
    }
}
