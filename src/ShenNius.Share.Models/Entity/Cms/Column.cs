using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

/*************************************
* 类名：Column
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 16:57:35
*┌───────────────────────────────────┐　    
*│　      版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Cms
{
    [SugarTable("Cms_Column")]
    public class Column : BaseTenantEntity
    {
        /// <summary>
        /// Desc:栏目标题
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Desc:英文栏位名称
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string EnTitle { get; set; }

        /// <summary>
        /// Desc:栏位副标题
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Desc:父栏目
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// Desc:栏位集合
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string ParentList { get; set; }

        /// <summary>
        /// Desc:栏位等级
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int Layer { get; set; }


        /// <summary>
        /// Desc:栏位属性
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Attr { get; set; }

        /// <summary>
        /// Desc:栏位图片
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// Desc:关键词
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Desc:描述
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Summary { get; set; }
    }
}