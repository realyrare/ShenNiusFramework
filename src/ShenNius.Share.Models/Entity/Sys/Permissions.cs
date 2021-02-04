using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ShenNius.Share.Model.Entity.Sys
{
    ///<summary>
    /// 权限角色管理菜单表
    ///</summary>
    [SugarTable("Sys_Permissions")]
    public partial class Permissions
    {
        public Permissions()
        {


        }
        /// <summary>
        /// Desc:角色Guid
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int RoleId { get; set; }

        /// <summary>
        /// 管理员编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Desc:菜单Guid
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int MenuId { get; set; }

        /// <summary>
        /// 角色-菜单-权限按钮Json
        /// </summary>
        public string BtnFunJson { get; set; }

        /// <summary>
        /// 授权类型1=角色-菜单 2=用户-角色 3=角色-菜单-按钮功能
        /// 默认=1
        /// </summary>
        public int Types { get; set; } = 1;
    }
}
