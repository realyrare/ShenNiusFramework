﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Client.Admin.Dtos
{
    public class UserDto
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public int Guid { get; set; }

        /// <summary>
        /// 归属角色
        /// </summary>
        public string RoleGuid { get; set; }

        /// <summary>
        /// 归属部门
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 归属部门
        /// </summary>
        public string DepartmentGuid { get; set; }

        /// <summary>
        /// 部门集合
        /// </summary>
        public string DepartmentGuidList { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPwd { get; set; }

        /// <summary>
        /// 真是姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPic { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; } = "男";

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 状态 1=整除 0=不允许登录
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; } = DateTime.Now;
    }
}
