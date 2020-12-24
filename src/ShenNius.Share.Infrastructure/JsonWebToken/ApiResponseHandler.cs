using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShenNius.Infrastructure.ApiResponse;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ShenNius.Infrastructure.JsonWebToken
{
    public class ApiResponseHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private JsonSerializerSettings setting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        public ApiResponseHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult<string>() { StatusCode= StatusCodes.Status401Unauthorized ,Msg= "很抱歉，您无权访问该接口，请确保已经登录!",Success=false}, setting));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult<string>() { StatusCode = StatusCodes.Status403Forbidden, Msg = "很抱歉，您的访问权限等级不够，联系管理员!", Data = "", Success = false }, setting));
        }

    }
}
