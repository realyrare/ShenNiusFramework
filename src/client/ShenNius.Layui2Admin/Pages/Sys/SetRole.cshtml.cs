using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Layui.Admin.Pages.Sys
{
    public class SetRoleModel : PageModel
    {
        [BindProperty]
        public int UserId { get; set; }
        public void OnGet(int id)
        {
            UserId = id;
        }
    }
}
