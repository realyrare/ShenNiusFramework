using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.ViewModels.Output
{
   public class LoginOutput
    {
        public int Id { get; set; }
        public string  LoginName { get; set; }
        public DateTime LoginTime { get; set; }
        public string  TrueName { get; set; }
        public string  Token { get; set; }
    }
}
