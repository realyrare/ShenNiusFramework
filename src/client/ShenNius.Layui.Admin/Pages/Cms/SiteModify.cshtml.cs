using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShenNius.Layui.Admin.Common;
using ShenNius.Layui.Admin.Extension;
using ShenNius.Layui.Admin.Model.Output;

namespace ShenNius.Layui.Admin.Pages.Cms
{
    public class SiteModifyModel : PageModel
    {
        private readonly HttpHelper _httpHelper;

        public SiteModifyModel(HttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public SiteOutput SiteOutput { get; set; } = new SiteOutput();
        public async Task OnGet(int id, string token)
        {
            Id = id;
            if (!string.IsNullOrEmpty(token))
            {
                var result = await _httpHelper.GetAsync<ApiResult<SiteOutput>>($"site/detail?id={id}", token);
                if (result != null && result.Success && result.StatusCode == 200)
                {
                    SiteOutput = result.Data;
                }
            }
        }
    }
}
