using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：AddressInput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/27 15:44:46
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
    public class AddressInput
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string Detail { get; set; }
        public int AddressId { get; set; }
    }
}