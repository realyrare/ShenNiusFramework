using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
using System;

/*************************************
* 类名：OrderListOutput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/19 17:16:11
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Output.Shop
{
    public class OrderListOutput
    {
        public decimal GoodsPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateTime { get; set; }
        public string OrderNo { get; set; }
        public string ImgUrl { get; set; }
        public string GoodsName { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int TotalNum { get; set; }
        public string AppUserName { get; set; }
        public int AppUserId { get; set; }
        public int PayStatus { get; set; }
        public string PayStatusText
        {
            get
            {
                string name = "";
                if (PayStatus == PayStatusEnum.WaitForPay.GetValue<int>())
                {
                    name = PayStatusEnum.WaitForPay.GetEnumText();
                }
                if (PayStatus == PayStatusEnum.Paid.GetValue<int>())
                {
                    name = PayStatusEnum.Paid.GetEnumText();
                }
                return name;
            }
        }
        public int DeliveryStatus { get; set; }
        public string DeliveryStatusText
        {
            get
            {
                string name = "";
                if (DeliveryStatus == DeliveryStatusEnum.WaitForSending.GetValue<int>())
                {
                    name = DeliveryStatusEnum.WaitForSending.GetEnumText();
                }
                if (DeliveryStatus == DeliveryStatusEnum.Sended.GetValue<int>())
                {
                    name = DeliveryStatusEnum.Sended.GetEnumText();
                }
                return name;
            }
        }
        public int ReceiptStatus { get; set; }
        public string ReceiptStatusText
        {
            get
            {
                string name = "";
                if (ReceiptStatus == ReceiptStatusEnum.WaitForReceiving.GetValue<int>())
                {
                    name = ReceiptStatusEnum.WaitForReceiving.GetEnumText();
                }
                if (ReceiptStatus == ReceiptStatusEnum.Received.GetValue<int>())
                {
                    name = ReceiptStatusEnum.Received.GetEnumText();
                }
                return name;
            }
        }
        public int OrderStatus { get; set; }
        public string OrderStatusText
        {
            get
            {
                string name = "";
                if (ReceiptStatus == OrderStatusEnum.NewOrder.GetValue<int>())
                {
                    name = OrderStatusEnum.NewOrder.GetEnumText();
                }
                if (ReceiptStatus == OrderStatusEnum.Canceled.GetValue<int>())
                {
                    name = OrderStatusEnum.Canceled.GetEnumText();
                }
                if (ReceiptStatus == OrderStatusEnum.Completed.GetValue<int>())
                {
                    name = OrderStatusEnum.Completed.GetEnumText();
                }
                return name;
            }
        }
        public int OrderId { get; set; }
        public int GoodsId { get; set; }

        public string TenantName { get; set; }
    }
}