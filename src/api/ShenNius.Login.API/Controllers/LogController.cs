using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Service.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShenNius.Sys.API.Controllers
{
   public class LogController : ApiControllerBase
    {
        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpDelete]
        public async Task<ApiResult> Deletes(List<string> ids)
        {
            if (ids.Count<=0||ids==null)
            {
                throw new ArgumentException(nameof(ids));
            }
          return new ApiResult( await _logService.DeleteAsync(ids));
        }

        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key)
        {
            var res = await _logService.GetPagesAsync(page, 15,d=>d.Exception.Contains(key),d=>d.Id,false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _logService.GetModelAsync(d=>d.Id==id);
            return new ApiResult(data: res);
        }
    }
}
