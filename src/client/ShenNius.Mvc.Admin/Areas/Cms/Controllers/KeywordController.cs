using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Models.Entity.Cms;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("shop")]
    public partial class KeywordController : Controller
    {
        private readonly IKeywordService _keywordService;

        public KeywordController(IKeywordService KeywordService)
        {
            _keywordService = KeywordService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Keyword model = id == 0 ? new Keyword() : await _keywordService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
