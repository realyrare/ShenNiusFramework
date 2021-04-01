using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Common;
using ShenNius.Share.Service.Repository;
using System.Threading.Tasks;

/*************************************
* 类名：ApiBaseController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/1 10:28:20
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.BaseController.Controllers
{
    /// <summary>
    /// 适用于不是多租户的模块使用
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDetailQuery"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TListQuery"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public abstract class ApiBaseController<TEntity, TDetailQuery, TDeleteInput, TListQuery, TCreateInput, TUpdateInput> : ControllerBase
       where TEntity : BaseEntity, new()
       where TDeleteInput : DeletesInput
       where TDetailQuery : DetailQuery
       where TListQuery : PageQuery
    {
        private readonly IBaseServer<TEntity> _service;
        private readonly IMapper _mapper;

        public ApiBaseController(IBaseServer<TEntity> service, IMapper mapper)
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
            var res = await _service.GetPagesAsync(listQuery.Page, listQuery.Limit, d => d.Id, false);
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