﻿using SqlSugar;
using System;

namespace ShenNius.Share.Models.Entity.Sys
{
    ///<summary>
    /// 系统操作表
    ///</summary>
    [SugarTable("Sys_Log")]
    public partial class Log
    {

        /// <summary>
        /// Desc:唯一标号Guid
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 应用程序
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public string Callsite { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 认证用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; }

    }
}
