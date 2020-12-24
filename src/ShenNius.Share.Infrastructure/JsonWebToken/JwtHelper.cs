using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShenNius.Infrastructure.JsonWebToken.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShenNius.Share.Infrastructure.JsonWebToken
{
    public class JwtHelper
    {
        /// <summary>   
        /// 获取基于JWT的Token
        /// </summary>
        /// <param name="claims">需要在登陆的时候配置</param>
        /// <param name="jwtSetting">配置信息</param>
        /// <returns></returns>

        public static string BuildJwtToken(Claim[] claims, IOptions<JwtSetting> jwtSetting)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Value.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // 实例化JwtSecurityToken
            var jwtToken = new JwtSecurityToken(
                issuer: jwtSetting.Value.Issuer,
                audience: jwtSetting.Value.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            // 生成 Token
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
