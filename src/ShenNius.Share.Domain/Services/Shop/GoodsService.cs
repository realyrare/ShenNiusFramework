using AutoMapper;
using Newtonsoft.Json;
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
            try
            {
                // 保存商品
                var goods = _mapper.Map<Goods>(input);
                var goodsId = await AddAsync(goods);
                // 保存规格
                if (input.SpecType == SpecTypeEnum.Single.GetValue<int>())
                {
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
                    if (goodsSpecRels.Count==0|| goodsSpecRels==null)
                    {
                        throw new FriendlyException("商品规格实体关系集合数据不能为空！");
                    }
                    await Db.Insertable(goodsSpecRels).ExecuteReturnIdentityAsync();
                }
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message);
            }          
            return new ApiResult();
        }
    }
}