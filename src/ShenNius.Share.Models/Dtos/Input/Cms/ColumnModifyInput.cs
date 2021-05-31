using ShenNius.Share.Models.Dtos.Common;
using System;

/*************************************
* 类 名： ColumnModifyInput
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/15 19:05:45
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Cms
{
    public class ColumnModifyInput: GlobalTenantInput
    {
        public int Id { get; set; }
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
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; } = DateTime.Now;

    }
}