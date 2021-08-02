using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：StaticExtension
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/2 18:06:12
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Utils
{
    public static class StaticExtension
    {
        public static string ToWebString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string TrimEnd(this string sourceStr, string removeStr)
        {
            if (string.IsNullOrEmpty(sourceStr) || !sourceStr.EndsWith(removeStr))
            {
                return sourceStr;
            }
            return sourceStr.Substring(0, sourceStr.Length - removeStr.Length);
        }
    }
}