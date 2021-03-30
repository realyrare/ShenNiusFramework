using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Repository;
using System;
using System.Threading.Tasks;

/*************************************
* 类名：ArticleService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:15:30
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Service.Cms
{

    public interface IColumnService : IBaseServer<Column>
    {
        Task<ApiResult> AddToUpdateAsync(ColumnInput columnInput);
        Task<ApiResult> ModifyAsync(ColumnModifyInput columnModifyInput);
    }
    public class ColumnService : BaseServer<Column>, IColumnService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ColumnService(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            this._memoryCache = memoryCache;
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
                SiteId=columnModifyInput.SiteId,
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
    }
}