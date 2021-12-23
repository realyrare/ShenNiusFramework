using AutoMapper;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Entity.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*************************************
* 类名：ArticleService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:15:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Cms
{

    public interface IColumnService : IBaseServer<Column>
    {
        Task<ApiResult> AddToUpdateAsync(ColumnInput columnInput);
        Task<ApiResult> ModifyAsync(ColumnModifyInput columnModifyInput);
        string GetTest();
        Task<ApiResult> GetAllParentColumnAsync();
        Task<ApiResult> GetListPagesAsync(KeyListTenantQuery keyListSiteQuery);
    }
    public class ColumnService : BaseServer<Column>, IColumnService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;

        public ColumnService(IMapper mapper, ICurrentUserContext currentUserContext)
        {
            _mapper = mapper;
            this._currentUserContext = currentUserContext;
        }
        [CacheInterceptor]
        public virtual string GetTest()
        {
            return "AOP-123";
        }
        public async Task<ApiResult> ModifyAsync(ColumnModifyInput columnModifyInput)
        {
            var columnModel = await GetModelAsync(d => d.Title.Equals(columnModifyInput.Title) && d.Id != columnModifyInput.Id);
            if (columnModel?.Id > 0)
            {
                throw new FriendlyException("已经存在类目名称了");
            }
            var result = await WebHelper.DealTreeData(columnModifyInput.ParentId, columnModifyInput.Id, async () =>
              await GetModelAsync(d => d.Id == columnModifyInput.ParentId));

            var i = await UpdateAsync(d => new Column()
            {
                Title = columnModifyInput.Title,
                EnTitle = columnModifyInput.EnTitle,
                Attr = columnModifyInput.Attr,
                SubTitle = columnModifyInput.SubTitle,
                Summary = columnModifyInput.Summary,
                ImgUrl = columnModifyInput.ImgUrl,
                Layer = result.Item1,
                ParentList = result.Item2,
                ModifyTime = DateTime.Now,
                Keyword = columnModifyInput.Keyword,
                TenantId = columnModifyInput.TenantId,
                ParentId = columnModifyInput.ParentId
            }, d => d.Id == columnModifyInput.Id);
            return new ApiResult(i);
        }
        public async Task<ApiResult> AddToUpdateAsync(ColumnInput columnInput)
        {
            var columnModel = await GetModelAsync(d => d.Title.Equals(columnInput.Title));
            if (columnModel?.Id > 0)
            {
                throw new FriendlyException("已经存在类目名称了");
            }
            var column = _mapper.Map<Column>(columnInput);
            var columnId = await AddAsync(column);
            var result = await WebHelper.DealTreeData(columnInput.ParentId, columnId, async () =>
           await GetModelAsync(d => d.Id == columnInput.ParentId));
            var i = await UpdateAsync(d => new Column() { ParentList = result.Item2, Layer = result.Item1 }, d => d.Id == columnId);
            return new ApiResult(i);
        }

        public async Task<ApiResult> GetAllParentColumnAsync()
        {
            var list = await GetListAsync(d => d.Status&&d.TenantId== _currentUserContext.TenantId);
            var data = new List<Column>();
            WebHelper.ChildNode(list, data, 0);
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Title = WebHelper.LevelName(item.Title, item.Layer);
                }
            }
            return new ApiResult(data);
        }

        public async Task<ApiResult> GetListPagesAsync(KeyListTenantQuery query)
        {
            var res = await Db.Queryable<Column>().Where(d => d.Status && d.TenantId == query.TenantId)
                .WhereIF(!string.IsNullOrEmpty(query.Key), c => c.Title.Contains(query.Key))
                .OrderBy(c => c.Id, OrderByType.Desc)
                .Select(c => new Column()
                {
                    Id = c.Id,
                    TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == c.TenantId).Select(s => s.Name),
                    Title = c.Title,
                    CreateTime = c.CreateTime,
                    EnTitle = c.EnTitle,
                    Layer = c.Layer,
                    ModifyTime = c.ModifyTime
                })
                .ToPageAsync(query.Page, query.Limit);

            var result = new List<Column>();
            if (!string.IsNullOrEmpty(query.Key))
            {
                var menuModel = await GetModelAsync(m => m.Title.Contains(query.Key));
                WebHelper.ChildNode(res.Items, result, menuModel.ParentId);
            }
            else
            {
                WebHelper.ChildNode(res.Items, result, 0);
            }
            if (result?.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Title = WebHelper.LevelName(item.Title, item.Layer);
                }
                return new ApiResult(data: new { count = res.TotalItems, items = result });
            }
            else
            {
                return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
            }
        }


    }
}