using SqlSugar;
using System;

namespace ShenNius.Share.Model.Entity.Sys
{
    [SugarTable("Sys_User")]
    public class User
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 归属角色
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 归属部门
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 归属部门
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// 部门集合
        /// </summary>
        public string DepartmentIdList { get; set; }

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
        /// 编号
        /// </summary>
        public string Number { get; set; }

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
        public string Remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 当前登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

    }
}
