using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

/*************************************
* 类名：EnumExtension
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/3 16:52:46
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Enums.Extension
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获得枚举字段的名称。
        /// </summary>
        /// <returns></returns>
        public static string GetName(this Enum thisValue)
        {
            return Enum.GetName(thisValue.GetType(), thisValue);
        }
        /// <summary>
        /// 获得枚举字段的值。
        /// </summary>
        /// <returns></returns>
        public static T GetValue<T>(this Enum thisValue)
        {
            return (T)Enum.Parse(thisValue.GetType(), thisValue.ToString());
        }
        public static string GetEnumText(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
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