using ShenNius.Share.Infrastructure.Extension;
using System;
using System.ComponentModel;

/*************************************
* 类名：EnumExtension
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/8 17:37:23
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Utils
{
    public static class EnumExtension
    {
        public static string GetEnumText(this Enum value)
        {
            if (value == null)
            {
                throw new FriendlyException("value");
            }
            var text = value.ToString();
            var fi = value.GetType().GetField(text);
            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    text = attributes[0].Description;
                }
                else
                {
                    text = string.Empty;
                }
            }
            else
            {
                text = string.Empty;
            }
            return text;
        }
    }
}