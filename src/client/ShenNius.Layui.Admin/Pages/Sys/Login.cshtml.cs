using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.ShenNius.Client.Admin.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
            var result = await _httpHelper.PostAsync<ApiResult>("user/log-out", null, "application/json");
            Response.Redirect("/sys/login/");
        }

      
        public FileResult OnGetVCode()
        {
            var vcode = VerifyCode.CreateRandomCode(4);
            HttpContext.Session.SetString("vcode", vcode);
            var img = VerifyCode.DrawImage(vcode, 20, Color.White);
            return File(img, "image/gif");
        }

        public async Task<IActionResult> OnPostSubmitAsync(LoginInput loginInput)
        {
            var apiResult = new ApiResult<LoginOutput>() { StatusCode = 500, Success = false };
            try
            {
              
                if (string.IsNullOrEmpty(loginInput.Captcha))
                {
                    apiResult.Msg = "验证码错误!";
                    return new JsonResult(apiResult);
                }
               var vcode=HttpContext.Session.GetString("vcode");
                if (string.IsNullOrEmpty(vcode))
                {
                    apiResult.Msg = "服务端验证码错误!";
                    return new JsonResult(apiResult);
                }
                if (!vcode.ToLower().Equals(loginInput.Captcha.ToLower()))
                {
                    apiResult.Msg = "验证码错误!";
                    return new JsonResult(apiResult);
                }
                var rsaKey = _cache.Get<List<string>>(LoginKey + loginInput.NumberGuid);
                if (rsaKey == null)
                {
                    apiResult.Msg = "登录失败，请刷新浏览器再次登录!";
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

                var result = await _httpHelper.PostAsync<ApiResult<LoginOutput>>("user/page-sign-in", JsonConvert.SerializeObject(loginInput), "application/json");
                if (result.StatusCode == 500)
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
                              new Claim("mobile",result.Data.Mobile),
                              new Claim("trueName",result.Data.TrueName)
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
                apiResult.Msg = e.Message;
                return new JsonResult(apiResult);
            }
        }
    }
}
