namespace ShenNius.Share.Infrastructure.Caches
{
    /// <summary>
    /// 全局缓存key
    /// </summary>
    public class KeyHelper
    {
        public class Cms
        {
            /// <summary>
            /// 当前站点key
            /// </summary>
            public const string CurrentTenant = "currentTenant";
        }
        public class User
        {
            /// <summary>
            /// 用户登录非对称加密
            /// </summary>
            public const string loginRSACrypt = "loginRSACrypt";
           /// <summary>
           /// 当前用户拥有的所有权限去做校验
           /// </summary>
            public const string AuthMenu = "authMenu";

            public const string LoginKey = "loginKey";

        }
    }
}
