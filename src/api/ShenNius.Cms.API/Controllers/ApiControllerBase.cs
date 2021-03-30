using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Cms.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        //private readonly ICtorService<IColumnService> _service;       
        //public ApiControllerBase(ICtorService<IColumnService> service)
        //{
        //    _service = service;
        //}
        //[HttpDelete]
        //public abstract async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        //{
            
        //}

        //[HttpGet]
        //public async Task<ApiResult> GetListPages(int page, string key = null)
        //{
        //    Expression<Func<Column, bool>> whereExpression = null;
        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        whereExpression = d => d.Title.Contains(key);
        //    }
        //    var res = await _columnService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
        //    return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        //}

        //[HttpGet]
        //public async Task<ApiResult> Detail(int id)
        //{
        //    var res = await _columnService.GetModelAsync(d => d.Id == id);
        //    return new ApiResult(data: res);
        //}
        //[HttpPost]
        //public async Task<ApiResult> Add([FromBody] ColumnInput columnInput)
        //{
        //    return await _columnService.AddToUpdateAsync(columnInput);
        //}
        //[HttpPut]
        //public async Task<ApiResult> Modify([FromBody] ColumnModifyInput columnModifyInput)
        //{
        //    return await _columnService.ModifyAsync(columnModifyInput);
        //}

        ///// <summary>
        ///// 所有父栏目
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<ApiResult> GetAllParentColumn()
        //{
        //    var data = await _columnService.GetListAsync(d => d.ParentId == 0);
        //    return new ApiResult(data);
        //}
    }
    public interface ICtorService<T>
    {
        
    }
}
