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

            Dictionary<AdvEnum, string> dic = new Dictionary<AdvEnum, string>
            {
                { AdvEnum.FriendlyLink, AdvEnum.FriendlyLink.GetEnumText() },
                 { AdvEnum.Slideshow, AdvEnum.Slideshow.GetEnumText() },
                  { AdvEnum.GoodBlog, AdvEnum.GoodBlog.GetEnumText() },
                   { AdvEnum.MiniApp, AdvEnum.MiniApp.GetEnumText() },
            };
            ViewBag.Dic = dic;
            return View(model);
        }
    }
}
