using MailKit.Net.Smtp;
using MimeKit;
using ShenNius.Share.Infrastructure.Configurations;
using System;
using System.Text.RegularExpressions;

namespace ShenNius.Share.Infrastructure.Common
{
    public class WebHelper
    {     
        public static void Send(string subject, string content, string toName, string toAddress)
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
                LogHelper.Default.ProcessError(500, ex.Message);
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
    }
}
