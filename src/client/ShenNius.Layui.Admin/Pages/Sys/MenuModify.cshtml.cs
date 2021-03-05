using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShenNius.Layui.Admin.Common;
using ShenNius.Layui.Admin.Model;
using ShenNius.Layui.Admin.Model;

namespace ShenNius.Layui.Admin.Pages.Sys
{
    public class MenuModifyModel : PageModel
    {
        private readonly HttpHelper _httpHelper;

        public MenuModifyModel(HttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }
        [BindProperty]
        public int Id { get; set; }   
        public void OnGet(int id)
        {
            Id = id;
         
        }
    }
}
