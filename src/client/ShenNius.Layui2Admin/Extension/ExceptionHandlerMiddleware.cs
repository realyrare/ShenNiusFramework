using Microsoft.AspNetCore.Http;
using ShenNius.Layui.Admin.Common;
using System;

namespace ShenNius.Layui.Admin.Extension
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void Invoke(HttpContext context)
        {
            try
            {
                 _next(context);
            }
            catch (Exception ex)
            {
                 ExceptionHandler(context, ex.Message);
            }
            finally
            {

                //var statusCode = context.Response.StatusCode;
                //var statusList=new List<int>() { 400, 401, 200, 403 };
                //if (!statusList.Contains(statusCode))
                //{
                //    Enum.TryParse(typeof(HttpStatusCode), statusCode.ToString(), out object message);
                //    await ExceptionHandlerAsync(context, message.ToString());
                //}
            }
        }

        /// <summary>
        /// 异常处理，返回JSON
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private void ExceptionHandler(HttpContext context, string message)
        {
            // context.Response.ContentType = "application/json;charset=utf-8";
            LogHelper.WriteLog(message);
             context.Response.Redirect("/error");
        }
    }
}
