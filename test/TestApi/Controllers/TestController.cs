using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShenNius.Share.Models.Configs;
using TestApi.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;
        private readonly IMediator _mediator;

        public TestController(ILogger<TestController> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }
        /// <summary>
        /// 测试通过 没问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Register()
        {
            NewUser user = new NewUser() { Password = "2323", Username = "mhg" };
            var result =await _mediator.Send(user);

          
            return Ok();
        }
        /// <summary>
        /// 测试通过 没问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register2()
        {
            NewUser2 user = new NewUser2() { Password = "2323", Username = "mhg" };
            _mediator.Publish(user);
            return Ok();
        }
        [HttpGet]
        public IEnumerable<WeatherForecast> Get1()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Authorize]
        public ApiResult Get2()
        {        
            return new ApiResult("测试内容信息");
        }
    }
}
