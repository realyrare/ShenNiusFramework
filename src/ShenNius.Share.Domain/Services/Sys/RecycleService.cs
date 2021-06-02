using Microsoft.AspNetCore.Http;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Entity.Sys;
using System.Threading.Tasks;

/*************************************
* 类名：RecycleService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/8 18:56:27
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IRecycleService : IBaseServer<Recycle>
    {
        Task<ApiResult> RestoreAsync(DeletesInput deletesInput);
    }
    public class RecycleService : BaseServer<Recycle>, IRecycleService
    {
        [Transaction]
        public async Task<ApiResult> RestoreAsync(DeletesInput input)
        {
            //先删除后还原  要启用事务
            foreach (var item in input.Ids)
            {
                var model = await GetModelAsync(d => d.Id == item);
                if (model != null)
                {
                  var i=await Db.Ado.ExecuteCommandAsync(model.Sql);
                    if (i>0)
                    {
                        await DeleteAsync(d => d.Id == item);
                    }
                   // await RestoreDataAsync(model.TableType, model.BusinessId);
                }              
            }
            return new ApiResult();
        }
    }   
}