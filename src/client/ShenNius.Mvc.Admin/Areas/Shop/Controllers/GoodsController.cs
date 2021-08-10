using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Entity.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Shop.Controllers
{
    public class GoodsController : Controller
    {
        private readonly IGoodsService _goodsService;

        public GoodsController(IGoodsService  goodsService)
        {
            this._goodsService = goodsService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Goods model = id == 0 ? new Goods() : await _goodsService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
