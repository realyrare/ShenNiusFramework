using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

/*************************************
* 类名：Keyword
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/31 19:03:29
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Cms
{
    /// <summary>
    /// 关键词
    /// </summary>
    [SugarTable("Cms_Keyword")]
    public class Keyword:BaseSiteEntity
    {
        public string  Title { get; set; }
        public string Url { get; set; }
    }
}