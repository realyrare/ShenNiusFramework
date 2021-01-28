namespace ShenNius.Share.Infrastructure.ApiResponse
{
    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; private set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; private set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get;private set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public dynamic Data { get; private set; } 
        public ApiResult(dynamic data = null, int statusCode = 200, bool success = true, string msg = "操作成功")
        {
            Data = data;
            StatusCode = statusCode;
            Success = success;
            Msg = msg;
        }
    }
    public class ApiResult<T> where T:class
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; private set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; private set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; private set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Data { get; private set; }
        public ApiResult(T data=null, int statusCode=200,bool success=true,string msg="操作成功")
        {
            Data = data;
            StatusCode = statusCode;
            Success = success;
            Msg = msg;
        }
        public ApiResult(string msg = "服务器内部错误",int statusCode = 500)
        {
            Data = default;
            StatusCode = statusCode;
            Success = false;
            Msg = msg;
        }
        //public ApiResult(string msg =null, int statusCode = 400, bool success = false)
        //{
        //    Data = default;
        //    StatusCode = statusCode;
        //    Success = success;
        //    Msg = msg;
        //}
    }
}
