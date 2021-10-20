using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Admin.API.Controllers.Sys
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        
    }
}
