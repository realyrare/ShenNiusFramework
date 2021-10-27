using System.ComponentModel;

/*************************************
* 类名：AdvEnum
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/8 17:13:49
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Enums.Cms
{
    public enum AdvEnum
    {
        /// <summary>
        /// 友情链接
        /// </summary>
        [Description("友情链接")]
        FriendlyLink = 0,
        /// <summary>
        /// 轮播图
        /// </summary>
        [Description("轮播图")]
        Slideshow = 1,
        /// <summary>
        /// 优秀博客
        /// </summary>
        [Description("优秀博客")]
        GoodBlog = 2,
        /// <summary>
        /// 小程序
        /// </summary>
        [Description("小程序")]
        MiniApp = 3
    }
}