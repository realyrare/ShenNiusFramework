using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Client.Admin.Pages.Sys
{
    public class LogDetailModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }
        public void OnGet(int id)
        {
            Id = id;
        }
    }
}