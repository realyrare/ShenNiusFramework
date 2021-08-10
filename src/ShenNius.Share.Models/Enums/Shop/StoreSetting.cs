using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

/*************************************
* 类名：StoreSetting
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/10 19:54:23
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Enums.Shop
{
    public enum StoreSetting
    {
        /// <summary>
        /// 短信通知
        /// </summary>
        [Description("短信通知")]
        Sms=0,
        /// <summary>
        /// 上传设置
        /// </summary>
        [Description("上传设置")]
        Storage=1,
        /// <summary>
        /// 商城设置
        /// </summary>
        [Description("商城设置")]
        Store=2,
        /// <summary>
        /// 交易设置
        /// </summary>
        [Description("交易设置")]
        Trade=3
    }
}