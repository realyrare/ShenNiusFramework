using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Service.Sys;
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
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {         
          return new ApiResult( await _logService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet, Authority(Module = "log")]
        public async Task<ApiResult> GetListPages(int page, string key=null)
        {            
            var res = await _logService.GetPagesAsync(page, 15);
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
