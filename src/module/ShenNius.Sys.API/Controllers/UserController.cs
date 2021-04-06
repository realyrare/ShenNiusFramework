﻿using Microsoft.AspNetCore.Authorization;
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
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Models.Dtos.Input;
using ShenNiusSystem.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Model.Entity.Sys;
using System.Linq.Expressions;
using ShenNius.Share.Infrastructure.Cache;

namespace ShenNius.Sys.API.Controllers
{/// <summary>
 /// 用户控制器
 /// </summary>
    public class UserController : ApiControllerBase
    {
        readonly IOptions<JwtSetting> _jwtSetting;
        private readonly IUserService _userService;
        private readonly ICacheHelper _cache;
        private readonly IR_User_RoleService _r_User_RoleService;
        private readonly IMenuService _menuService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtSetting"></param>
        /// <param name="userService"></param>
        /// <param name="cache"></param>
        /// <param name="r_User_RoleService"></param>
        /// <param name="menuService"></param>
        public UserController(IOptions<JwtSetting> jwtSetting, IUserService userService, ICacheHelper cache, IR_User_RoleService r_User_RoleService, IMenuService menuService)
        {
            _jwtSetting = jwtSetting;
            _userService = userService;
            _cache = cache;
            _r_User_RoleService = r_User_RoleService;
            _menuService = menuService;
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userRegisterInput"></param>
        /// <returns></returns>
        [HttpPost, Authority(Module = "user",Method ="add")]
        public async Task<ApiResult> Register([FromBody] UserRegisterInput userRegisterInput)
        {
            return await _userService.RegisterAsync(userRegisterInput);
        }
        [HttpPost, Authority(Module = "user", Method = "edit")]
        public async Task<ApiResult> Modify([FromBody] UserModifyInput userModifyInput)
        {
            return await _userService.ModfiyAsync(userModifyInput);
        }
        [HttpDelete, Authority(Module = "user", Method = "delete")]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return await _userService.DeletesAsync(commonDeleteInput.Ids);
        }
        [HttpPost]
        public async Task<ApiResult> ModfiyPwd([FromBody] ModifyPwdInput modifyPwdInput)
        {
            return await _userService.ModfiyPwdAsync(modifyPwdInput);
        }
        [HttpGet]
        public async Task<ApiResult> GetUser(int id)
        {
            if (id == 0)
            {
                throw new FriendlyException(nameof(id));
            }
            return await _userService.GetUserAsync(id);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authority(Module ="user")]
        public async Task<ApiResult> GetListPages(int page, string key)
        {
            Expression<Func<User, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Name.Contains(key);
            }
            var res = await _userService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        /// <summary>
        /// 设置角色（授权）
        /// </summary>
        /// <param name="setUserRoleInput"></param>
        /// <returns></returns>
        [HttpPost, Authority(Module = "user",Method ="auth")]
        public async Task<ApiResult> SetRole([FromBody]SetUserRoleInput setUserRoleInput)
        {
            return await _r_User_RoleService.SetRoleAsync(setUserRoleInput);          
        }

        [HttpGet]
        [AllowAnonymous]
        public ApiResult LoadLoginInfo()
        {
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();
            if (rsaKey.Count <= 0 || rsaKey == null)
            {
                throw new ArgumentNullException("获取登录的公钥和私钥为空");
            }
            //获得公钥和私钥
            _cache.Set("LOGINKEY" + number, rsaKey);
            return new ApiResult(data: new { RsaKey = rsaKey, Number = number });
        }

        /// <summary>
        ///用户前后端分离的登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("v1/sign-in")]
        [AllowAnonymous]
        public async Task<ApiResult<LoginOutput>> SignIn([FromBody] LoginInput loginInput)
        {
            var rsaKey = _cache.Get<List<string>>("LOGINKEY" + loginInput.NumberGuid);
            if (rsaKey == null)
            {
                return new ApiResult<LoginOutput>("登录失败，请刷新浏览器再次登录!");
            }
            //Ras解密密码
            var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
            loginInput.Password = ras.Decrypt(loginInput.Password);
            var result = await _userService.LoginAsync(loginInput);
            var token = GetJwtToken(result.Data);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResult<LoginOutput>("生成的token字符串为空!");
            }
            result.Data.Token = token;
            return result;
        }
        /// <summary>
        /// asp.net core page私有定制登陆
        /// </summary>
        /// <param name="loginInput">登陆实体</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<LoginOutput>> PageSignIn([FromBody] LoginInput loginInput)
        {
            var result = await _userService.LoginAsync(loginInput);
            if (result.StatusCode == 500)
            {
                result.Data = new LoginOutput();
                return result;
            }
            var token = GetJwtToken(result.Data);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResult<LoginOutput>("生成的token字符串为空!");
            }
            //请求当前用户的所有权限并存到缓存里面并发给前端 准备后面鉴权使用
            var menuAuths=  await _menuService.GetCurrentAuthMenus(result.Data.Id);
            result.Data.MenuAuthOutputs = menuAuths;
            result.Data.Token = token;
            return result;
        }

        [HttpPost]
        public ApiResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ApiResult(data: "/user/login");
        }
        private string GetJwtToken(LoginOutput loginOutput)
        {
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, loginOutput.LoginName),
                    new Claim(JwtRegisteredClaimNames.Sid, loginOutput.Id.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_jwtSetting.Value.ExpireSeconds).ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role,"Type"),
                    new Claim("mobile",loginOutput.Mobile)
            };
            var token = JwtHelper.BuildJwtToken(claims.ToArray(), _jwtSetting);
            return token;
        }
    }
}