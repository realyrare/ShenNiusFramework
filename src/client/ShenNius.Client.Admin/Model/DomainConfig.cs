using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.HuoChong.Common
{
    public class DomainConfig
    {
        /// <summary>
        /// 前台域名
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 图片域名
        /// </summary>
        public string ImgHost { get; set; }
        /// <summary>
        /// api域名
        /// </summary>
        public string  ApiHost { get; set; }
    }
}
