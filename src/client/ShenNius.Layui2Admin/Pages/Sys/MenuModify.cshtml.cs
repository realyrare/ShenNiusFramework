using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShenNius.Layui.Admin.Common;
using ShenNius.Layui.Admin.Extension;
using ShenNius.Layui.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public MenuOutput MenuOutput { get; set; } = new MenuOutput();
        public List<ConfigOutput> ConfigOutputs { get; set; } = new List<ConfigOutput>();
        public string AlreadyBtns { get; set; }
        public async Task OnGet(int id, string token)
        {
            if (id>0&&!string.IsNullOrEmpty(token))
            {
                var result = await _httpHelper.GetAsync<ApiResult<MenuOutput>>($"menu/detail?id={id}", token);
                if (result!=null&&result.Success && result.StatusCode == 200)
                {                   
                    if (result.Data!=null)
                    {
                        MenuOutput = result.Data;
                        if (result.Data.BtnCodeIds.Length>0)
                        {
                            for (int i = 0; i < result.Data.BtnCodeIds.Length; i++)
                            {
                                AlreadyBtns += result.Data.BtnCodeIds[i] + ",";
                            }
                            if (!string.IsNullOrEmpty(AlreadyBtns))
                            {
                                AlreadyBtns = AlreadyBtns.TrimEnd(',');
                            }
                        }
                    }
                }               
            }
            if (!string.IsNullOrEmpty(token))
            {
                var result2 = await _httpHelper.GetAsync<ApiResult<List<ConfigOutput>>>($"menu/get-btn-code-list", token);
                if (result2 != null && result2.Success && result2.StatusCode == 200)
                {
                    ConfigOutputs = result2.Data;
                }
            }
        }
    }
}
