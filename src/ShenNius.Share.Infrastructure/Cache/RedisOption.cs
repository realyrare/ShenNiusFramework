using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：RedisOption
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 18:52:47
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Cache
{
    public class RedisOption
    {
        public bool Enable { get; set; }
        public string Connection { get; set; }
        public string InstanceName { get; set; }
        public int Database { get; set; }
    }
}