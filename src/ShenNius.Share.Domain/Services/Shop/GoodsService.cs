using AutoMapper;
using Newtonsoft.Json;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Dtos.Output.Shop;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
using SqlSugar;
using System;
using System.Linq;
using System.Linq.Expressions;
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
    public interface IGoodsService : IBaseServer<Goods>
    {
        Task<ApiResult> AddAsync(GoodsInput input);
        Task<ApiResult> ModifyAsync(GoodsModifyInput input);
        Task<ApiResult<GoodsModifyInput>> DetailAsync(int id);
        /// <summary>
        /// 添加规格组名称和值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResult> AddSpecAsync(SpecInput input);
        /// <summary>
        /// 添加规格组值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResult> AddSpecAsync(SpecValuesInput input);

        Task<ApiResult> GetListPageAsync(KeyListTenantQuery query);
        /// <summary>
        /// 小程序首页
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<ApiResult> GetByWherePageAsync(ListTenantQuery query, Expression<Func<Goods, Category, GoodsSpec, object>> orderBywhere, OrderByType sort, Expression<Func<Goods, Category, GoodsSpec, bool>> where = null);
        /// <summary>
        /// 立即购买
        /// </summary>
        /// <returns></returns>
        Task<ApiResult> GetBuyNowAsync(int goodsId, int goodsNum, string goodsNo, int tenantId);
        /// <summary>
        /// 商品信息判断（添加购物车，下单共用）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsNum"></param>
        /// <param name="specSkuId"></param>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        Task<Tuple<Goods, GoodsSpec>> GoodInfoIsExist(int goodsId, int goodsNum, string specSkuId, int appUserId);
    }
    public class GoodsService : BaseServer<Goods>, IGoodsService
    {
        private readonly IMapper _mapper;

        public GoodsService(IMapper mapper)
        {
            _mapper = mapper;
        }

        
        public async Task<Tuple<Goods, GoodsSpec>> GoodInfoIsExist(int goodsId, int goodsNum, string specSkuId, int appUserId)
        {
            Goods goodsModel = await GetModelAsync(d => d.Id == goodsId && d.Status);
            if (goodsModel?.Id == null)
            {
                throw new FriendlyException($"此商品{goodsId}没有查找到对应的商品信息");
            }
            if (goodsModel.GoodsStatus == GoodsStatusEnum.SoldOut.GetValue<int>())
            {
                throw new FriendlyException($"此商品已经下架");
            }
            GoodsSpec goodsSpec = null;
            if (!string.IsNullOrEmpty(specSkuId))
            {
                //多规格
                // var skuIds = specSkuId.Split('_');
                // List<int> ids = new List<int>();
                // for (int i = 0; i < skuIds.Length; i++)
                // {
                //     ids.Add(Convert.ToInt32(skuIds[i]));
                // }
                // //根据skuid查出对应的商品id
                //var goodsIds =await Db.Queryable<GoodsSpecRel>().Where(d => d.Status && ids.Contains(d.SpecValueId)).Select(d=>d.GoodsId).ToListAsync();
                // if (goodsIds.Count>0)
                // {
                //     var goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.Status && goodsIds.Contains(d.GoodsId)).FirstAsync();

                // }
                goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.Status && d.GoodsId == goodsId && d.SpecSkuId.Equals(specSkuId)).FirstAsync();
            }
            else
            {
                goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.Status && d.GoodsId == goodsId).FirstAsync();
            }

            if (!(goodsSpec.StockNum > 0 & goodsSpec.StockNum > goodsNum))
            {
                throw new ArgumentNullException($"商品库存不存，目前仅剩{goodsSpec.StockNum}件");
            }
            return new Tuple<Goods, GoodsSpec>(goodsModel, goodsSpec);
        }

        public async Task<ApiResult> GetBuyNowAsync(int goodsId,int goodsNum, string goodsNo, int tenantId)
        {
          var model=  await Db.Queryable<Goods, GoodsSpec>((g, gc) => new JoinQueryInfos(JoinType.Inner, g.Id == gc.GoodsId && gc.GoodsNo == goodsNo))
                .Where((g, gc) => g.TenantId == tenantId&&g.Id==goodsId&&gc.Id==goodsId)
                .Select((g, gc) => new {
                g.Id,
                g.ImgUrl,
                gc.GoodsNo,
                gc.GoodsPrice,
                gc.StockNum
                }).FirstAsync();

            if (model?.Id == null)
            {
                return new ApiResult("未找到该商品");
            }
            if (!(model.StockNum > 0 & model.StockNum > goodsNum))
            {
                throw new ArgumentNullException($"商品库存不存，目前仅剩{model.StockNum}件");
            }
            var totalPrice = model.GoodsPrice * goodsNum;
            return new ApiResult(new
            {
                GoodsList = model, //单价*数量 
                OrderTotalPrice = totalPrice
            });
          
        }
        public async Task<ApiResult> GetByWherePageAsync(ListTenantQuery query, Expression<Func<Goods, Category, GoodsSpec, object>> orderBywhere, OrderByType sort,Expression<Func<Goods, Category, GoodsSpec, bool>> where=null )
        {
            var datas = await Db.Queryable<Goods, Category, GoodsSpec>((g, c,gc) => new JoinQueryInfos(
                JoinType.Inner, g.CategoryId == c.Id && g.TenantId == query.TenantId,JoinType.Inner,g.Id==gc.GoodsId))
                .Where((g, c, gc) =>g.Status&&g.GoodsStatus==10)
                .WhereIF(where!=null,where)
              .OrderBy(orderBywhere, OrderByType.Desc)
              .Select((g, c, gc) => new GoodsOutput()
              {
                  Name = g.Name,
                  CategoryName = c.Name,
                  SalesActual = g.SalesActual,
                  Id = g.Id,
                  ImgUrl = g.ImgUrl,
                  GoodsPrice = gc.GoodsPrice,
            GoodsSales = gc.GoodsSales,
            LinePrice = gc.LinePrice,
              }).ToPageAsync(query.Page, query.Limit);
            foreach (var item in datas.Items)
            {
                if (!string.IsNullOrEmpty(item.ImgUrl))
                {
                    var imgArry = item.ImgUrl.Split(',');
                    if (imgArry.Length > 0)
                    {
                        item.ImgUrl = imgArry[0];
                    }
                }               
            }
            return new ApiResult(datas);
        }
        public async Task<ApiResult> GetListPageAsync(KeyListTenantQuery  query)
        {
            var datas =await Db.Queryable<Goods, Category>((g, c) => new JoinQueryInfos(JoinType.Inner, g.CategoryId == c.Id&&g.TenantId==query.TenantId))
                .WhereIF(!string.IsNullOrEmpty(query.Key), (g, c) => g.Name.Contains(query.Key))
                .OrderBy((g, c) => g.Id, OrderByType.Desc)
                .Select((g, c) => new Goods() { 
                Name=g.Name,
                CategoryName=c.Name,
                CreateTime=g.CreateTime,
                DeductStockType=g.DeductStockType,
                SalesInitial=g.SalesInitial,
                SalesActual=g.SalesActual,
                SpecType=g.SpecType,
                Id=g.Id,
                TenantName= SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == c.TenantId).Select(s => s.Name),
                TenantId=c.TenantId,
                ImgUrl=g.ImgUrl
                }).ToPageAsync(query.Page,query.Limit);
            foreach (var item in datas.Items)
            {
                if (!string.IsNullOrEmpty(item.ImgUrl))
                {
                    var imgArry = item.ImgUrl.Split(',');
                    if (imgArry.Length>0)
                    {
                        item.ImgUrl = imgArry[0];
                    }                   
                }
            }
            return new ApiResult(datas);
        }
        public async Task<ApiResult<GoodsModifyInput>> DetailAsync(int id)
        {
            Goods goods = await GetModelAsync(d => d.Id == id && d.Status);

            if (goods == null) throw new FriendlyException($"此商品{id}没有查找对应的商品信息");
           var model= _mapper.Map<GoodsModifyInput>(goods);
              
            model.GoodsSpecInput = new GoodsSpecInput();
            if (model.SpecType == SpecTypeEnum.Single.GetValue<int>())
            {
                var goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.GoodsId == id).FirstAsync();                 
                model.GoodsSpecInput = _mapper.Map<GoodsSpecInput>(goodsSpec);
            }
            return new ApiResult<GoodsModifyInput>(model);
        }
  
        public async Task<ApiResult> AddAsync(GoodsInput input)
        {
            try
            {
                Db.BeginTran();
                // 保存商品
                var goods = _mapper.Map<Goods>(input);
                var goodsId = await Db.Insertable<Goods>(goods).ExecuteReturnIdentityAsync();
                // 保存规格
                await DealwithGoodsSpec(goodsId, input);
                Db.CommitTran();
            }
            catch (Exception e)
            {
                Db.RollbackTran();
                return new ApiResult(e.Message);
            }          
            return new ApiResult();
        }
        /// <summary>
        /// 公共的商品规格信息处理
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task DealwithGoodsSpec(int goodsId, GoodsInput input) 
        {
            // 保存规格
            if (input.SpecType == SpecTypeEnum.Single.GetValue<int>())
            {
                var specSingle= JsonConvert.DeserializeObject<GoodsSpecInput>(input.SpecSingle);
                input.GoodsSpecInput = specSingle;
               var goodsSpec = input.BuildGoodsSpec(goodsId);
                if (null == goodsSpec)
                {
                    throw new FriendlyException("商品规格实体数据不能为空！");
                }
                await Db.Insertable(goodsSpec).ExecuteReturnIdentityAsync();
            }
            else
            {
                var goodsSpecs = input.BuildGoodsSpecs(goodsId);
                if (null == goodsSpecs || goodsSpecs.Count == 0)
                {
                    throw new FriendlyException("商品规格实体数据集合不能为空！");
                }
                await Db.Insertable(goodsSpecs).ExecuteReturnIdentityAsync();
                var goodsSpecRels = input.BuildGoodsSpecRels(goodsId);
                if (goodsSpecRels.Count == 0 || goodsSpecRels == null)
                {
                    throw new FriendlyException("商品规格实体关系集合数据不能为空！");
                }
                //根据规格值反推规格组id
               var specValues= await Db.Queryable<SpecValue>().Where(d => d.Status).ToListAsync();
                foreach (var item in goodsSpecRels)
                {
                   var specId = specValues.Where(d => d.Status && d.Id == item.SpecValueId).Select(d=>d.SpecId);
                    item.SpecId = specId.FirstOrDefault();
                }
                await Db.Insertable(goodsSpecRels).ExecuteReturnIdentityAsync();
            }
        }
        public async Task<ApiResult> ModifyAsync(GoodsModifyInput input)
        {           
            var goods = await GetModelAsync(d => d.Id == input.Id); 
            if (goods == null) throw new FriendlyException($"此商品{input.Id}没有查找对应的商品信息");
            try
            {
                Db.BeginTran();
                // 更新商品
                var model = _mapper.Map<Goods>(input);
                var goodsId = await Db.Updateable(model).IgnoreColumns(d => new { d.CreateTime }).ExecuteCommandAsync();
                // 更新规格 
                await Db.Deleteable<GoodsSpec>().Where(d => d.GoodsId == input.Id).ExecuteCommandAsync();
                await Db.Deleteable<GoodsSpecRel>().Where(d => d.GoodsId == input.Id).ExecuteCommandAsync();
                // 保存规格
                await DealwithGoodsSpec(input.Id, input);
                Db.CommitTran();
            }
            catch (Exception e)
            {
                Db.RollbackTran();
                return new ApiResult(e.Message);
            }
            return new ApiResult();        
        }
        /// <summary>
        /// 添加规格组名称和值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddSpecAsync(SpecInput input)
        {
            // 判断规格组是否存在
            var specId =await Db.Queryable<Spec>().Where(d => d.Name.Equals(input.SpecName)).Select(d=>d.Id).FirstAsync();
            if (specId==0)
            {
                var specModel = new Spec
                {
                    Name = input.SpecName,
                    TenantId = input.TenantId,
                    CreateTime = DateTime.Now     
                };
                specId = await Db.Insertable(specModel).ExecuteReturnIdentityAsync();
            }        
            var specValueId = await  Db.Queryable<SpecValue>().Where(d => d.Value.Equals(input.SpecValue) && d.SpecId == specId).Select(d => d.Id).FirstAsync();
            // 判断规格值是否存在
           
            if (specValueId == 0)
            {
                var specValueModel = new SpecValue
                {
                    SpecId = specId,
                    Value = input.SpecValue,
                    TenantId = input.TenantId,
                    CreateTime = DateTime.Now,
                };
                specValueId = await Db.Insertable(specValueModel).ExecuteReturnIdentityAsync();
            }
            return new ApiResult(data: new { specId, specValueId });
        }
        /// <summary>
        /// 添加规格id添加规格组值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddSpecAsync(SpecValuesInput input)
        {
            var specValueId = await Db.Queryable<SpecValue>().Where(d => d.Value.Equals(input.SpecValue) && d.SpecId == input.SpecId).Select(d => d.Id).FirstAsync();         
            if (specValueId == 0)
            {
                var specValueModel = new SpecValue
                {
                    SpecId = input.SpecId,
                    Value = input.SpecValue,
                    TenantId = input.TenantId,
                    CreateTime = DateTime.Now,
                };
                specValueId = await Db.Insertable(specValueModel).ExecuteReturnIdentityAsync();
            }
            return new ApiResult(data: new {specValueId });
        }
    }
}