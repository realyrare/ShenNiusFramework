using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Input;
using ShenNiusSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IR_User_RoleService _r_User_RoleService;
        private readonly IMenuService _menuService;
        private readonly ICacheHelper _cacheHelper;
        private readonly ICurrentUserContext _currentUserContext;

        /// <param name="jwtSetting"></param>
        /// <param name="userService"></param>
        /// <param name="r_User_RoleService"></param>
        /// <param name="menuService"></param>
        /// <param name="cacheHelper"></param>
        /// <param name="currentUserContext"></param>
        /// <param name="hubContext"></param>
        public UserController(IUserService userService, IR_User_RoleService r_User_RoleService, IMenuService menuService, ICacheHelper cacheHelper, ICurrentUserContext currentUserContext)
        {
            _userService = userService;
            _r_User_RoleService = r_User_RoleService;
            _menuService = menuService;
            _cacheHelper = cacheHelper;
            _currentUserContext = currentUserContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();
            if (rsaKey.Count <= 0 || rsaKey == null)
            {
                throw new FriendlyException("获取登录的公钥和私钥为空");
            }
            ViewBag.RsaKey = rsaKey;
            ViewBag.Number = number;
            //获得公钥和私钥
            _cacheHelper.Set($"{KeyHelper.User.LoginKey}:{number}", rsaKey);
            return View();
        }
        [HttpPost]
        public async Task<ApiResult<LoginOutput>> Login(LoginInput loginInput)
        {
            var rsaKey = _cacheHelper.Get<List<string>>($"{KeyHelper.User.LoginKey}:{loginInput.NumberGuid}");
            if (rsaKey == null)
            {
                throw new FriendlyException("登录失败，请刷新浏览器再次登录!");
            }
            var result = await _userService.LoginAsync(loginInput);
            if (result.StatusCode == 500)
            {
                result.Data = new LoginOutput();
                return result;
            }
            //请求当前用户的所有权限并存到缓存里面并发给前端 准备后面鉴权使用
            var menuAuths = await _menuService.GetCurrentAuthMenus(result.Data.Id);
            if (menuAuths == null || menuAuths.Count == 0)
            {
                throw new FriendlyException("不好意思，该用户当前没有权限。请联系系统管理员分配权限！");
            }
            result.Data.MenuAuthOutputs = menuAuths;
            var identity = new ClaimsPrincipal(
               new ClaimsIdentity(new[]
                   {
                              new Claim(ClaimTypes.Sid,result.Data.Id.ToString()),
                              new Claim(ClaimTypes.Name,result.Data.LoginName),
                              new Claim(ClaimTypes.WindowsAccountName,result.Data.LoginName),
                              new Claim(ClaimTypes.UserData,result.Data.LoginTime.ToString()),
                              new Claim("mobile",result.Data.Mobile),
                              new Claim("trueName",result.Data.TrueName)
                   }, CookieAuthenticationDefaults.AuthenticationScheme)
              );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddHours(24),
                IsPersistent = true,
                AllowRefresh = false
            });
            _cacheHelper.Remove($"{KeyHelper.User.LoginKey}:{loginInput.NumberGuid}");
            return result;
        }
        [HttpGet]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Redirect("/user/login/");
        }
    }
}
