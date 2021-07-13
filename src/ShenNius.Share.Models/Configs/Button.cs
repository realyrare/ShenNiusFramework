using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：Button
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/7/13 14:12:15
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Configs
{
    /// <summary>
    /// 菜单按钮配置
    /// </summary>
    public class Button
    {
        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 导出
        /// </summary>
        public string Export { get; set; }
        /// <summary>
        /// 导入
        /// </summary>
        public string Import { get; set; }
        /// <summary>
        /// 删除
        /// </summary>
        public string Delete { get; set; }
        /// <summary>
        /// 编辑
        /// </summary>
        public string Edit { get; set; }
        /// <summary>
        /// 添加
        /// </summary>
        public string Add { get; set; }
        /// <summary>
        /// 授权
        /// </summary>
        public string Auth { get; set; }

    }
}