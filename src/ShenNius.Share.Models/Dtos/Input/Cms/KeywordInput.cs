using ShenNius.Share.Models.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：KeywordInput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/31 19:22:32
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Cms
{
    public class KeywordInput: GlobalSiteInput
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}