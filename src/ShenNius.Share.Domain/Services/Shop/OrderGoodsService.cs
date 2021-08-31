using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
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
    }
    public class OrderGoodsService : BaseServer<OrderGoods>, IOrderGoodsService
    {
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
    }
}