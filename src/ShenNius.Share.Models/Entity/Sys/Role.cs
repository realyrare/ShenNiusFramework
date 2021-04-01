using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Model.Entity.Sys
{
    ///<summary>
    /// 权限角色表
    ///</summary>
    [SugarTable("Sys_Role")]
    public partial class Role : BaseSiteEntity
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
