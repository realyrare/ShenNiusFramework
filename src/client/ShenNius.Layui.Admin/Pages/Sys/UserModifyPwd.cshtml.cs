using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Layui.Admin.Pages.Sys
{
    public class UserModifyPwdModel : PageModel
    {

        public string CurrentUserId { get; set; }
        public void OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                CurrentUserId = HttpContext.User.Claims.Where(d => d.Type == ClaimTypes.Sid).Select(d => d.Value).FirstOrDefault();
            }
            else
            {
                Redirect("/sys/login");
            }
        }
    }
}
