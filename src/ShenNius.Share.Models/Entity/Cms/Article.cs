using ShenNius.Share.Models.Entity.Common;
using SqlSugar;
using System;

/*************************************
* 类 名： Article
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/3 15:33:30
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　      版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Cms
{
    [SugarTable("Cms_Article")]
    public class Article : BaseTenantEntity
    {
        /// <summary>
        /// Desc:栏目ID
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int ColumnId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string ColumnName { get; set; }
        /// <summary>
        /// Desc:文章标题
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Desc:文章标题颜色
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string TitleColor { get; set; }

        /// <summary>
        /// Desc:文章副标题
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Desc:作者
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Desc:来源
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Desc:是否外链
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsLink { get; set; } = false;

        /// <summary>
        /// Desc:外部链接地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// Desc:文章标签
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Desc:文章图
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string ThumImg { get; set; }

        /// <summary>
        /// Desc:视频链接地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Desc:是否置顶
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsTop { get; set; } = false;

        /// <summary>
        /// Desc:是否热点
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsHot { get; set; } = false;

        /// <summary>
        /// Desc:是否允许评论
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsComment { get; set; } = false;


        /// <summary>
        /// Desc:审核状态
        /// Default:b'1'
        /// Nullable:False
        /// </summary>
        public bool Audit { get; set; } = true;

        /// <summary>
        /// SEO关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 文章摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Desc:删除到回收站时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? DeleteTime { get; set; }



    }
}