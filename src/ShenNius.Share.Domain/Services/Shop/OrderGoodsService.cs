using AutoMapper;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*************************************
* 类名：OrderGoodsService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/20 11:53:40
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Shop
{
    public interface IOrderGoodsService : IBaseServer<OrderGoods>
    {
        Task<ApiResult> ReceiptAsync(int orderId, int appUserId);
        Task<ApiResult> CancelOrderAsync(int orderId, int goodsId, int appUserId);
        Task<ApiResult> GetListAsync(int appUserId, string dataType);
        Task<ApiResult> BuyNowAsync(int goodsId, int goodsNum, string specSkuId, int appUserId);
        Task<ApiResult> CartBuyAsync(int appUserId);
    }
    public class OrderGoodsService : BaseServer<OrderGoods>, IOrderGoodsService
    {
        private readonly IAppUserAddressService _appUserAddressService;
        private readonly IGoodsService _goodsService;
        private readonly IMapper _mapper;
        public OrderGoodsService(IAppUserAddressService appUserAddressService, IGoodsService goodsService, IMapper mapper)
        {
            _appUserAddressService = appUserAddressService;
            _goodsService = goodsService;
            _mapper = mapper;
        }
        public async Task<ApiResult> ReceiptAsync(int orderId, int appUserId)
        {
            await Db.Updateable<Order>().SetColumns(d => new Order() { ReceiptStatus = ReceiptStatusEnum.Received.GetValue<int>(), ReceiptTime = DateTime.Now }).Where(d => d.Id == orderId && d.AppUserId.Equals(appUserId)).ExecuteCommandAsync();
            return new ApiResult();
        }
        public async Task<ApiResult> CancelOrderAsync(int orderId, int goodsId, int appUserId)
        {
            try
            {
                Db.Ado.BeginTran();
                await Db.Deleteable<OrderGoods>().Where(d => d.OrderId == orderId && d.GoodsId == goodsId && d.AppUserId == appUserId).ExecuteCommandAsync();
                var orderGoodsModel = await Db.Queryable<OrderGoods>().Where(d => d.OrderId == orderId && d.AppUserId == appUserId).FirstAsync();
                if (orderGoodsModel == null)
                {
                    await Db.Deleteable<Order>().Where(d => d.AppUserId == appUserId && d.Id == orderId).ExecuteCommandAsync();
                }
                Db.Ado.CommitTran();
                return new ApiResult();
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                return new ApiResult(ex.Message);
            }
        }

        public async Task<ApiResult> GetListAsync(int appUserId, string dataType)
        {
            var data = await Db.Queryable<Order, OrderGoods>((o, od) => new object[] {
                JoinType.Inner,o.Id==od.OrderId,

            }).Where((o, od) => o.AppUserId.Equals(appUserId))
            .WhereIF(dataType == "payment", (o, od) => o.PayStatus == PayStatusEnum.WaitForPay.GetValue<int>())
            .WhereIF(dataType == "delivery", (o, od) => o.DeliveryStatus == DeliveryStatusEnum.WaitForSending.GetValue<int>() && o.PayStatus == PayStatusEnum.Paid.GetValue<int>())
            .WhereIF(dataType == "received", (o, od) => o.ReceiptStatus == ReceiptStatusEnum.WaitForReceiving.GetValue<int>() && o.PayStatus == PayStatusEnum.Paid.GetValue<int>() && o.DeliveryStatus == DeliveryStatusEnum.Sended.GetValue<int>())
            .Select((o, od) => new
            {
                CreateTime = o.CreateTime,
                OrderNo = o.OrderNo,
                TotalPrice = od.TotalPrice,
                TotalNum = od.TotalNum,
                GoodsId = od.GoodsId,
                GoodsName = od.GoodsName,
                GoodsImg = od.ImgUrl,
                GoodsPrice = od.GoodsPrice,
                PayStatus = o.PayStatus,
                DeliveryStatus = o.DeliveryStatus,
                ReceiptStatus = o.ReceiptStatus,
                OrderId = o.Id,
                OrderStatus = o.OrderStatus,
            }).ToListAsync();

            return new ApiResult(data);
        }


        /// <summary>
        /// 立马购买
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsNum"></param>
        /// <param name="specSkuId"></param>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        public async Task<ApiResult> BuyNowAsync(int goodsId, int goodsNum, string specSkuId, int appUserId)
        {
            var goodsData = await _goodsService.GoodInfoIsExist(goodsId, goodsNum, specSkuId, appUserId);
            var addressModel = await _appUserAddressService.GetModelAsync(d => d.AppUserId == appUserId && d.IsDefault == true);

            try
            {
                Db.Ado.BeginTran();
                Order order = new Order();
                order = order.BuildOrder(appUserId);
                order.Id = await Db.Insertable(order).ExecuteReturnIdentityAsync();

                OrderGoods orderGoods = order.BuildOrderGoods(goodsData.Item1, goodsData.Item2, goodsNum);

                await Db.Insertable(orderGoods).ExecuteCommandAsync();

                var orderAddress = order.BuildOrderAddress(addressModel, order.Id);

                await Db.Insertable(orderAddress).ExecuteCommandAsync();
                //订单表统计最终支付的费用
                await Db.Updateable<Order>().SetColumns(d => new Order() { TotalPrice = orderGoods.TotalPrice, PayPrice = orderGoods.TotalPrice }).Where(d => d.Id == order.Id).ExecuteCommandAsync();
                Db.Ado.CommitTran();
                return new ApiResult();
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                return new ApiResult(ex.Message);
            }
        }

        /// <summary>
        /// 购物车结算
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<ApiResult> CartBuyAsync(int appUserId)
        {
            try
            {
                Db.Ado.BeginTran();
                var cartList = await Db.Queryable<Cart>().Where(d => d.AppUserId.Equals(appUserId)).ToListAsync();
                var inCludeGoods = cartList.Select(d => d.GoodsId).ToList();
                var goodsList = await _goodsService.GetListAsync(l => l.Status == true && inCludeGoods.Contains(l.Id));
                var addressModel = await _appUserAddressService.GetModelAsync(d => d.AppUserId == appUserId && d.IsDefault == true);
                Order order = new Order();
                order = order.BuildOrder(appUserId);
                order.Id = await Db.Insertable(order).ExecuteReturnIdentityAsync();

                var orderAddress = order.BuildOrderAddress(addressModel, order.Id);
                await Db.Insertable(orderAddress).ExecuteCommandAsync();

                int cartExistGoodsNum = 0; decimal totalPrice = 0, payPrice = 0;
                List<OrderGoods> orderGoodsList = new List<OrderGoods>();

                foreach (var item in cartList)
                {
                    cartExistGoodsNum = cartList.Where(d => d.GoodsId.Equals(item.Id)).Select(d => d.GoodsNum).FirstOrDefault();
                    var goodsData = await _goodsService.GoodInfoIsExist(item.GoodsId, cartExistGoodsNum, item.SpecSkuId, appUserId);
                    OrderGoods orderGoods = order.BuildOrderGoods(goodsData.Item1, goodsData.Item2, cartExistGoodsNum);
                    totalPrice += goodsData.Item2.GoodsPrice * cartExistGoodsNum;
                    payPrice += goodsData.Item2.GoodsPrice * cartExistGoodsNum;
                    orderGoodsList.Add(orderGoods);
                }

                await Db.Insertable(orderGoodsList).ExecuteCommandAsync();

                //订单表统计最终支付的费用
                await Db.Updateable<Order>().SetColumns(d => new Order() { PayPrice = totalPrice, TotalPrice = payPrice }).Where(d => d.Id == order.Id).ExecuteCommandAsync();
                Db.Ado.CommitTran();
                return new ApiResult();
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                throw ex;
            }
        }
    }
}