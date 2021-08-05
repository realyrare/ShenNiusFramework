namespace ShenNius.Share.Models.Configs
{
    /// <summary>
    /// 七牛云配置信息实体
    /// </summary>
    public class QiNiuOss
    {
        public string Ak { get; set; }
        public string Sk { get; set; }
        public string Bucket { get; set; }
        public string BasePath { get; set; }
        public string ImgDomain { get; set; }
    }
}
