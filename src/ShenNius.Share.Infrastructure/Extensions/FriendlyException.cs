using System;

namespace ShenNius.Share.Infrastructure.Extensions
{
    public class FriendlyException : Exception
    {
        /// <summary>
        /// Status code<br/>
        /// 状态码<br/>
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Initialize<br/>
        /// 初始化<br/>
        /// </summary>
        /// <param name="statusCode">Status code</param>
        /// <param name="message">Exception message</param>
        public FriendlyException(string message = null, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
