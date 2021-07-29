using System.Collections.Generic;

namespace ShenNius.Layui.Admin.Model
{
    public class LoginOutput
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Mobile { get; set; }
        public string LoginTime { get; set; }
        public string TrueName { get; set; }
        public string Token { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public List<MenuAuthOutput> MenuAuthOutputs { get; set; } = new List<MenuAuthOutput>();
    }
}
