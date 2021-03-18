﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Infrastructure.ImgUpload;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(IWebHostEnvironment webHostEnvironment )
        {
            this._webHostEnvironment = webHostEnvironment;
        }
        public ApiResult QiniuFile([FromBody]string filePath)
        {
          var i=  QiniuCloud.UploadFile(filePath);
            return new ApiResult(i);
        }
        public ApiResult LocalFile()
        {
            string path=string.Concat(_webHostEnvironment.ContentRootPath, "\\wwwroot\\Files");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var file = Request.Form.Files[0];
            ImgDealwith(file);
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
            return new ApiResult(filename2);
        }
        private void ImgDealwith(IFormFile file)
        {
            string fileExt = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            if (fileExt == null)
            {
                throw new FriendlyException("上传的文件没有后缀");
            }
            //判断文件大小    
            long length = file.Length;
            if (length > 1024 * 1024 * 2) //2M
            {
                throw new FriendlyException("上传的文件不能大于2M");
            }
            string imgTypes = ".gif|.jpg|.php|.jsp|.jpeg|.png|......";
            if (imgTypes.IndexOf(fileExt.ToLower(), StringComparison.Ordinal) <= -1)
            {
                throw new FriendlyException("上传的文件不是图片");
            }
        }
    }
}
