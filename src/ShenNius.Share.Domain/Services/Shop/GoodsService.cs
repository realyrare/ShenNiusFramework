using AutoMapper;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Entity.Shop;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    public class GoodsService : BaseServer<Goods>, IGoodsService
    {
        private readonly IMapper _mapper;

        public GoodsService(IMapper mapper)
        {
            _mapper = mapper;
        }
        [Transaction]
        public async Task<ApiResult> AddAsync(GoodsInput input)
        {
            // 保存商品
            var goods = _mapper.Map<Goods>(input);
            var goodsId = await AddAsync(goods);
           
            // 保存规格
            if (input.SpecType == SpecTypeEnum.Single.GetValue<int>())
            {
                GoodsSpec goodsSpec = new GoodsSpec()
                {
                    CreateTime=DateTime.Now,
                    TenantId=input.TenantId,
                    GoodsId=goodsId,
                    SpecSkuId=null
                };               
               await Db.Insertable(goodsSpec).ExecuteReturnIdentityAsync();               
            }
            else
            {
                var list = new List<GoodsSpec>();
               // var specMany = JsonConvert.DeserializeObject<SpecManyDto>(input.SpecMany);
                //foreach (var specList in specMany.SpecList)
                //{
                //    specList.GoodsSpec.SpecSkuId = specList.SpecSkuId;
                //    specList.GoodsSpec.GoodsId = goodsId;
                //    specList.GoodsSpec.CreateTime = CreateTime;
                //    specList.GoodsSpec.UpdateTime = UpdateTime;
                //    specList.GoodsSpec.WxappId = WxappId;
                //    var goodsSpec = specList.GoodsSpec.Mapper<GoodsSpec>();
                //    list.Add(goodsSpec);
                //}
                //return list;

                //var goodsSpecs = request.BuildGoodsSpecs((uint)goodsId);
                //await _fsql.Insert<GoodsSpec>().AppendData(goodsSpecs).ExecuteAffrowsAsync();
                //var goodsSpecRels = request.BuildGoodsSpecRels((uint)goodsId);
                //await _fsql.Insert<GoodsSpecRel>().AppendData(goodsSpecRels).ExecuteAffrowsAsync();
            }
            return new ApiResult();
        }
    }
}