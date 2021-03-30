using System;

/*************************************
* 类名：Common
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 17:24:54
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Common
{
    public class GlobalSite
    {
        public int SiteId { get; set; }
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// Desc:添加时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}