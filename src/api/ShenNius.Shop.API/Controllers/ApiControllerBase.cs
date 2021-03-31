using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Common;
using ShenNius.Share.Service.Repository;
using System.Threading.Tasks;

namespace ShenNius.Shop.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ApiControllerBase<TEntity, TDetailQuery, TDeleteInput, TListQuery, TCreateInput, TUpdateInput> : ControllerBase
        where TEntity : BaseEntity, new()
        where TDeleteInput : CommonDeleteInput
        where TDetailQuery : DetailQuery
        where TListQuery : ListQuery
    {
        private readonly IBaseServer<TEntity> _service;
        private readonly IMapper _mapper;

        public ApiControllerBase(IBaseServer<TEntity> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        [HttpDelete]
        public virtual async Task<ApiResult> Deletes([FromBody] TDeleteInput commonDeleteInput)
        {
            var res = await _service.DeleteAsync(commonDeleteInput.Ids);
            if (res <= 0)
            {
                throw new FriendlyException("删除失败了！");
            }
            return new ApiResult();
        }

        [HttpGet]
        public virtual async Task<ApiResult> GetListPages([FromQuery] TListQuery listQuery)
        {
            var res = await _service.GetPagesAsync(listQuery.Page, listQuery.Limit);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet]
        public virtual async Task<ApiResult> Detail([FromQuery] TDetailQuery detailQuery)
        {
            var res = await _service.GetModelAsync(d => d.Id == detailQuery.Id);
            return new ApiResult(data: res);
        }
        [HttpPost]
        public virtual async Task<ApiResult> Add([FromBody] TCreateInput createInput)
        {
            var entity = _mapper.Map<TEntity>(createInput);
            var res = await _service.AddAsync(entity);
            if (res <= 0)
            {
                throw new FriendlyException("添加失败了！");
            }
            return new ApiResult(data: res);

        }
        [HttpPut]
        public virtual async Task<ApiResult> Modify([FromBody] TUpdateInput updateInput)
        {
            var entity = _mapper.Map<TEntity>(updateInput);
            var res = await _service.UpdateAsync(entity);
            if (res <= 0)
            {
                throw new FriendlyException("修改失败了！");
            }
            return new ApiResult(data: res);
        }
    }
}
