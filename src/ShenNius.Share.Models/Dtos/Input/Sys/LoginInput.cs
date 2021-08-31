namespace ShenNius.Share.Models.Dtos.Input
{
    public class LoginInput
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        public string NumberGuid { get; set; }
        /// <summary>
        /// 确定再次登录
        /// </summary>
        public bool ConfirmLogin { get; set; }
    }
}
