using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Infrastructure.Utils;
using ShenNiusSystem.Common;
using System;
using System.Drawing;
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

        public UserController(IUserService userService, IR_User_RoleService r_User_RoleService, IMenuService menuService, ICacheHelper cacheHelper, ICurrentUserContext currentUserContext)
        {
            _userService = userService;
            _r_User_RoleService = r_User_RoleService;
            _menuService = menuService;
            _cacheHelper = cacheHelper;
            _currentUserContext = currentUserContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();
            if (rsaKey.Count <= 0 || rsaKey == null)
            {
                throw new FriendlyException("获取登录的公钥和私钥为空");
            }
            ViewBag.RsaKey = rsaKey[0];
            ViewBag.Number = number;
            //获得公钥和私钥
            _cacheHelper.Set($"{KeyHelper.User.LoginKey}:{number}", rsaKey);
            return View();
        }
       
        [HttpGet]
        public FileResult OnGetVCode()
        {
            var vcode = VerifyCode.CreateRandomCode(4);
            HttpContext.Session.SetString("vcode", vcode);
            var img = VerifyCode.DrawImage(vcode, 20, Color.White);
            return File(img, "image/gif");
        }
        [HttpGet]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Redirect("/user/login/");
        }
    }
}
