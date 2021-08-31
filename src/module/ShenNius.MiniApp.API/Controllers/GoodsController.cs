using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Enums.Shop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Dtos.Input.Shop;
using System.Linq.Expressions;
using ShenNius.Share.Models.Dtos.Common;
using SqlSugar;
/*************************************
* 类名：GoodsController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/31 14:46:34
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.MiniApp.API.Controllers
{
    public class GoodsController : MiniAppBaseController
    {
        private readonly IGoodsService _goodsService;

        public GoodsController(IGoodsService goodsService)
        {
            _goodsService = goodsService;
        }
        [HttpGet("BuyNow")]
        public  Task<ApiResult> BuyNow(int goodsId, int goodsNum,string goodsNo)
        {
            /*订单购买  查询商品需要知道商品的id,商品的编码*/
            //查询实体
            return _goodsService.GetBuyNowAsync(goodsId, goodsNum, goodsNo,HttpWx.TenantId);          
        }
        [HttpGet("detail")]
        public  Task<ApiResult<GoodsModifyInput>> Detail(int goodsId)
        {
            return _goodsService.DetailAsync(goodsId);
        }
        [HttpGet("lists")]
        public  Task<ApiResult> Lists( string sortType, int sortPrice, int categoryId)
        {
            var query = new ListTenantQuery() { Page = 1, Limit = 20, TenantId = HttpWx.TenantId };
            Expression<Func<Goods, Category, GoodsSpec, bool>> expression = (g, c, gc) => g.Status == true;

            Expression<Func<Goods, Category, GoodsSpec, object>> order = (g, c, gc) => g.CreateTime;

            OrderByType sort = OrderByType.Desc;

            if (categoryId > 0)
            {
                expression = (g, c, gc) =>  g.CategoryId == categoryId;
            }

            if (sortType == "all" && sortPrice == 0)
            {
                order = (g, c, gc) =>g.SalesActual;
            }
            else if (sortType == "all" && sortPrice == 1)
            {
                order = (g, c, gc) =>g.Id;
            }
            else if (sortType == "sales" && sortPrice == 1)
            {
                order = (g, c, gc)=>gc.GoodsSales;
            }
            else if (sortType == "price" && sortPrice == 0)
            {
                order = (g, c, gc) => gc.GoodsPrice;
                sort = OrderByType.Asc;
            }
            else if (sortType == "price" && sortPrice == 1)
            {
                order = (g, c, gc) => gc.GoodsPrice;
            }
            
            return  _goodsService.GetByWherePageAsync(query,order, sort,expression);
          
        }
    }
}