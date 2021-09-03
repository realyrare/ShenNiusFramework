using Newtonsoft.Json;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Output.Shop;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
using SqlSugar;
using System;
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
        Task<ApiResult> BuyNowAsync(int goodsId, int goodsNum, int goodsSkuId, int appUserId);
    }
    public class OrderGoodsService : BaseServer<OrderGoods>, IOrderGoodsService
    {
        private readonly IAppUserAddressService _appUserAddressService;
        private readonly IGoodsService _goodsService;

        public OrderGoodsService(IAppUserAddressService appUserAddressService,IGoodsService goodsService )
        {
            this._appUserAddressService = appUserAddressService;
            this._goodsService = goodsService;
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

        public async Task<ApiResult> GetListAsync(int appUserId ,string dataType)
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
        /// TODO 需要修改
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsNum"></param>
        /// <param name="goodsSkuId"></param>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        public async Task<ApiResult> BuyNowAsync(int goodsId, int goodsNum, int goodsSkuId, int appUserId)
        {
           // var goodsModel = await Db.Queryable<Goods>().Where(d => d.Status == true &&d.GoodsStatus==GoodsStatusEnum.PutAway.GetValue<int>() && d.Id.Equals(goodsId)).FirstAsync();
            var goodsModel = await _goodsService.DetailAsync(goodsId);
            var addressModel = await _appUserAddressService.GetModelAsync(d =>d.AppUserId == appUserId && d.IsDefault == true);
            if (goodsModel == null)
            {
                throw new ArgumentNullException("商品实体信息为空！");
            }
            try
            {
                Db.Ado.BeginTran();
                Order order = new Order()
                {
                    AppUserId = appUserId,
                    AppUserAddressId = addressModel.Id,
                    OrderNo = DateTime.Now.ToString("yyyyMMddss") + new Random().Next(1, 9999).GetHashCode(),
                    OrderStatus = OrderStatusEnum.NewOrder.GetValue<int>(),
                    PayStatus = PayStatusEnum.WaitForPay.GetValue<int>(),
                    DeliveryStatus = DeliveryStatusEnum.WaitForSending.GetValue<int>(),
                    //DeliveryTime = DateTime.Now,
                    CreateTime = DateTime.Now
                };
                var orderId = await Db.Insertable(order).ExecuteReturnIdentityAsync();

                OrderGoods orderGoods = new OrderGoods()
                {
                    GoodsId = goodsId,
                    GoodsName = goodsModel.Data.Name,
                    GoodsPrice = goodsModel.Data.GoodsSpecInput.GoodsPrice,
                    LinePrice = goodsModel.Data.GoodsSpecInput.LinePrice,
                    CreateTime = DateTime.Now,
                    GoodsNo = goodsModel.Data.GoodsSpecInput.GoodsNo,
                    Content = goodsModel.Data.Content,
                    ImgUrl = goodsModel.Data.ImgUrl,
                    AppUserId = appUserId,
                    TotalNum = goodsNum,
                    TotalPrice = goodsModel.Data.GoodsSpecInput.GoodsPrice * goodsNum,
                    //PayPrice = goodsModel.Data.GoodsSpecInput.GoodsPrice * goodsNum,
                    SpecType = goodsModel.Data.SpecType,
                    GoodsAttr = JsonConvert.SerializeObject(goodsModel.Data.SpecMany),
                    OrderId = orderId
                };
                await Db.Insertable(orderGoods).ExecuteCommandAsync();
                //订单表统计最终支付的费用
                await Db.Updateable<Order>().SetColumns(d => new Order() { TotalPrice = orderGoods.TotalPrice, PayPrice= orderGoods.TotalPrice }).Where(d => d.Id == orderId).ExecuteCommandAsync();
                Db.Ado.CommitTran();
                return new ApiResult();
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                return new ApiResult(ex.Message);
            }
        }
    }
}