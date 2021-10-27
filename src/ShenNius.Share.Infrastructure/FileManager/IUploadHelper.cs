using Microsoft.AspNetCore.Http;
using ShenNius.Share.Models.Configs;

/*************************************
* 类名：IUploadHelper
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/5 16:15:47
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.FileManager
{
    public interface IUploadHelper
    {
        /// <summary>
        /// 批量上传
        /// </summary>
        /// <param name="files"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        ApiResult Upload(IFormFileCollection files, string prefix);
        ApiResult Upload(IFormFile file, string prefix);
        /// <summary>
        /// 根据文件名删除文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        ApiResult Delete(string filename);
        /// <summary>
        /// 根据前缀获得文件列表
        /// </summary>
        /// <param name="prefix">指定前缀，只有资源名匹配该前缀的资源会被列出</param>
        /// <param name="marker">上一次列举返回的位置标记，作为本次列举的起点信息</param>
        /// <returns></returns>
        ApiResult List(string prefix, string marker);
    }
}