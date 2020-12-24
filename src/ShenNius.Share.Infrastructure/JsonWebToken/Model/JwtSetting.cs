using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Infrastructure.JsonWebToken.Model
{
    public class JwtSetting
    {

        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string SecurityKey { get; set; }

        public int ExpireSeconds { get; set; }
    }
}
