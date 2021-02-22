using SqlSugar;
using System;

namespace ShenNius.Share.Model.Entity.Sys
{
    ///<summary>
    /// 系统菜单表
    ///</summary>
    [SugarTable("Sys_Menu")]
    public partial class Menu
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int ParentId { get; set; }
        public string NameCode { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Url { get; set; }

        /// <summary>
        /// Desc:
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string HttpMethod { get; set; }

        public DateTime ModifyTime { get; set; } 

        public bool Status { get; set; }
        public int Sort { get; set; }
        public string  Icon { get; set; }

    }
}
