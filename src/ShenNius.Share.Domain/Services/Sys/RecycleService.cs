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
        private readonly IHttpContextAccessor _accessor;

        public RecycleService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;


        }
        [Transaction]
        public async Task<ApiResult> RestoreAsync(DeletesInput input)
        {
            //先删除后还原  要启用事务
            foreach (var item in input.Ids)
            {
                var model = await GetModelAsync(d => d.Id == item);
                if (model != null)
                {
                    await RestoreDataAsync(model.TableType, model.BusinessId);
                }
                await DeleteAsync(d => d.Id == item);
            }
            return new ApiResult();
        }
     
        private Task RestoreDataAsync(string tableType, int businessId)
        {          
            //后面优化 ， 目前没想到什么好的办法
            if (tableType.Contains(nameof(Article)))
            {
                var service = _accessor.HttpContext.RequestServices.GetService(typeof(IBaseServer<Article>)) as IBaseServer<Article>;
                service.UpdateAsync(d => new Article() { Status = true }, d => d.Id == businessId && d.Status == false);
            }
            if (tableType.Contains(nameof(Column)))
            {
                var service = _accessor.HttpContext.RequestServices.GetService(typeof(IBaseServer<Column>)) as IBaseServer<Column>;
                service.UpdateAsync(d => new Column() { Status = true }, d => d.Id == businessId && d.Status == false);
            }
            if (tableType.Contains(nameof(AdvList)))
            {
                var service = _accessor.HttpContext.RequestServices.GetService(typeof(IBaseServer<AdvList>)) as IBaseServer<AdvList>;
                service.UpdateAsync(d => new AdvList() { Status = true }, d => d.Id == businessId && d.Status == false);
            }
            return Task.FromResult(0);
        }
    }   
}