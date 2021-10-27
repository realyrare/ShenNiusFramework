/*************************************
* 类名：AppUserLoginInput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/27 16:40:57
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
    public class AppUserLoginInput
    {
        public int TenantId { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }

        public string UserInfo { get; set; }

        public string EncryptedData { get; set; }

        public string Iv { get; set; }

        public string Signature { get; set; }
    }
}