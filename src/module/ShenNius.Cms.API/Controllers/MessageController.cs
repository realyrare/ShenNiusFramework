using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Entity.Sys;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：MessageController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/16 14:40:43
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [MultiTenant]
    public class MessageController:ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IRecycleService _recycleService;

        public MessageController(IMessageService messageService, IRecycleService recycleService)
        {
            this._messageService = messageService;
            this._recycleService = recycleService;
        }
        [HttpGet]
        public  async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        {
            Expression<Func<Message, bool>> whereExpression = d => d.Status == true;
            if (keywordListTenantQuery.TenantId > 0)
            {
                whereExpression = d => d.TenantId == keywordListTenantQuery.TenantId;
            }
            if (!string.IsNullOrEmpty(keywordListTenantQuery.Key))
            {
                whereExpression = d => d.UserName.Contains(keywordListTenantQuery.Key);
            }
            var res = await _messageService.GetPagesAsync(keywordListTenantQuery.Page, keywordListTenantQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpDelete]
        public  async Task<ApiResult> SoftDelete([FromBody] DeletesTenantInput input)
        {          
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(d => d.Type == JwtRegisteredClaimNames.Sid).Value);
            var currentName = HttpContext.User.Identity.Name;
            foreach (var item in input.Ids)
            {
                var res = await _messageService.UpdateAsync(d => new Message() { Status = false }, d => d.Id == item && d.TenantId ==input.TenantId && d.Status == true);
                var model = new Recycle()
                { CreateTime = DateTime.Now, BusinessId = item, UserId = userId, TableType = nameof(Message), TenantId = input.TenantId, Remark = $"{HttpContext.User.Identity.Name}删除了{nameof(Message)}中的{item}记录",RestoreSql=$"update {nameof(Message)} set status=false where id={item}and TenantId={input.TenantId}" };
                await _recycleService.AddAsync(model);
                if (res <= 0)
                {
                    throw new FriendlyException("删除失败了！");
                }
            }
            return new ApiResult();
        }
        /// <summary>
        /// 批量真实删除
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        [HttpDelete]
        public  async Task<ApiResult> Deletes([FromBody] DeletesTenantInput deleteInput)
        {
            var res = await _messageService.DeleteAsync(deleteInput.Ids);
            if (res <= 0)
            {
                throw new FriendlyException("删除失败了！");
            }
            return new ApiResult();
        }
    }
}