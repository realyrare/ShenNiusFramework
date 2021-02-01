using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Entity.Sys
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Sys_R_User_Role")]
    public partial class R_User_Role
    {
        public R_User_Role()
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
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int UserId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int RoleId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public bool IsEnable { get; set; }

    }
}
