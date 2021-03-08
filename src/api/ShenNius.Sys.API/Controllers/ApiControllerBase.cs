using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Sys.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        //public async virtual Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        //{
        //    return new ApiResult();

        //}
    }
}
