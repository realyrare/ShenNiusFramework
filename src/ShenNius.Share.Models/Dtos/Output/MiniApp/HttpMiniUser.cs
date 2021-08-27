using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：HttpMiniUser
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/27 15:10:31
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Output.MiniApp
{
    public class HttpMiniUser
    {
        [JsonProperty("errcode")]
        public int Errcode { get; set; } = 0;
        [JsonProperty("errmsg")]
        public string Errmsg { get; set; }
        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [JsonProperty("openid")]
        public string Openid { get; set; }
        /// <summary>
        ///     会话密钥
        /// </summary>
        [JsonProperty("session_key")]
        public string Session_key { get; set; }
        /// <summary>
        ///     用户在开放平台的唯一标识符。本字段在满足一定条件的情况下才返回。具体参看：https://mp.weixin.qq.com/debug/wxadoc/dev/api/uinionID.html
        /// </summary>
        [JsonProperty("unionid")]
        public string Unionid { get; set; }
    }
}