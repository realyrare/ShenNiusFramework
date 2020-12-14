using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Models.ViewModels.Response
{
   public class ApiResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; } = 200;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Data { get; set; }
    }
}
