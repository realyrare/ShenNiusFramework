using ShenNius.Share.Models.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

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
    public class DeletesSiteInput: DeletesInput, IGlobalSite
    {
        public int SiteId { get; set; }
    }
}
