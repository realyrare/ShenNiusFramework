/*************************************
* 类名：GlobalSiteInput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 18:15:44
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

using ShenNius.Share.Models.Entity.Common;

namespace ShenNius.Share.Models.Dtos.Common
{   
    public class GlobalSiteInput : IGlobalSite
    {
        public int SiteId { get; set; }
    }
}