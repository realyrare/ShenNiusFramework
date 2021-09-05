using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Output.Shop;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
using SqlSugar;
using System;
using System.Linq;
using System.Threading.Tasks;

/*************************************
* 类名：CartService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/20 11:53:40
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Shop
{
    public interface ICartService : IBaseServer<Cart>
    {
        Task<ApiResult> AddAsync(int goodsId, int goodsNum, int appUserId, string specSkuId);
        Task<ApiResult> GetListsAsync(int appUserId,int tenantId);
    }
    public class CartService : BaseServer<Cart>, ICartService
    {
        private readonly IGoodsService _goodsService;

        public CartService(IGoodsService goodsService)
        {
            _goodsService = goodsService;
        }
       public async Task<ApiResult> GetListsAsync(int appUserId, int tenantId)
        {
            var cartList = await GetListAsync(l => l.AppUserId == appUserId);
            var inCludeGoods = cartList.Select(d => d.GoodsId).ToList();
           
            var goodsList = await Db.Queryable<Goods, GoodsSpec>((g, gc) => new JoinQueryInfos(JoinType.Inner, g.Id == gc.GoodsId))
               .Where((g, gc) => g.TenantId == tenantId)
               .Select((g, gc) => new CartGoodsOutput
               {
                   GoodsId= g.Id,
                   ImgUrl= g.ImgUrl,
                   GoodsPrice= gc.GoodsPrice,
                   SpecType=g.SpecType,
                   LinePrice= gc.LinePrice,
                   GoodsSales= gc.GoodsSales,
                   SalesActual=g.SalesActual,
                   SpecMany=g.SpecMany
               }).ToListAsync();

            double totalPrice = 0;
            foreach (var item in goodsList)
            {
                //去查商品对应的购物车中的数量
                var cartGoodsNum = cartList.Where(d => d.GoodsId == item.GoodsId).Select(d => d.GoodsNum).FirstOrDefault();
                item.OrderTotalNum = cartGoodsNum;
                totalPrice += (double)item.GoodsPrice * cartGoodsNum;
                if (item.SpecType == SpecTypeEnum.Multi.GetValue<int>())
                {
                    //显示规格组和值

                }               
            }
            return new ApiResult(new
            {
                GoodsList = goodsList, //单价*数量 
                OrderTotalPrice = totalPrice
            });
        }
        public async Task<ApiResult> AddAsync(int goodsId, int goodsNum,int appUserId,string specSkuId)
        {
            var goodsData = await _goodsService.GoodInfoIsExist(goodsId, goodsNum, specSkuId, appUserId);
            var isExistCartModel = await GetModelAsync(l => l.AppUserId == appUserId && l.GoodsId == goodsId);
            if (isExistCartModel?.Id > 0)
            {
                goodsNum += isExistCartModel.GoodsNum;
                await UpdateAsync(d => new Cart() { GoodsNum = goodsNum,SpecSkuId=specSkuId, ModifyTime = DateTime.Now }, d => d.Id == isExistCartModel.Id&&d.AppUserId==appUserId);
            }
            else
            {
                Cart model = new Cart()
                {
                    GoodsId = goodsData.Item1.Id,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    AppUserId = appUserId,
                    GoodsNum = goodsNum,
                    SpecSkuId=specSkuId
                };
                await AddAsync(model);
            }
            return new ApiResult();
        }
    }
}