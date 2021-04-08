using SqlSugar;
using System;

/*************************************
* 类名：Recycle
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/8 18:01:45
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Sys
{
    public class Recycle
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }
        public int BusinessId { get; set; }
        public string TableType { get; set; }
        public DateTime CreateTime { get; set; }
        public string  Remark { get; set; }
    }
}