using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Models.Configs;
using System.Collections.Generic;
using System.IO;

/*************************************
* 类 名： LocalFile
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/18 14:19:38
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.FileManager
{
    /// <summary>
    ///本地图片上传 使用的时候单独把类注入
    /// </summary>
    public class LocalFile : IUploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalFile(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        public ApiResult Delete(string filename)
        {
            throw new System.NotImplementedException();
        }

        public ApiResult List(string prefix, string marker)
        {
            throw new System.NotImplementedException();
        }

        public ApiResult Upload(IFormFile file, string prefix)
        {
            var path = CreateDirectory(prefix);
            //var file = Request.Form.Files[0];
            var fileName = WebHelper.ImgSuffixIsExists(file);
            string fileFullName = path + "\\" + fileName;
            using (FileStream fs = File.Create(fileFullName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return new ApiResult(data: Path.Combine($"/Files/{prefix}/", fileName));
        }

        public ApiResult Upload(IFormFileCollection files, string prefix)
        {
            var path = CreateDirectory(prefix);
            List<string> list = new List<string>();
            foreach (var file in files)
            {
                var fileName = WebHelper.ImgSuffixIsExists(file);
                string fileFullName = Path.Combine(path,fileName);
                using (FileStream fs = File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                list.Add(Path.Combine($"/Files/{prefix}/", fileName));
            }
            return new ApiResult(data: list);
        }

        private string CreateDirectory(string prefix)
        {
            string path;
            if (!string.IsNullOrEmpty(prefix))
            {
                path = string.Concat(_webHostEnvironment.WebRootPath, $"\\Files\\{prefix}");
            }
            else
            {
                path = string.Concat(_webHostEnvironment.WebRootPath, "\\Files");
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}