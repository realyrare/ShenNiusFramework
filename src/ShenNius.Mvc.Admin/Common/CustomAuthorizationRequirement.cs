using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Common
{
    /// <summary>
    /// 策略授权参数
    /// </summary>
    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomAuthorizationRequirement(string policyname)
        {
            this.Name = policyname;
        }

        public string Name { get; set; }
    }
}
