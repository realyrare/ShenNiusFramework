using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShenNius.Layui.Admin.Common;
using ShenNius.Layui.Admin.Extension;
using ShenNius.Layui.Admin.Model.Output;
using System.Threading.Tasks;

namespace ShenNius.Layui.Admin.Pages.Cms
{
    public class ArticleModifyModel : PageModel
    {
        private readonly HttpHelper _httpHelper;

        public ArticleModifyModel(HttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }
        [BindProperty]
        public ArticleOutput articleOutput { get; set; } = new ArticleOutput();
        public async Task OnGet(int id, string token)
        {
            if (id > 0 && !string.IsNullOrEmpty(token))
            {
                var result = await _httpHelper.GetAsync<ApiResult<ArticleOutput>>($"article/detail?id={id}", token);
                if (result != null && result.Success && result.StatusCode == 200)
                {
                    articleOutput = result.Data;
                }
            }
        }
    }
}
