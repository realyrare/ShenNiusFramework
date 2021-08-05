namespace ShenNius.Share.Models.Configs
{
    /// <summary>
    /// jwt实体类
    /// </summary>
    public class JwtSetting
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecurityKey { get; set; }

        public int ExpireSeconds { get; set; }
    }
}
