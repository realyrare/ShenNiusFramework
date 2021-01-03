
using AntDesign.Pro.Layout;

namespace ShenNius.Client.Admin.Constants
{
    public class SystemConstant
    {
        /// <summary>
        /// 本地化浏览器缓存key
        /// </summary>
        public readonly static string BlazorCultureKey = "BlazorCulture";
        /// <summary>
        /// 时间显示格式化
        /// </summary>
        public readonly static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 操作按钮大小
        /// </summary>
        public readonly static string OperationButtonSize = "default";
        
        /// <summary>
        /// 底部友链
        /// </summary>
        public readonly static LinkItem[] FooterLinks =
        {
                new LinkItem{ Key = "Furion", BlankTarget = true, Title = "Furion" ,Href="https://gitee.com/monksoul/Furion"},
                new LinkItem{ Key = "Ant Design", BlankTarget = true, Title = "Ant Design",Href="https://github.com/ant-design-blazor/ant-design-blazor"},
                new LinkItem{ Key = "Gardener", BlankTarget = true, Title = "Gardener",Href="https://gitee.com/hgflydream/Gardener"}
        };

    }
}
