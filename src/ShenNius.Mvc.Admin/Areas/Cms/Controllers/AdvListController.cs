using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Enums.Cms;
using ShenNius.Share.Models.Enums.Extension;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("cms")]
    public class AdvListController : Controller
    {
        private readonly IAdvListService _advListService;

        public AdvListController(IAdvListService advListService)
        {
            _advListService = advListService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            AdvList model = id == 0 ? new AdvList() : await _advListService.GetModelAsync(d => d.Id == id && d.Status);

            Dictionary<int, string> dic = new Dictionary<int, string>
            {
                { AdvEnum.FriendlyLink.GetValue<int>(), AdvEnum.FriendlyLink.GetEnumText() },
                 { AdvEnum.Slideshow.GetValue<int>(), AdvEnum.Slideshow.GetEnumText() },
                  { AdvEnum.GoodBlog.GetValue<int>(), AdvEnum.GoodBlog.GetEnumText() },
                   { AdvEnum.MiniApp.GetValue<int>(), AdvEnum.MiniApp.GetEnumText() },
            };
            ViewBag.Dic = dic;
            return View(model);
        }
    }
}
