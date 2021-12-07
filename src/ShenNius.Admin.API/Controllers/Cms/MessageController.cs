using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using System;
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

namespace ShenNius.Admin.API.Controllers.Cms
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [MultiTenant]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            this._messageService = messageService;
        }
        [HttpGet]
        public async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
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
        /// 批量真实删除
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] DeletesTenantInput deleteInput)
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