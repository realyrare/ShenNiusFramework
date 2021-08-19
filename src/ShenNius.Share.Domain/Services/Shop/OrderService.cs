using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Output.Shop;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Entity.Sys;
using SqlSugar;
using System.Threading.Tasks;

/*************************************
* 类名：OrderService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/19 17:12:16
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Shop
{
    public interface IOrderService : IBaseServer<Order>
    {
        Task<ApiResult> GetListPageAsync(KeyListTenantQuery query);
    }
    public class OrderService: BaseServer<Order>, IOrderService
    {
        public async Task<ApiResult> GetListPageAsync(KeyListTenantQuery query)
        {
            var data = await Db.Queryable<Order, OrderGoods, AppUser>((o, og, u) => new object[] { JoinType.Inner,o.Id==og.OrderId,
                JoinType.Inner,og.AppUserId==u.Id,
            })
                  .OrderBy((o, og, u) => o.Id, OrderByType.Desc)
                  .Select((o, og, u) => new OrderListOutput()
                  {
                      GoodsName = og.GoodsName,
                      GoodsId = og.GoodsId,
                      CreateTime = o.CreateTime,
                      ImgUrl = og.ImgUrl,
                      OrderNo = o.OrderNo,
                      OrderId = o.Id,
                      AppUserName = u.NickName,
                      AppUserId = u.Id,
                      OrderStatus = o.OrderStatus,
                      PayStatus = o.PayStatus,
                      DeliveryStatus = o.DeliveryStatus,
                      ReceiptStatus = o.ReceiptStatus,
                      TotalNum = og.TotalNum,
                      GoodsPrice = og.GoodsPrice,
                      TotalPrice = og.TotalPrice,
                      TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == o.TenantId).Select(s => s.Name),
                  }).ToPageAsync(query.Page, query.Limit);
            foreach (var item in data.Items)
            {
                if (!string.IsNullOrEmpty(item.ImgUrl))
                {
                    var imgArry = item.ImgUrl.Split(',');
                    if (imgArry.Length > 0)
                    {
                        item.ImgUrl = imgArry[0];
                    }
                }
            }
            return new ApiResult(data);
        }
        public async Task<ApiResult<OrderDetailOutput>> GetOrderDetailAsync(int orderId)
        {
            var model = await Db.Queryable<Order, AppUser>((o, u) => new object[] {
                JoinType.Inner,o.AppUserId==u.Id,
            })
              .Where((o, u) => o.Id == orderId)
             .Select((o, u) => new OrderDetailOutput()
             {
                 OrderNo = o.OrderNo,
                 Id = o.Id,
                 AppUserName = u.NickName,
                 AppUserId = u.Id,
                 AddressId=u.AddressId,
                 OrderStatus = o.OrderStatus,
                 PayStatus = o.PayStatus,
                 DeliveryStatus = o.DeliveryStatus,
                 ReceiptStatus = o.ReceiptStatus,
                 TransactionId = o.TransactionId,
                 ExpressCompany = o.ExpressCompany,
                 ExpressPrice = o.ExpressPrice,
                 ExpressNo = o.ExpressNo,
                 DeliveryTime = o.DeliveryTime,
                 ReceiptTime = o.ReceiptTime,
                 AllPayPrice = o.AllPayPrice,
                 AllTotalPrice = o.AllTotalPrice,
                 CreateTime = o.CreateTime,
                 TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == o.TenantId).Select(s => s.Name),
             }).FirstAsync();
            if (model == null)
            {
                throw new FriendlyException($"订单详情实体数据为空！");
            }
            //TODO  地址待调整确定
            model.Address = await Db.Queryable<AppUserAddress>().Where(oa => oa.Id == model.AddressId)
               .FirstAsync();

            model.GoodsDetailList = await Db.Queryable<OrderGoods>().Where(d => d.OrderId == orderId).Select(d => new OrderGoodsDetailOutput
            {
                GoodsImg = d.ImgUrl,
                GoodsName = d.GoodsName,
                GoodsId = d.GoodsId,
                GoodsPrice = d.GoodsPrice,
                GoodsWeight = d.GoodsWeight,
                TotalNum = d.TotalNum,
                TotalPrice = d.TotalPrice,
                CreateTime = d.CreateTime,
                GoodsNo = d.GoodsNo
            }).ToListAsync();

            return new ApiResult<OrderDetailOutput>(model);
        }
    }


}