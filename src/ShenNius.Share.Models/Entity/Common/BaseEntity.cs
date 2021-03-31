using SqlSugar;
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
    public interface IEntity
    {
         int Id { get; set; }
         DateTime? ModifyTime { get; set; }
         DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 假删除
    /// </summary>
    public interface IDeleted
    {
        public DateTime? DeleteTime { get; set; }
        public bool Status { get; set; }
    }
    public class BaseEntity: IGlobalSite,IEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int SiteId { get; set; }
        public DateTime? ModifyTime { get; set; }
        public DateTime CreateTime { get; set; }

    }
}