using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShenNius.Layui.Admin.Pages.Cms
{
    public class ColumnModifyModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }
        public void OnGet(int id)
        {
            Id = id;
        }
    }
}
