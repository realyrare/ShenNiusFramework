using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Input
{
   public class LoginInput
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Loginname { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        public string NumberGuid { get; set; }
    }
}
