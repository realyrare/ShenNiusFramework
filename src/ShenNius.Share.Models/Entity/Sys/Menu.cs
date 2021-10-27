using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Model.Entity.Sys
{
    ///<summary>
    /// 系统菜单表
    ///</summary>
    [SugarTable("Sys_Menu")]
    public partial class Menu : BaseEntity
    {
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
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string HttpMethod { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
        [SugarColumn(IsJson = true)]
        public string[] BtnCodeIds { get; set; }

        public string ParentIdList { get; set; }
        public int Layer { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string BtnCodeName { get; set; }

    }
}
