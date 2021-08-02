using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Layui.Admin.Pages.Sys
{
    public class CurrentUserInfoModel : PageModel
    {
        public string CurrentUserName { get; set; }
        public string CurrentUserId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string TrueName { get; set; }

        public void OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                CurrentUserName = HttpContext.User.Identity.Name;
                CurrentUserId = HttpContext.User.Claims.Where(d => d.Type == ClaimTypes.Sid).Select(d => d.Value).FirstOrDefault();
                Mobile = HttpContext.User.Claims.Where(d => d.Type == "mobile").Select(d => d.Value).FirstOrDefault();
                Email = HttpContext.User.Claims.Where(d => d.Type == ClaimTypes.Email).Select(d => d.Value).FirstOrDefault();
                TrueName = HttpContext.User.Claims.Where(d => d.Type == "trueName").Select(d => d.Value).FirstOrDefault();
            }
            else {
                Redirect("/sys/login");
            }
        }
    }
}
