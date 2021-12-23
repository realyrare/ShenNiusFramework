using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Infrastructure.Configurations;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Common;
using ShenNius.Share.Models.Entity.Sys;
using SqlSugar;
using System;
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
        Task<ApiResult> SoftDeleteAsync<TEntity>(DeletesTenantInput input, IBaseServer<TEntity> service) where TEntity : BaseTenantEntity, new();
        Task<ApiResult> GetPagesAsync(KeyListQuery query);
    }
    public class RecycleService : BaseServer<Recycle>, IRecycleService
    {
        private readonly ICurrentUserContext _currentUserContext;

        public RecycleService(ICurrentUserContext currentUserContext)
        {
            _currentUserContext = currentUserContext;
        }
        public async Task<ApiResult> RestoreAsync(DeletesInput input)
        {
            try
            {
                Db.BeginTran();
                //先删除后还原  要启用事务
                foreach (var item in input.Ids)
                {
                    var model = await GetModelAsync(d => d.Id == item);
                    if (model != null)
                    {
                        if (string.IsNullOrEmpty(model.RestoreSql))
                        {
                            throw new FriendlyException($"根据传递的【{item}】参数查出来该条数据不存在！");
                        }
                        var i = await Db.Ado.ExecuteCommandAsync(model.RestoreSql);
                        if (i > 0)
                        {
                            //把回收站的记录清空
                            await DeleteAsync(d => d.Id == item);
                        }
                    }
                }
                Db.CommitTran();
            }
            catch (Exception e)
            {
                Db.RollbackTran();
                return new ApiResult(e.Message, 500);
            }
            return new ApiResult();
        }

        public async Task<ApiResult> RealyDeleteAsync(DeletesInput input)
        {
            var list = await GetListAsync(d => input.Ids.Contains(d.Id));
            if (list == null || list.Count <= 0)
            {
                throw new FriendlyException($"{nameof(input.Ids)}:中参数没有匹配到任何数据记录");
            }
            try
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        if (string.IsNullOrEmpty(item.RealyDelSql))
                        {
                            throw new FriendlyException($"根据传递的【{item}】参数查出来该条数据不存在！");
                        }
                        var i = await Db.Ado.ExecuteCommandAsync(item.RealyDelSql);
                        if (i > 0)
                        {
                            //把回收站的记录清空
                            await DeleteAsync(d => d.Id == item.Id);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, 500);
            }
            return new ApiResult();
        }

        public async Task<ApiResult> SoftDeleteAsync<TEntity>(DeletesTenantInput input, IBaseServer<TEntity> service) where TEntity : BaseTenantEntity, new()
        {
            var userId = _currentUserContext.Id;
            var allTable = AppSettings.DbTable.Value;
            var currentName = _currentUserContext.Name;
            try
            {
                Db.BeginTran();
                foreach (var item in input.Ids)
                {
                    var res = await service.UpdateAsync(d => new TEntity() { Status = false }, d => d.Id == item && d.TenantId == input.TenantId && d.Status == true);
                    if (res <= 0)
                    {
                        var model = await service.GetModelAsync(d => d.Id == item);
                        if (model?.Id <= 0)
                        {
                            throw new FriendlyException($"根据传递的【{item}】参数查出来该条数据不存在！");
                        }
                        if (model.Status == false)
                        {
                            return new ApiResult("该条数据已经被删除了", 200);
                        }
                        throw new FriendlyException("删除失败了！");
                    }
                    var tableName = new TEntity().GetType().Name;
                    if (!string.IsNullOrEmpty(allTable))
                    {
                        var allTableArry = allTable.Split(',');
                        for (int j = 0; j < allTableArry.Length; j++)
                        {
                            if (allTableArry[j].Contains(tableName))
                            {
                                tableName = allTableArry[j];
                                break;
                            }
                        }
                    }
                    var recycle = new Recycle()
                    {
                        CreateTime = DateTime.Now,
                        BusinessId = item,
                        UserId = userId,
                        TableType = tableName,
                        TenantId = input.TenantId,
                        Remark = $"用户名为【{currentName}】删除了表【{tableName}】中id={item}的记录数据。",
                        RestoreSql = $"update {tableName} set status=true where id={item} and TenantId={input.TenantId}",
                        RealyDelSql = $"delete  from {tableName}  where id={item} and TenantId={input.TenantId}"
                    };
                    var i = await AddAsync(recycle);
                    if (i <= 0)
                    {
                        throw new FriendlyException("删除成功了，但是放进回收站时失败了！");
                    }
                }
                Db.CommitTran();
            }
            catch (Exception e)
            {
                Db.RollbackTran();
                return new ApiResult(e.Message, 500);
            }
            return new ApiResult();
        }

        public async Task<ApiResult> GetPagesAsync(KeyListQuery query)
        {
            var datas = await Db.Queryable<Recycle, User>((r, u) => new JoinQueryInfos(JoinType.Inner, r.UserId == u.Id))
                    .WhereIF(!string.IsNullOrEmpty(query.Key), (r, u) => r.Remark.Contains(query.Key))
                  .OrderBy((r, u) => r.Id, OrderByType.Desc)
                  .Select((r, u) => new
                  {
                      UserName = u.Name,
                      CreateTime = r.CreateTime,
                      Id = r.Id,
                      r.TableType,
                      Remark = r.Remark,
                      TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == r.TenantId).Select(s => s.Name),
                  }).ToPageAsync(query.Page, query.Limit);
            return new ApiResult(datas);
        }
    }
}