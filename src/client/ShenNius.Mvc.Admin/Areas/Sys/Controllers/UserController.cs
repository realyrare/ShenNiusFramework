using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Common;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Output.Sys;
using System;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Sys.Controllers
{

    public partial class UserController : Controller
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
        /// <summary>
        /// 用户首页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 设置角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SetRole()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id=0)
        {
            User model = null;
            if (id == 0)
            {
                model = new User();
            }
            else
            {
                model = await _userService.GetModelAsync(d => d.Id == id&&d.Status);
            }            
            return View(model);
        }
        [HttpGet]
        public IActionResult ModifyPwd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CurrentUserInfo()
        {
            UserOutput userOutput = new UserOutput();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userOutput.Name = HttpContext.User.Identity.Name;
                userOutput.Id =Convert.ToInt32( HttpContext.User.Claims.Where(d => d.Type == JwtRegisteredClaimNames.Sid).Select(d => d.Value).FirstOrDefault());
                userOutput. Mobile = HttpContext.User.Claims.Where(d => d.Type == "mobile").Select(d => d.Value).FirstOrDefault();
                userOutput. Email = HttpContext.User.Claims.Where(d => d.Type == ClaimTypes.Email).Select(d => d.Value).FirstOrDefault();
                userOutput.TrueName = HttpContext.User.Claims.Where(d => d.Type == "trueName").Select(d => d.Value).FirstOrDefault();
            }
            else
            {
                Redirect("/user/login");
            }
            return View(userOutput);
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

        [HttpGet, AllowAnonymous]
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
            Response.Redirect("/sys/user/login/");
        }
    }
}
