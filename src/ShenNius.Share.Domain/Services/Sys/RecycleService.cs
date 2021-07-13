using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Input.Sys;
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
        Task<ApiResult> RealyDeleteAsync(DeletesInput deletesInput);
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
                  var i=await Db.Ado.ExecuteCommandAsync(model.RestoreSql);
                    if (i>0)
                    {
                        //把回收站的记录清空
                        await DeleteAsync(d => d.Id == item);
                    }
                }              
            }
            return new ApiResult();
        }
        [Transaction]
        public async Task<ApiResult> RealyDeleteAsync(DeletesInput input)
        {
           var list= await GetListAsync(d => input.Ids.Contains(d.Id));
            if (list == null||list.Count<=0)
            {
                throw new FriendlyException($"{nameof(input.Ids)}:中参数没有匹配到任何数据记录");
            }
            foreach (var item in list)
            {
                if (item != null)
                {
                    var i = await Db.Ado.ExecuteCommandAsync(item.RealyDelSql);
                    if (i > 0)
                    {
                        //把回收站的记录清空
                        await DeleteAsync(d => d.Id == item.Id);
                    }
                }
            }
            return new ApiResult();
        }
    }   
}