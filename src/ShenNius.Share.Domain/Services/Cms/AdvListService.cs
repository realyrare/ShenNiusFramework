using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Entity.Sys;
using SqlSugar;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类 名： AdvListService
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 18:10:18
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Cms
{
    public interface IAdvListService : IBaseServer<AdvList>
    {
        Task<ApiResult> GetPagesAsync(KeyListTenantQuery query);
    }
    public class AdvListService : BaseServer<AdvList>, IAdvListService
    {
        public async Task<ApiResult> GetPagesAsync(KeyListTenantQuery query)
        {
            var res = await Db.Queryable<AdvList>().Where(d => d.Status && d.TenantId == query.TenantId)
                .WhereIF(!string.IsNullOrEmpty(query.Key), d => d.Title.Contains(query.Key)).Select(
                d=>new AdvList() {
                    TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == d.TenantId).Select(s => s.Name),
                    Id=d.Id,
                    CreateTime=d.CreateTime,
                    Type=d.Type,
                    ModifyTime=d.ModifyTime,
                    Status=d.Status,
                    Summary=d.Summary,
                    Title=d.Title
                }
                ).ToPageAsync(query.Page,query.Limit);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
    }
}