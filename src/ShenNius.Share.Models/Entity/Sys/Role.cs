using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ShenNius.Share.Model.Entity.Sys
{
    ///<summary>
    /// 权限角色表
    ///</summary>
    [SugarTable("Sys_Role")]
    public partial class Role
    {
      
            public Role()
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
            /// Nullable:True
            /// </summary>           
            public string Name { get; set; }

            /// <summary>
            /// Desc:
            /// Default:DateTime.Now
            /// Nullable:False
            /// </summary>           
            public DateTime CreateTime { get; set; }
            public DateTime ModifyTime { get; set; } = DateTime.Now;
            public string Description { get; set; }
        }
}
