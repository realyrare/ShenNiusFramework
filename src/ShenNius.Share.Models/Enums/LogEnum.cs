using System.ComponentModel;

namespace ShenNius.Share.Models.Enums
{
    public enum LogEnum
    {
        /// <summary>
        /// 保存或添加
        /// </summary>
        [Description("保存或添加")]
        Add = 1,

        /// <summary>
        /// 更新
        /// </summary>
        [Description("更新/修改")]
        Update = 2,

        /// <summary>
        /// 更新
        /// </summary>
        [Description("审核")]
        Audit = 3,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 4,

        /// <summary>
        /// 读取/查询
        /// </summary>
        [Description("读取/查询")]
        Read = 5,

        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login = 6,

        /// <summary>
        /// 查看
        /// </summary>
        [Description("查看")]
        Look = 7,

        /// <summary>
        /// 更改状态
        /// </summary>
        [Description("更改状态")]
        Status = 8,

        /// <summary>
        /// 授权
        /// </summary>
        [Description("授权")]
        Authorize = 9,

        /// <summary>
        /// 退出登录
        /// </summary>
        [Description("退出登录")]
        Logout = 10,
    }
}
