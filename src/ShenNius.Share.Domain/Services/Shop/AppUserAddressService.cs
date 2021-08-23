using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Output.Shop;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Entity.Sys;
using SqlSugar;
using System.Threading.Tasks;

/*************************************
* 类名：OrderGoodsService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/20 11:53:40
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Shop
{
    public interface IAppUserAddressService : IBaseServer<AppUserAddress>
    {
        Task<ApiResult> GetListPageAsync(KeyListTenantQuery query);
    }
    public class AppUserAddressService : BaseServer<AppUserAddress>, IAppUserAddressService
    {
        public async Task<ApiResult> GetListPageAsync(KeyListTenantQuery query)
        {
            var data = await Db.Queryable<AppUserAddress,  AppUser>((d,  u) => new object[] { JoinType.Inner,d.Id==u.AddressId&&d.Status
            }).Where((d, u)=>d.TenantId==query.TenantId).WhereIF(!string.IsNullOrEmpty(query.Key),(d,u)=>d.Name.Equals(query.Key))
                .OrderBy((d, u) => d.Id, OrderByType.Desc)
                .Select((d, u) => new AppUserAddressOutput()
                {
                    Name=d.Name,
                    Province=d.Province,
                    City=d.City,
                    Detail=d.Detail,
                    Region=d.Region,
                    CreateTime=d.CreateTime,
                    AppUserName=u.NickName,
                    TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == d.TenantId).Select(s => s.Name),
                }).ToPageAsync(query.Page, query.Limit);
            return new ApiResult(data);
        }
    }
}