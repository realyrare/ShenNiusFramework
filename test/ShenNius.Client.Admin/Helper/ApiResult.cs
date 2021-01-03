using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Client.Admin.Helper
{
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int statusCode { get; set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; }
    }
    public class Page<T> 
    {
        public int count { get;  set; }
        public List<T> item { get; set; }
    }
}
