﻿namespace ShenNius.Share.Models.Dtos.Output.Sys
{
    public class UserOutput
    {

        public int Id { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 真是姓名
        /// </summary>
        public string TrueName { get; set; }

        public string Password { get; set; }

        ///// <summary>
        ///// 性别
        ///// </summary>
        //public string Sex { get; set; } = "男";

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }


        //public bool Status { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
