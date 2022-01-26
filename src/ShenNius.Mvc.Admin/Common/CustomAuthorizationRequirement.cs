using Microsoft.AspNetCore.Authorization;

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
