using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Domain.Services.Sys;
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
        private readonly IConfigService _configService;


        public GoodsController(IGoodsService  goodsService,IConfigService configService)
        {
            _goodsService = goodsService;
            _configService = configService;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var datas = await _configService.GetListAsync(d => d.Type.Equals("Freight"));
            ViewBag.Freights = datas;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            var result = await _goodsService.DetailAsync(id);
            var datas = await _configService.GetListAsync(d => d.Type.Equals("Freight"));
            ViewBag.Freights = datas;

            return View(result.Data);
        }
    }
}
