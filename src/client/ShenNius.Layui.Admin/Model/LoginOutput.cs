using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
