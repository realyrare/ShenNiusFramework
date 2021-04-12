using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShenNius.Share.Infrastructure.JsonWebToken;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenNius.Share.Infrastructure.Extension
{
    public static class AuthorizationExtension
    {
        public static void AddAuthorizationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new FriendlyException(nameof(services));



            #region 1、基于角色的API授权 
            // 1【授权】、这个很简单，其他什么都不用做， 只需要在API层的controller上边，增加特性即可，注意，只能是角色的:
            // [Authorize(Roles = "Admin,System")]
            #endregion

            #region 2、基于策略的授权（简单版）
            // 1【授权】、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Anchor", policy => policy.RequireRole("Anchor").Build());
            //    options.AddPolicy("Store", policy => policy.RequireRole("Store").Build());
            //    options.AddPolicy("MemberAll", policy => policy.RequireRole("Anchor", "Store"));
            //    options.AddPolicy("Admin", policy => policy.RequireRole("Anchor", "Store", "Admin"));
            //});
            #endregion



            //读取配置文件       
            //JwtSetting jwtSetting = new JwtSetting();
            //configuration.Bind("JwtSetting", jwtSetting);
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSetting>();
            if (jwtSetting == null)
            {
                throw new FriendlyException(nameof(JwtSetting));
            }
            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtSetting.Issuer,//发行人
                ValidateAudience = true,
                ValidAudience = jwtSetting.Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(jwtSetting.ExpireSeconds),//ClockSkew默认值为20s，它是一个缓冲期
                RequireExpirationTime = true,
            };

            //2.1【认证】、core自带官方JWT认证
            // 开启Bearer认证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseHandler);
                o.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
             // 添加JwtBearer服务
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnChallenge = context =>
                     {
                         context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                         var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);

                         if (jwtToken.Issuer != jwtSetting.Issuer)
                         {
                             context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                         }

                         if (jwtToken.Audiences.FirstOrDefault() != jwtSetting.Audience)
                         {
                             context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                         }
                         // 如果过期，则把<是否过期>添加到，返回头信息中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             })
             .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });
        }
    }
}

