using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Util;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using System.Collections.Generic;
using System.IO;

/*************************************
* 类 名： QiniuCloud
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/17 11:12:43
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.FileManager
{
    public class QiniuCloud : IUploadHelper
    {
        private readonly QiNiuOss _qiNiuOssModel;
        public QiniuCloud(IOptionsMonitor<QiNiuOss> qiNiuOssModel)
        {
            _qiNiuOssModel = qiNiuOssModel.CurrentValue;
        }
        /// <summary>
        /// 根据前缀获得文件列表
        /// </summary>
        /// <param name="prefix">指定前缀，只有资源名匹配该前缀的资源会被列出</param>
        /// <param name="marker">上一次列举返回的位置标记，作为本次列举的起点信息</param>
        /// <returns></returns>
        public ApiResult List(string prefix = "case", string marker = "")
        {
            Mac mac = new Mac(_qiNiuOssModel.Ak, _qiNiuOssModel.Sk);
            // 设置存储区域
            Config.SetZone(ZoneID.CN_South, false);
            BucketManager bucketManager = new BucketManager(mac);
            // 指定目录分隔符，列出所有公共前缀（模拟列出目录效果）
            string delimiter = "";
            // 本次列举的条目数，范围为1-1000
            int limit = 20;
            prefix = _qiNiuOssModel.BasePath + prefix;
            ListResult listRet = bucketManager.ListFiles(_qiNiuOssModel.Bucket, prefix, marker, limit, delimiter);
            if (listRet.Code != 200)
            {
                throw new FriendlyException(listRet.Text);
            }
            return new ApiResult(data: listRet);
        }

        /// <summary>
        /// 删除云端图片
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns></returns>
        public ApiResult Delete(string filename)
        {
            Mac mac = new Mac(_qiNiuOssModel.Ak, _qiNiuOssModel.Sk);
            // 设置存储区域
            Config.SetZone(ZoneID.CN_South, false);
            BucketManager bucketManager = new BucketManager(mac);
            // 文件名
            filename = filename.Replace(_qiNiuOssModel.ImgDomain, "");
            HttpResult result = bucketManager.Delete(_qiNiuOssModel.Bucket, filename);
            if (result.Code != 200)
            {
                throw new FriendlyException(result.Text);
            }
            return new ApiResult(data: result);
        }


        public ApiResult Upload(IFormFileCollection files, string prefix)
        {

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(_qiNiuOssModel.Ak, _qiNiuOssModel.Sk);
            string saveKey = _qiNiuOssModel.BasePath + prefix;
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy
            {
                // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
                // putPolicy.Scope = bucket + ":" + saveKey;
                Scope = _qiNiuOssModel.Bucket
            };
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            //设置上传域名区域
            Config.SetZone(ZoneID.CN_South, false);
            UploadManager um = new UploadManager();
            List<string> list = new List<string>();
            foreach (IFormFile file in files)
            {
                var fileName = WebHelper.ImgSuffixIsExists(file);
                saveKey += fileName;
                Stream stream = file.OpenReadStream();
                HttpResult result = um.UploadStream(stream, saveKey, token);
                if (result.Code != 200)
                {
                    throw new FriendlyException(result.Text);
                }
                else
                {
                    list.Add(_qiNiuOssModel.ImgDomain + saveKey);

                }
            }
            //HttpResult result = um.UploadFile(localFile, saveKey, token);        
            return new ApiResult(data: list);
        }
        public ApiResult Upload(IFormFile file, string prefix)
        {

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(_qiNiuOssModel.Ak, _qiNiuOssModel.Sk);
            string saveKey = _qiNiuOssModel.BasePath + prefix;
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = _qiNiuOssModel.Bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            //设置上传域名区域
            Config.SetZone(ZoneID.CN_South, false);
            UploadManager um = new UploadManager();
            var fileName = WebHelper.ImgSuffixIsExists(file);
            saveKey += fileName;
            Stream stream = file.OpenReadStream();
            HttpResult result = um.UploadStream(stream, saveKey, token);
            if (result.Code != 200)
            {
                throw new FriendlyException(result.Text);
            }

            //HttpResult result = um.UploadFile(localFile, saveKey, token);        
            return new ApiResult(data: _qiNiuOssModel.ImgDomain + saveKey);
        }

        /// <summary>
        /// 删除云端图片
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns></returns>
        private string GetToken()
        {
            Mac mac = new Mac(_qiNiuOssModel.Ak, _qiNiuOssModel.Sk);
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = _qiNiuOssModel.Bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            if (string.IsNullOrEmpty(token))
            {
                throw new FriendlyException("七牛云图片token获取为空!");
            }
            return token;
        }
    }
}