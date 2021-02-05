namespace ShenNius.Client.Admin.Model
{
    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get;  set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get;  set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public dynamic Data { get;  set; } 
        /// <summary>
        /// get请求成功，直接传递数据
        /// </summary>
        /// <param name="data">数据</param>
        public ApiResult(dynamic data)
        {
            Data = data;
            StatusCode = 200;
            Success = true;
            Msg = "操作成功";
        }
        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="msg">错误消息</param>
        /// <param name="statusCode">错误码</param>
        public ApiResult(string msg = "服务器内部错误", int statusCode = 500)
        {
            StatusCode = statusCode;
            Success = false;
            Msg = msg;
        }
    }
    public class ApiResult<T> where T:class
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get;  set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get;  set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Data { get;  set; }
          
    }
}
