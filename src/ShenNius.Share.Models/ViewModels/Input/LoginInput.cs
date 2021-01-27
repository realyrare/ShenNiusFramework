using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.ViewModels.Input
{
   public class LoginInput
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string password { get; set; }
    }
}
