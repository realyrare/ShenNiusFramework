using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using System.Threading.Tasks;

/*************************************
* 类名：RecycleController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/8 19:24:46
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Sys.API.Controllers
{
    /// <summary>
    /// 回收站
    /// </summary>
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
        public  Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return  _recycleService.RealyDeleteAsync(commonDeleteInput);
        }

        [HttpGet]
        public  Task<ApiResult> GetListPages([FromQuery] KeyListQuery query)
        {           
           return _recycleService.GetPagesAsync(query);          
        }

        /// <summary>
        /// 数据还原
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResult> Restore([FromBody] DeletesInput input)
        {
            return _recycleService.RestoreAsync(input);
        }

    }
}