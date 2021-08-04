using Microsoft.AspNetCore.Http;
using ShenNius.Share.Common;
using ShenNius.Share.Infrastructure.Extension;
using System;

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

namespace ShenNius.Share.Infrastructure.ImgUpload
{
    /// <summary>
    /// 使用的时候单独把类注入
    /// </summary>
    public class LocalFile
    {
        //private readonly IWebHostEnvironment _webHostEnvironment;

        //public LocalFile(IWebHostEnvironment webHostEnvironment)
        //{
        //    this._webHostEnvironment = webHostEnvironment;
        //}

        //public string Upload(IFormFile file)
        //{
        //    string path = string.Concat(_webHostEnvironment.ContentRootPath, "\\wwwroot\\Files");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    //var file = Request.Form.Files[0];
        //     var fileName=  ImgDealwith(file);
        //    string fileFullName = path + "\\" + fileName;

        //    using (FileStream fs = File.Create(fileFullName))
        //    {
        //        file.CopyTo(fs);
        //        fs.Flush();
        //    }
        //    return fileName;
        //}
        public static string ImgDealwith(IFormFile file)
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
            string filename = Md5Crypt.GetStreamMd5(file.OpenReadStream()) + "." + fileExt;
            return filename;
        }    
    }
}