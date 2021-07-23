using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Common;
using ShenNius.Share.Domain.Repository;
using System.Threading.Tasks;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Models.Entity.Sys;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using ShenNius.Share.Infrastructure.Configurations;

/*************************************
* 类名：ApiBaseController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/1 10:28:20
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.BaseController.Controllers
{
    /// <summary>
    /// 适用于多租户模块使用
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TDetailQuery">详情查询参数实体</typeparam>
    /// <typeparam name="TDeleteInput">删除实体</typeparam>
    /// <typeparam name="TListQuery">列表分页查询参数实体</typeparam>
    /// <typeparam name="TCreateInput">创建实体</typeparam>
    /// <typeparam name="TUpdateInput">更新实体</typeparam>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [MultiTenant]
    public abstract class ApiTenantBaseController<TEntity, TDetailQuery, TDeleteInput, TListQuery, TCreateInput, TUpdateInput> : ControllerBase
       where TEntity : BaseTenantEntity, new()
       where TDetailQuery : DetailTenantQuery
       where TDeleteInput : DeletesTenantInput
       where TListQuery : ListTenantQuery
       where TCreateInput : class
       where TUpdateInput : class
    {
        private readonly IBaseServer<TEntity> _service;
        private readonly IMapper _mapper;

        public ApiTenantBaseController(IBaseServer<TEntity> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        /// <summary>
        /// 批量真实删除
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        [HttpDelete]
        public virtual async Task<ApiResult> Deletes([FromBody] TDeleteInput deleteInput)
        {
            var res = await _service.DeleteAsync(deleteInput.Ids);
            if (res <= 0)
            {
                throw new FriendlyException("删除失败了！");
            }
            return new ApiResult();
        }
        /// <summary>
        /// 单个真实删除
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        [HttpDelete]
        public virtual async Task<ApiResult> Delete([FromBody] TDeleteInput deleteInput)
        {
            foreach (var item in deleteInput.Ids)
            {
                var res = await _service.DeleteAsync(d => d.Id == item && d.TenantId == deleteInput.TenantId);
                if (res <= 0)
                {
                    throw new FriendlyException("删除失败了！");
                }
            }
            return new ApiResult();
        }
        /// <summary>
        /// 软删除并将内容放回到回收站
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        [HttpDelete]
        [Transaction]
        public virtual async Task<ApiResult> SoftDelete([FromBody] TDeleteInput deleteInput)
        {
            var recycleService = HttpContext.RequestServices.GetService(typeof(IRecycleService)) as IRecycleService;
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(d => d.Type == JwtRegisteredClaimNames.Sid).Value);
            var allTable = AppSettings.DbTable.Value;

            var currentName = HttpContext.User.Identity.Name;
            foreach (var item in deleteInput.Ids)
            {
                var res = await _service.UpdateAsync(d => new TEntity() { Status = false }, d => d.Id == item && d.TenantId == deleteInput.TenantId && d.Status == true);
                if (res <= 0)
                {
                    var model = await _service.GetModelAsync(d => d.Id == item);
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
                    TenantId = deleteInput.TenantId,
                    Remark = $"用户名为【{HttpContext.User.Identity.Name}】删除了表【{tableName}】中id={item}的记录数据。",
                    RestoreSql = $"update {tableName} set status=true where id={item} and TenantId={deleteInput.TenantId}",
                    RealyDelSql = $"delete  from {tableName}  where id={item} and TenantId={deleteInput.TenantId}"
                };
                var i = await recycleService.AddAsync(recycle);
                if (i <= 0)
                {
                    throw new FriendlyException("删除成功了，但是放进回收站时失败了！");
                }
            }
            return new ApiResult();
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="listQuery">参数实体</param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ApiResult> GetListPages([FromQuery] TListQuery listQuery)
        {
            var res = await _service.GetPagesAsync(listQuery.Page, listQuery.Limit, d => d.TenantId == listQuery.TenantId && d.Status == true, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="detailQuery">参数实体</param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ApiResult> Detail([FromQuery] TDetailQuery detailQuery)
        {
            var res = await _service.GetModelAsync(d => d.Id == detailQuery.Id && d.TenantId == detailQuery.TenantId && d.Status == true);
            return new ApiResult(data: res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="createInput">添加实体</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ApiResult> Add([FromBody] TCreateInput createInput)
        {
            var entity = _mapper.Map<TEntity>(createInput);
            var res = await _service.AddAsync(entity);
            if (res <= 0)
            {
                throw new FriendlyException("添加失败了！");
            }
            return new ApiResult(data: res);
        }
        /// <summary>
        /// 修改-默认忽略更新CreateTime字段
        /// </summary>
        /// <param name="updateInput">修改实体</param>
        /// <returns></returns>
        [HttpPut]
        public virtual async Task<ApiResult> Modify([FromBody] TUpdateInput updateInput)
        {
            var entity = _mapper.Map<TEntity>(updateInput);
            var res = await _service.UpdateAsync(entity, d => new { d.CreateTime });
            if (res <= 0)
            {
                throw new FriendlyException("修改失败了！");
            }
            return new ApiResult(data: res);
        }
    }
}