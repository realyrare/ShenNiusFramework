using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Cms;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：ColumnController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:24:30
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{
    public class ColumnController : ApiControllerBase
    {
        private readonly IColumnService _columnService;
        private readonly IMapper _mapper;

        public ColumnController(IColumnService columnService, IMapper mapper)
        {
            _columnService = columnService;
            this._mapper = mapper;
        }
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult(await _columnService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<Column, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Title.Contains(key);
            }
            var res = await _columnService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _columnService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpPost]
        [MultiTenant]
        public async Task<ApiResult> Add([FromBody] ColumnInput columnInput )
        {
         return await _columnService.AddToUpdateAsync(columnInput);
        }
        [HttpPut]
        [MultiTenant]
        public async Task<ApiResult> Modify([FromBody] ColumnModifyInput columnModifyInput)
        {
            return await _columnService.ModifyAsync(columnModifyInput);     
        }

        /// <summary>
        /// 所有父栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAllParentColumn()
        {
            var data= await _columnService.GetListAsync(d =>d.ParentId==0);
            return new ApiResult(data);
        }
    }
}