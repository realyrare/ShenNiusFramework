using ShenNius.Share.Models.Dtos.Common;

/*************************************
* 类名：SpecInput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/12 17:25:37
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
    public class SpecInput : GlobalTenantInput
    {
        public string SpecName { get; set; }
        public string SpecValue { get; set; }
    }
    /// <summary>
    /// 根据规格组id添加规格值
    /// </summary>
    public class SpecValuesInput : GlobalTenantInput
    {
        public int SpecId { get; set; }
        public string SpecValue { get; set; }
    }

}