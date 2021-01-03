
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ShenNius.Share.Service.Configurations
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
        public static class Db
        {
            public static string Connection = string.Empty;//=> _config["ConnectionStrings:MySql"];
        }
    }
}
