/*************************************
* 类名：AppUserInput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/30 16:07:39
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
    public class AppUserInput
    {
        public string NickName { get; set; }
        public byte Gender { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string AvatarUrl { get; set; }
    }
}