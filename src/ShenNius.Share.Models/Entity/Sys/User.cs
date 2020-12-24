using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Entity.Sys
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public int Name { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string Mobile { get; set; }
    }
}
