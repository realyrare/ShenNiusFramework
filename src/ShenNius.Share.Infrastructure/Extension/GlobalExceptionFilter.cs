using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

using ShenNius.Share.Infrastructure.ApiResponse;

using NLog;
using Microsoft.Extensions.Logging;

namespace ShenNius.Share.Infrastructure.Extension
{
    /// <summary>
    /// Http全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment webHostEnvironment, ILogger<GlobalExceptionFilter> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new ApiResult { StatusCode = 500, Success = false, Msg = context.Exception.Message };
            var errorAudit = "Unable to resolve service for";
            if (!string.IsNullOrEmpty(json.Msg) && json.Msg.Contains(errorAudit))
            {
                json.Msg = json.Msg.Replace(errorAudit, $"（若新添加服务，需要重新编译项目）{errorAudit}");
            }

            if (_webHostEnvironment.IsDevelopment())
                json.Msg = context.Exception.StackTrace;//显示堆栈信息

            context.Result = new InternalServerErrorObjectResult(json);

            //采用NLog 记录错误日志
            _logger.LogError(json.Msg + "\r\n" + context.Exception.StackTrace);
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
