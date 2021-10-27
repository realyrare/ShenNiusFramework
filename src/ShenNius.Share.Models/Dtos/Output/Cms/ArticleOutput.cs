using System;

/*************************************
* 类名：ArticleOutput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/2 17:24:48
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Cms
{
    public class ArticleOutput
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public int ColumnId { get; set; } = 0;
        public string Title { get; set; }
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
        public DateTime CreateTime { get; set; }
        public string Tag { get; set; }

        public string EnTitle { get; set; }

        public string ThumImg { get; set; }
        public string Content { get; set; }
        public string ColumnName { get; set; }
        /// <summary>
        /// Desc:点击量
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int Hits { get; set; } = 0;



        /// <summary>
        /// Desc:最后点击时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? LastHitTime { get; set; }
        public string KeyWord { get; set; }
        /// <summary>
        /// Desc:是否置顶
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// Desc:是否热点
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsHot { get; set; }
        public string ParentColumnUrl { get; set; }
    }
}