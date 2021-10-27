using ShenNius.Share.Models.Dtos.Input.Sys;
using System;
using System.Collections.Generic;

namespace ShenNius.Share.Models.Dtos.Input
{
    public class LoginOutput
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Mobile { get; set; }
        public DateTime LoginTime { get; set; }
        public string TrueName { get; set; }
        public string Token { get; set; }
        /// <summary>
        /// 是否已经登录
        /// </summary>
        public bool IsLogin { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public List<MenuAuthOutput> MenuAuthOutputs { get; set; } = new List<MenuAuthOutput>();
    }
}
