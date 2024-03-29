﻿/*************************************
* 类名：GlobalTenantInput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 18:15:44
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

using ShenNius.Share.Models.Entity.Common;

namespace ShenNius.Share.Models.Dtos.Common
{
    /// <summary>
    /// 多租户约定方便Add ,Modify使用
    /// </summary>
    public class GlobalTenantInput : IGlobalTenant
    {
        public int TenantId { get; set; }
    }
    public class UploadInput
    {
        public string Directory { get; set; }
    }
}