using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

/*************************************
* 类名：GenderEnum
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/10 19:57:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Enums.Shop
{
    public enum GenderEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Man = 1,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Woman女 = 2
    }
}