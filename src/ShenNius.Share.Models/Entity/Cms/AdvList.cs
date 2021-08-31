using System;
using ShenNius.Share.Models.Entity.Common;
using ShenNius.Share.Models.Enums.Cms;
using ShenNius.Share.Models.Enums.Extension;
using SqlSugar;
namespace ShenNius.Share.Models.Entity.Cms
{
    /// <summary>
    /// 广告位管理
    /// </summary>
    [SugarTable("Cms_AdvList")]
    public class AdvList : BaseTenantEntity
    {
        /// <summary>
        /// Desc:广告位名称
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Title {get;set;}

        /// <summary>
        /// Desc:广告位类型
        /// Default:0
        /// Nullable:False
        /// </summary>
        public AdvEnum Type { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string TypeName
        {
            get
            {
                string name = "";
                if (Type == AdvEnum.FriendlyLink)
                {
                    name = AdvEnum.FriendlyLink.GetEnumText();
                }
                if (Type == AdvEnum.Slideshow)
                {
                    name= AdvEnum.Slideshow.GetEnumText();
                }
                if (Type == AdvEnum.GoodBlog)
                {
                    name= AdvEnum.GoodBlog.GetEnumText();
                }
                if (Type == AdvEnum.MiniApp)
                {
                    name = AdvEnum.MiniApp.GetEnumText();
                }
                return name;
            }
        }
        /// <summary>
        /// Desc:图片地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string ImgUrl {get;set;}

        /// <summary>
        /// Desc:链接地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string LinkUrl {get;set;}

        /// <summary>
        /// Desc:打开窗口类型
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Desc:是否启用时间显示
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsTimeLimit { get; set; }

        /// <summary>
        /// Desc:开始时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? BeginTime {get;set;}

        /// <summary>
        /// Desc:结束时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? EndTime {get;set;}

        /// <summary>
        /// Desc:排序
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int Sort { get; set; } = 0;

        public string Summary { get; set; }
    }
}
