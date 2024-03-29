﻿using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Sys
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Sys_R_Role_Menu")]
    public partial class R_Role_Menu : BaseEntity
    {
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
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public bool IsPass { get; set; } = true;
        [SugarColumn(IsJson = true)]
        public string[] BtnCodeIds { get; set; }

    }
}
