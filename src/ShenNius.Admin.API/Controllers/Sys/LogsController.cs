using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Admin.API.Controllers.Sys
{
    [Authority]
    public class LogsController : ApiControllerBase
    {
        private readonly ILogService _logService;
        public LogsController(ILogService logService)
        {
            _logService = logService;
        }


        [HttpDelete, Authority]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return new ApiResult(await _logService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages(int page, int limit=15,string key = null)
        {
            Expression<Func<Log, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Message.Contains(key);
            }
            var res = await _logService.GetPagesAsync(page, limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet, Authority]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _logService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
    }
}
