﻿using AutoMapper;
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