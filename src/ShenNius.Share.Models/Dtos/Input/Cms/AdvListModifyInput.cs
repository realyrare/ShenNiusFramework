using ShenNius.Share.Models.Dtos.Common;
using System;

/*************************************
* 类 名： AdvListModifyInputValidator
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 18:01:26
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Cms
{
    public class AdvListModifyInput : GlobalTenantInput
    {
        public int Id { get; set; }
        /// <summary>
        /// Desc:广告位名称
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Desc:广告位类型
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Desc:是否启用
        /// Default:b'1'
        /// Nullable:False
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Desc:图片地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// Desc:链接地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// Desc:打开窗口类型
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Target { get; set; } = "_target";

        /// <summary>
        /// Desc:是否启用时间显示
        /// Default:b'0'
        /// Nullable:False
        /// </summary>
        public bool IsTimeLimit { get; set; } = false;

        /// <summary>
        /// Desc:开始时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// Desc:结束时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int Sort { get; set; } = 0;
        public DateTime ModifyTime { get; set; } = DateTime.Now;
        public string Summary { get; set; }
    }
}