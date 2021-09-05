using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using ShenNius.Share.Common;
using ShenNius.Share.Infrastructure.Configurations;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShenNius.Share.Infrastructure.Common
{
    public class WebHelper
    {  
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="toName"></param>
        /// <param name="toAddress"></param>
        public static void SendEmail(string subject, string content, string toName, string toAddress)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(AppSettings.Email.FromName, AppSettings.Email.FromAddress));
                if (!string.IsNullOrEmpty(toName) && !string.IsNullOrEmpty(toAddress))
                {
                    message.To.Add(new MailboxAddress(toName, toAddress));
                }
                else
                {
                    message.To.Add(new MailboxAddress("mhg", "mhg215@yeah.net"));
                }
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = content
                };
                using (var client = new SmtpClient())
                {
                    //client.QueryCapabilitiesAfterAuthenticating = false;
                    client.Connect("smtp.qq.com", 587, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: since we don't have an OAuth2 token, disable 	
                    // the XOAUTH2 authentication mechanism.     
                    client.Authenticate(AppSettings.Email.FromAddress, AppSettings.Email.AuthCode);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Failed. Error: {ex.Message}");
                #endif
                LogHelper.Default.ProcessError("邮件发送", ex.Message);
            }
        }
        /// <summary>
        /// 显示错层方法
        /// </summary>
        public static string LevelName(string name, decimal? level)
        {
            if (level > 1)
            {
                string nbsp = "";
                for (int i = 0; i < level; i++)
                {
                    nbsp += "　";
                }
                name = nbsp + "|--" + name;
            }
            return name;
        }
        /// <summary>
        /// 移除文本字符的a标签
        /// </summary>
        public static string ReplaceStrHref(string content)
        {

            var r = new Regex(@"<a\s+(?:(?!</a>).)*?>|</a>", RegexOptions.IgnoreCase);
            Match m;
            for (m = r.Match(content); m.Success; m = m.NextMatch())
            {
                content = content.Replace(m.Groups[0].ToString(), "");
            }
            return content;
        }
        /// <summary>
        /// 移除字符文本Img里面Alt关键字包裹的内链
        /// </summary>
        public static string RemoveStrImgAlt(string content)
        {
            Regex rg2 = new Regex("(?<=alt=\"<a[^<]*)</a>\"");
            if (rg2.Match(content).Success)
            {
                content = rg2.Replace(content, "");
            }
            Regex rg = new Regex("(?<=alt=\")<a href=\"[^>]*>");
            if (rg.Match(content).Success)
            {
                content = rg.Replace(content, "");
            }
            return content;
        }
        /// <summary>
        /// 图片后缀是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ImgSuffixIsExists(IFormFile file)
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

        /// <summary>
        /// 处理数据库树形结构数据。比如menu\category\column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentId">树形父id</param>
        /// <param name="id">树形id</param>
        /// <param name="getDataCallback">回调函数</param>
        /// <returns></returns>
        public static async Task<Tuple<int, string>> DealTreeData<T>(int parentId, int id, Func<Task<T>> getDataCallback) where T: BaseTenantTreeEntity
        {
            string parentIdList = ""; int layer = 0;
            if (parentId > 0)
            {
                // 说明有父级  根据父级，查询对应的模型
                var model = await getDataCallback();
                if (model?.Id > 0)
                {
                    parentIdList = model.ParentList + id + ",";
                    layer = model.Layer + 1;
                }
            }
            else
            {
                parentIdList = "," + id + ",";
                layer = 1;
            }
            return new Tuple<int, string>(layer, parentIdList);
        }

        /// <summary>
        /// 树形数据递归 比如menu\category\column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="newlist"></param>
        /// <param name="parentId"></param>
        public static void ChildNode<T>(List<T> list, List<T> newlist, int parentId) where T : BaseTenantTreeEntity
        {
            var result = list.Where(p => p.ParentId == parentId).OrderBy(p => p.Layer).ToList();
            if (!result.Any()) return ;
            for (int i = 0; i < result.Count; i++)
            {
                newlist.Add(result[i]);
                ChildNode(list, newlist, result[i].Id);
            }
        }
       
    }
}
