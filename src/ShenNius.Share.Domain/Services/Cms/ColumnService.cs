using AutoMapper;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Domain.Repository;
using System;
using System.Threading.Tasks;
using ShenNius.Share.Infrastructure.Attributes;
using System.Collections.Generic;
using System.Linq;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Configs;

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
    
        public ColumnService(IMapper mapper)
        {
            _mapper = mapper;
        }
        [CacheInterceptor]
        public virtual string GetTest()
        {
          return  "AOP-123";

        }
        public async Task<ApiResult> ModifyAsync(ColumnModifyInput columnModifyInput)
        {
            //columnModifyInput.SiteId = _memoryCache.Get<Site>(KeyHelper.Cms.CurrentSite).Id;
            var columnModel = await GetModelAsync(d => d.Title.Equals(columnModifyInput.Title)&&d.Id!= columnModifyInput.Id);
            if (columnModel?.Id > 0)
            {
                throw new FriendlyException("已经存在类目名称了");
            }
            var result = await DealColumn(columnModifyInput.ParentId, columnModifyInput.Id);
            var i = await UpdateAsync(d => new Column() {
                Title= columnModifyInput.Title,
                EnTitle= columnModifyInput.EnTitle,
                Attr= columnModifyInput.Attr,
                SubTitle= columnModifyInput.SubTitle,
                Summary= columnModifyInput.Summary,
                ImgUrl=columnModifyInput.ImgUrl,
                Layer=result.Item1,
                ParentList=result.Item2,
                ModifyTime=DateTime.Now,
                Keyword= columnModifyInput.Keyword,
                TenantId=columnModifyInput.TenantId,
                ParentId= columnModifyInput.ParentId
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
            
              var result= await  DealColumn(columnInput.ParentId, columnId);
            var i = await UpdateAsync(d => new Column() { ParentList = result.Item2, Layer = result.Item1 }, d => d.Id == columnId);
            return new ApiResult(i);
        }

        /// <summary>
        /// 处理栏目层次逻辑
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <param name="columnId">当前栏目Id</param>
        /// <returns></returns>
        private async Task< Tuple<int, string>> DealColumn(int parentId,int columnId)
        {
            string parentIdList = ""; int layer = 0;
            if (parentId > 0)
            {
                // 说明有父级  根据父级，查询对应的模型
                var model = await GetModelAsync(d => d.Id == parentId);
                if (model.Id > 0)
                {
                    parentIdList = model.ParentList + columnId + ",";
                    layer = model.Layer + 1;
                }
            }
            else
            {
                parentIdList = "," + columnId + ",";
                layer = 1;
            }
            return new Tuple<int, string>(layer,parentIdList);
        }

        public async Task<ApiResult> GetAllParentColumnAsync()
        {
            var list = await GetListAsync(d => d.Status);
            var data = new List<Column>();
            ChildModule(list, data, 0);

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Title = WebHelper.LevelName(item.Title, item.Layer);
                }
            }
            return new ApiResult(data);
        }
        /// <summary>
        /// 递归模块列表
        /// </summary>
        private void ChildModule(List<Column> list, List<Column> newlist, int parentId)
        {
            var result = list.Where(p => p.ParentId == parentId).OrderBy(p => p.Layer).ToList();
            if (!result.Any()) return;
            for (int i = 0; i < result.Count(); i++)
            {
                newlist.Add(result[i]);
                ChildModule(list, newlist, result[i].Id);
            }
        }
        public async Task<ApiResult> GetListPagesAsync(KeyListTenantQuery query)
        {
            var res = await Db.Queryable<Column>().Where(d => d.Status&&d.TenantId==query.TenantId).WhereIF(!string.IsNullOrEmpty(query.Key), d => d.Title.Contains(query.Key))
                .OrderBy(m => m.CreateTime, SqlSugar.OrderByType.Desc)
                .ToPageAsync(query.Page, query.Limit);
            var result = new List<Column>();
            if (!string.IsNullOrEmpty(query.Key))
            {
                var menuModel = await GetModelAsync(m => m.Title.Contains(query.Key));
                ChildModule(res.Items, result, menuModel.ParentId);
            }
            else
            {
                ChildModule(res.Items, result, 0);
            }
            if (result?.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Title = WebHelper.LevelName(item.Title, item.Layer);
                }
            }
            return new ApiResult(data: new { count = res.TotalItems, items = result });
        }


    }
}