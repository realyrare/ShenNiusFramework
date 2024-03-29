using System;
using System.Collections.Generic;
using System.Drawing;
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
using ShenNius.Layui.Admin.Common;
using ShenNius.Layui.Admin.Extension;
using ShenNius.Layui.Admin.Model;

namespace ShenNius.Layui.Admin.Pages.Sys
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
        private void LoadKey()
        {
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();
            if (rsaKey.Count <= 0 || rsaKey == null)
            {
                throw new ArgumentNullException("��ȡ��¼�Ĺ�Կ��˽ԿΪ��");
            }
            RsaKey = rsaKey;
            Number = number;
            //��ù�Կ��˽Կ
            _cache.Set(LoginKey + number, rsaKey);
        }
        public void OnGet()
        {
            LoadKey();
        }
        public async void OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            LoadKey();
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
                    apiResult.Msg = "��֤�����!";
                    return new JsonResult(apiResult);
                }
                var vcode = HttpContext.Session.GetString("vcode");
                if (string.IsNullOrEmpty(vcode))
                {
                    apiResult.Msg = "�������֤�����!";
                    return new JsonResult(apiResult);
                }
                if (!vcode.ToLower().Equals(loginInput.Captcha.ToLower()))
                {
                    apiResult.Msg = "��֤�����!";
                    return new JsonResult(apiResult);
                }
                var rsaKey = _cache.Get<List<string>>(LoginKey + loginInput.NumberGuid);
                if (rsaKey == null)
                {
                    apiResult.Msg = "��¼ʧ�ܣ���ˢ��������ٴε�¼!";
                    return new JsonResult(apiResult);
                }
                if (string.IsNullOrEmpty(loginInput.LoginName) || string.IsNullOrEmpty(loginInput.Password))
                {
                    apiResult.Msg = "�û����������!";
                    return new JsonResult(apiResult);
                }
                //Ras��������
                var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
                loginInput.Password = ras.Decrypt(loginInput.Password);

                var result = await _httpHelper.PostAsync<ApiResult<LoginOutput>>("user/page-sign-in", JsonConvert.SerializeObject(loginInput), "application/json");
                if (result.StatusCode == 500)
                {
                    return new JsonResult(result);
                }
                //��Ȩ��
                //_cache.Set($"frontAuthMenu:{result.Data.Id}", result.Data.MenuAuthOutputs);
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
                    ExpiresUtc = DateTime.UtcNow.AddHours(24),
                    IsPersistent = true,
                    AllowRefresh = false
                });
                _cache.Remove(LoginKey + loginInput.NumberGuid);
                return new JsonResult(result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains("statusCode") && e.Message.Contains("success") && e.Message.Contains("msg"))
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<ApiResult>(e.Message);
                        apiResult.Msg = result.Msg;
                        return new JsonResult(apiResult);
                    }
                    catch
                    {

                    }
                }
                apiResult.Msg = e.Message;
                return new JsonResult(apiResult);
            }
        }
    }
}
