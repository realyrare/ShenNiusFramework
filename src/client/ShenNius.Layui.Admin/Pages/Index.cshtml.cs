using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
namespace ShenNius.Layui.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public string  CurrentUserName { get; set; }
        public string CurrentUserId { get; set; }
        public void OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                CurrentUserName = HttpContext.User.Identity.Name;
                CurrentUserId = HttpContext.User.Claims.Where(d => d.Type == ClaimTypes.Sid).Select(d => d.Value).FirstOrDefault();
            }
            else
            {
                Redirect("/sys/login");
            }
        }
    }
}
