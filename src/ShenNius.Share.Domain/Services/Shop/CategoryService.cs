using AutoMapper;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Entity.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*************************************
* 类名：CategoryService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/10 11:26:55
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Shop
{
    public interface ICategoryService : IBaseServer<Category>
    {
        Task<ApiResult> AddToUpdateAsync(CategoryInput CategoryInput);
        Task<ApiResult> ModifyAsync(CategoryModifyInput CategoryModifyInput);

        Task<ApiResult> GetAllParentCategoryAsync();
        Task<ApiResult> GetListPagesAsync(KeyListTenantQuery keyListSiteQuery);
    }
    public class CategoryService : BaseServer<Category>, ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;

        public CategoryService(IMapper mapper, ICurrentUserContext currentUserContext)
        {
            _mapper = mapper;
            this._currentUserContext = currentUserContext;
        }

        public async Task<ApiResult> ModifyAsync(CategoryModifyInput input)
        {
            var categoryModel = await GetModelAsync(d => d.Name.Equals(input.Name) && d.Id != input.Id);
            if (categoryModel?.Id > 0)
            {
                throw new FriendlyException("已经存在类目名称了");
            }

            var result = await WebHelper.DealTreeData(input.ParentId, input.Id, async () =>
              await GetModelAsync(d => d.Id == input.ParentId));

            var i = await UpdateAsync(d => new Category()
            {
                Name = input.Name,
                IconSrc = input.IconSrc,
                Layer = result.Item1,
                ParentList = result.Item2,
                ModifyTime = DateTime.Now,
                TenantId = input.TenantId,
                ParentId = input.ParentId
            }, d => d.Id == input.Id);
            return new ApiResult(i);
        }
        [Transaction]
        public async Task<ApiResult> AddToUpdateAsync(CategoryInput input)
        {
            var CategoryModel = await GetModelAsync(d => d.Name.Equals(input.Name));
            if (CategoryModel?.Id > 0)
            {
                throw new FriendlyException("已经存在类目名称了");
            }
            var category = _mapper.Map<Category>(input);
            var categoryId = await AddAsync(category);
            var result = await WebHelper.DealTreeData(input.ParentId, categoryId, async () =>
           await GetModelAsync(d => d.Id == input.ParentId));
            var i = await UpdateAsync(d => new Category() { ParentList = result.Item2, Layer = result.Item1 }, d => d.Id == categoryId);
            return new ApiResult(i);
        }

        public async Task<ApiResult> GetAllParentCategoryAsync()
        {
            var list = await GetListAsync(d => d.Status&&d.TenantId== _currentUserContext.TenantId);
            var data = new List<Category>();
            WebHelper.ChildNode(list, data, 0);
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Name = WebHelper.LevelName(item.Name, item.Layer);
                }
            }
            return new ApiResult(data);
        }

        public async Task<ApiResult> GetListPagesAsync(KeyListTenantQuery query)
        {
            var res = await Db.Queryable<Category>().Where(d => d.Status && d.TenantId == query.TenantId)
               .WhereIF(!string.IsNullOrEmpty(query.Key), c => c.Name.Contains(query.Key))
               .OrderBy(c => c.Id, OrderByType.Desc)
               .Select(c => new Category()
               {
                   Id = c.Id,
                   TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == c.TenantId).Select(s => s.Name),
                   Name = c.Name,
                   CreateTime = c.CreateTime,
                   Layer = c.Layer,
                   ModifyTime = c.ModifyTime,
                   IconSrc = c.IconSrc
               })
               .ToPageAsync(query.Page, query.Limit);
            var result = new List<Category>();
            if (!string.IsNullOrEmpty(query.Key))
            {
                var menuModel = await GetModelAsync(m => m.Name.Contains(query.Key));
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
                    item.Name = WebHelper.LevelName(item.Name, item.Layer);
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