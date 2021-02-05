using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ShenNius.Client.Admin.Common;
using ShenNius.Client.Admin.Model;

namespace ShenNius.Client.Admin.Pages.Sys
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IMemoryCache _cache;
        private readonly HttpHelper _httpHelper;

        public LoginModel(IMemoryCache memoryCache, HttpHelper httpHelper)
        {
            _cache = memoryCache;
            _httpHelper = httpHelper;
        }
        private const string LoginKey = "LOGINKEY";
        [BindProperty]
        public List<string> RsaKey { get; set; }
        public string Number { get; set; }
        public void OnGet()
        {
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();
            if (rsaKey.Count <= 0 || rsaKey == null)
            {
                throw new ArgumentNullException("获取登录的公钥和私钥为空");
            }
            RsaKey = rsaKey;
            Number = number;
            //获得公钥和私钥
            _cache.Set(LoginKey + number, rsaKey);
        }
        public async void OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //确保安全一定要去清除api的token
            Response.Redirect("/sys/login/");
            //return new JsonResult(new ApiResult<string>() { data = "/admin/login/" });
        }

        public FileResult OnVCode()
        {
            var vcode = VerifyCode.CreateRandomCode(4);
            HttpContext.Session.SetString("vcode", vcode);
            var img = VerifyCode.DrawImage(vcode, 20, Color.White);
            return File(img, "image/gif");
        }

        public async Task<IActionResult> OnPostSubmitAsync(LoginInput loginInput)
        {
            try
            {
                var apiResult = new ApiResult<LoginOutput>() { StatusCode = 500,Success=false };
                var rsaKey = _cache.Get<List<string>>(LoginKey + loginInput.NumberGuid);
                if (rsaKey == null)
                {
                    apiResult.Msg="登录失败，请刷新浏览器再次登录!";
                    return new JsonResult(apiResult);
                }
                if (string.IsNullOrEmpty(loginInput.LoginName) || string.IsNullOrEmpty(loginInput.Password))
                {
                    apiResult.Msg = "用户和密码必填!";
                    return new JsonResult(apiResult);
                }
                //Ras解密密码
                var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
                loginInput.Password = ras.Decrypt(loginInput.Password);
                var api = "https://localhost:5001/api/";
                var result = await _httpHelper.PostAsync<ApiResult<LoginOutput>>(api + "user/page-sign-in", JsonConvert.SerializeObject(loginInput), "application/json");
                if (result.StatusCode==500)
                {
                    return new JsonResult(result);
                }
                var identity = new ClaimsPrincipal(
                   new ClaimsIdentity(new[]
                       {
                              new Claim(ClaimTypes.Sid,result.Data.Id.ToString()),
                              new Claim(ClaimTypes.Name,result.Data.LoginName),
                              new Claim(ClaimTypes.WindowsAccountName,result.Data.LoginName),
                              new Claim(ClaimTypes.UserData,result.Data.LoginTime),
                              new Claim("mobile",result.Data.Mobile)
                       }, CookieAuthenticationDefaults.AuthenticationScheme)
                  );
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddHours(1),
                    IsPersistent = true,
                    AllowRefresh = false
                });
                _cache.Remove(LoginKey + loginInput.NumberGuid);
                return new JsonResult(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns></returns>
    //public async Task<IActionResult> OnPostLogoutAsync()
    //{
    //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //    return new JsonResult(new ApiResult<string>() { Data = "/admin/login/" });
    //}

}
