
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ShenNius.Share.Infrastructure.Configurations
{
    public class AppSettings
    {
        /// <summary>
        /// 配置文件的根节点
        /// </summary>
        private static readonly IConfigurationRoot _config;

        /// <summary>
        /// Constructor
        /// </summary>
        static AppSettings()
        {
            // 加载appsettings.json，并构建IConfigurationRoot
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            _config = builder.Build();
        }
        /// <summary>
        /// mvc 不启用jwt
        /// </summary>
        public static class Jwt
        {
            public static bool Value => !string.IsNullOrEmpty(_config["JwtConfig:IsEnable"]) ? Convert.ToBoolean(_config["JwtConfig:IsEnable"]) : true;
        }
        public static class Db
        {
            public static string Connection = string.Empty;//=> _config["ConnectionStrings:MySql"];
        }
        public static class DbTable
        {
            public static string Value => _config["DbTable:Value"];
        }
        public static class Email
        {
            /// <summary>
            /// 发送人
            /// </summary>
            public static string FromName => _config["Email:FromName"];
            /// <summary>
            /// 发送邮件地址
            /// </summary>
            public static string FromAddress => _config["Email:FromAddress"];
            /// <summary>
            /// 授权码
            /// </summary>
            public static string AuthCode => _config["Email:AuthCode"];
        }
    }
}
