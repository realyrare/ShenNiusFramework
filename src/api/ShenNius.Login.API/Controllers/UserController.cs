using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.JsonWebToken.Model;
using ShenNius.Share.Infrastructure.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using ShenNius.Share.Service.Sys;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Output;
using ShenNius.Sys.API.Authority;
using ShenNiusSystem.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ShenNius.Sys.API.Controllers
{/// <summary>
 /// 用户控制器
 /// </summary>
    public class UserController : ApiControllerBase
    {
        readonly IOptions<JwtSetting> _jwtSetting;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtSetting"></param>
        /// <param name="userService"></param>
        /// <param name="cache"></param>
        public UserController(IOptions<JwtSetting> jwtSetting, IUserService userService, IMemoryCache cache)
        {
            _jwtSetting = jwtSetting;
            _userService = userService;
            _cache = cache;
        }
        [HttpPost]
        public ApiResult PostTest([FromBody] LoginInput loginInput)
        {
            return new ApiResult("自己定义的错误码  不知道能不能走到这来",408);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiResult> GetListPages(int page, string key)
        {
            var res = await _userService.GetPagesAsync(page, 15);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpGet]
        [AllowAnonymous]
        public ApiResult LoadLoginInfo()
        {         
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();            
            if (rsaKey.Count <= 0|| rsaKey==null)
            {
                throw new ArgumentNullException("获取登录的公钥和私钥为空");   
            }
            //获得公钥和私钥
            _cache.Set("LOGINKEY" + number, rsaKey);
            return new ApiResult(data:new{ RsaKey = rsaKey, Number = number });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<LoginOutput>> SignIn([FromBody]LoginInput loginInput)
        {
            var rsaKey = _cache.Get<List<string>>("LOGINKEY" + loginInput.NumberGuid);
            if (rsaKey == null)
            {
                return new ApiResult<LoginOutput>("登录失败，请刷新浏览器再次登录!");
            }
            //Ras解密密码
            var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
            loginInput.Password = ras.Decrypt(loginInput.Password);
            var result = await _userService.Login(loginInput);
            var token = GetJwtToken(result.Data);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResult<LoginOutput>("生成的token字符串为空!");
            }
            result.Data.Token = token;
            return result;
        }
        [HttpPost]
        public ApiResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ApiResult(data:"/sys/login");
        }
        private string GetJwtToken(LoginOutput loginOutput)
        {
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, loginOutput.LoginName),
                    new Claim(JwtRegisteredClaimNames.Jti, loginOutput.Id.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_jwtSetting.Value.ExpireSeconds).ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role,"Type"),
                    new Claim("mobile",loginOutput.Mobile)
            };
            var token = JwtHelper.BuildJwtToken(claims.ToArray(), _jwtSetting);
            return token;
        }
    }
}
