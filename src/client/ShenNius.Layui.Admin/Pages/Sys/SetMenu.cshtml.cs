using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Layui.Admin.Pages.Sys
{
    public class SetMenuModel : PageModel
    {
        [BindProperty]
        public int RoleId { get; set; }
        public void OnGet(int id)
        {
            RoleId = id;
        }
    }
}
