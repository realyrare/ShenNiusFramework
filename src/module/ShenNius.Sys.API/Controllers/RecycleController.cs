using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：RecycleController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/8 19:24:46
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Sys.API.Controllers
{
    public class RecycleController : ApiControllerBase
    {
        private readonly IRecycleService _recycleService;
        public RecycleController(IRecycleService recycleService)
        {
            _recycleService = recycleService;
        }
        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="commonDeleteInput"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return new ApiResult(await _recycleService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<Recycle, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Remark.Contains(key);
            }
            var res = await _recycleService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpPost]
        public async Task<ApiResult> Restore([FromBody] DeletesInput input)
        {
            //先删除后还原
            foreach (var item in input.Ids)
            {
                var model = await _recycleService.GetModelAsync(d => d.Id == item);
                if (model != null)
                {
                    var entity = "I" + model.TableType + "Service";
                    entity.GetType();
                    //发布订阅
                    // var s= typeof(model.TableType) ;
                    //根据实体名称创建对应的服务实例，然后去还原。
                }
                await _recycleService.DeleteAsync(d=>d.Id==item);               
            }                     
            return new ApiResult();
        }
    }
}