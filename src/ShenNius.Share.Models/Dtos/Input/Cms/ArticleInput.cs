using ShenNius.Share.Models.Dtos.Common;
using System;

namespace ShenNius.Share.Models.Dtos.Input.Cms
{
    public class ArticleInput: GlobalSiteInput
    {

        /// <summary>
        /// Desc:栏目ID
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int ColumnId { get; set; } = 0;


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
        /// Desc:是否在回收站
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsRecycle { get; set; } = false;

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
        /// Desc:添加时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;



    }
}
