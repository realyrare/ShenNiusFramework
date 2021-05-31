using ShenNius.Share.Models.Entity.Common;
using SqlSugar;
using System;

namespace ShenNius.Share.Model.Entity.Sys
{
    [SugarTable("Sys_User")]
    public class User: BaseTenantEntity
    { 
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 真是姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; } = "男";

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public string Ip { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 当前登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

    }
}
