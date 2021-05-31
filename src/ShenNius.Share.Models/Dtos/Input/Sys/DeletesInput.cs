using ShenNius.Share.Models.Entity.Common;
using System.Collections.Generic;

namespace ShenNius.Share.Models.Dtos.Input.Sys
{
    /// <summary>
    /// 批量删除
    /// </summary>
    public class DeletesInput
    {
        public List<int> Ids { get; set; } = new List<int>();
    }
    /// <summary>
    /// 多租户使用
    /// </summary>
    public class DeletesTenantInput: DeletesInput, IGlobalTenant 
    {
        public int TenantId { get; set; }
    }
}
