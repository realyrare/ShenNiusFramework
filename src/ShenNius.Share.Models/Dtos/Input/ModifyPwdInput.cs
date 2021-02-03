using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Input
{
  public  class ModifyPwdInput
    {
        public int Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }
    }
}
