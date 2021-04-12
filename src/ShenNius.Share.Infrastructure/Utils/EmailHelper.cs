using MailKit.Net.Smtp;
using MimeKit;
using System;

/*************************************
* 类名：EmailHelper
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/12 16:02:58
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Utils
{
    public class EmailHelper
    {
        public static void Send(string subject, string content)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("shennius", "1248415209@qq.com"));
                message.To.Add(new MailboxAddress("mhg", "mhg215@yeah.net"));
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
                    client.Authenticate("1248415209@qq.com", "rsvehznvpixojagf");
                    client.Send(message);
                    client.Disconnect(true);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed. Error: {ex.Message}");
            }
        }
    }
}