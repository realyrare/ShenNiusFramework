using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Input.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShenNius.Sys.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        public async virtual Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult();
        }
    }
}
