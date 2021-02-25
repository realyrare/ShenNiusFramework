using SqlSugar;
using System;

namespace ShenNius.Share.Models.Entity.Sys
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Sys_R_Role_Menu")]
    public partial class R_Role_Menu
    {
        public R_Role_Menu()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int RoleId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int MenuId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public bool IsPass { get; set; } = true;
        [SugarColumn(IsJson = true)]
        public string[] BtnCodeIds { get; set; }

    }
}
