using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Client.Admin.Pages.Sys
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {

        }
        public async void OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Redirect("/admin/login/");
            //return new JsonResult(new ApiResult<string>() { data = "/admin/login/" });
        }
        /// <summary>
        /// ÍË³öµÇÂ¼
        /// </summary>
        /// <returns></returns>
        //public async Task<IActionResult> OnPostLogoutAsync()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return new JsonResult(new ApiResult<string>() { Data = "/admin/login/" });
        //}
    }
}
