using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
