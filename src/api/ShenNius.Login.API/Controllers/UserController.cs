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
using ShenNius.Share.Models.ViewModels.Input;
using ShenNius.Share.Models.ViewModels.Output;

namespace ShenNius.Sys.API.Controllers
{/// <summary>
 /// 用户控制器
 /// </summary>
    public class UserController : ApiControllerBase
    {
        readonly IOptions<JwtSetting> _jwtSetting;
        private readonly IUserService _userService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtSetting"></param>
        /// <param name="userService"></param>
        public UserController(IOptions<JwtSetting> jwtSetting, IUserService userService)
        {
            _jwtSetting = jwtSetting;
            _userService = userService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key)
        {
            var res = await _userService.GetPagesAsync(page, 15);
            return new ApiResult() { Data = new { count = res.TotalItems, items = res.Items } };
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> SignIn([FromBody] LoginInput loginInput)
        {
            var loginModel = await _userService.GetModelAsync(d => d.Name.Equals(loginInput.loginname) && d.Password.Equals(loginInput.password));
            if (loginModel == null)
            {
                return new ApiResult() { Data = "SignIn", StatusCode = 400, Msg = "用户名或密码错误" };
            }
            var token = GetJwtToken(loginModel.Id);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResult() { StatusCode = 500, Msg = "生成的token字符串为空!" };
            }
            var loginOutput = new LoginOutput() { LoginName = loginModel.Name, TrueName = loginModel.TrueName, Id = loginModel.Id, Token = token, LoginTime = loginModel.LoginTime.Value };
            return new ApiResult() { Data = loginOutput, StatusCode = 200, Msg = "msg" };
        }
        /// <summary>
        /// 请求用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]
        public ApiResult GetUser()
        {
            return new ApiResult() { Data = "GetUser", StatusCode = 200, Msg = "GetUser" };
        }
        private string GetJwtToken(int id)
        {
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, "UserName"),
                    new Claim(JwtRegisteredClaimNames.Jti, id.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_jwtSetting.Value.ExpireSeconds).ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role,"Type"),
                    new Claim("mobile","Mobile")
            };
            var token = JwtHelper.BuildJwtToken(claims.ToArray(), _jwtSetting);
            return token;
        }
    }
}
