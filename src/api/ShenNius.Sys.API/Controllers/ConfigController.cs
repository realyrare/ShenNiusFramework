using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Service.Sys;
using System;
using System.Threading.Tasks;

namespace ShenNius.Sys.API.Controllers
{
    public  class ConfigController : ApiControllerBase
    {
        private readonly IConfigService _configService;
        private readonly IMapper _mapper;

        public ConfigController(IConfigService configService, IMapper mapper)
        {
            _configService = configService;
            _mapper = mapper;
        }
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult(await _configService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            var res = await _configService.GetPagesAsync(page, 15);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _configService.GetModelAsync(d=>d.Id==id);
            return new ApiResult(data: res);
        }

        [HttpPost]
        public async Task<ApiResult> Add([FromBody] ConfigInput input)
        {
            var model = _mapper.Map<Config>(input);
            var res = await _configService.AddAsync(model);
            return new ApiResult(data: res);
        }
        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] ConfigModifyInput input)
        {           
            var res = await _configService.UpdateAsync(d=>new Config() {Name=input.Name,EnName=input.EnName,Type=input.Type,
                UpdateTime=DateTime.Now,
                Summary=input.Summary
            },d=>d.Id==input.Id);
            return new ApiResult(data: res);
        }
    }
}
