using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Input
{
   public class UserRegisterInput
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
        /// <summary>
        /// 状态 1=整除 0=不允许登录
        /// </summary>
        public bool Status { get; set; } = true;

    }
}
