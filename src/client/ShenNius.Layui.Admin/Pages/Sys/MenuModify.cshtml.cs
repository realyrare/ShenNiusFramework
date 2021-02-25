using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Client.Admin.Pages.Sys
{
    public class MenuModifyModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }
        public void OnGet(int id)
        {
            Id = id;
        }
    }
}
