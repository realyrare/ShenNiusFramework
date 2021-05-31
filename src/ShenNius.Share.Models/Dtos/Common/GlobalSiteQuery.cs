using ShenNius.Share.Models.Entity.Common;

namespace ShenNius.Share.Models.Dtos.Common
{
    public class GlobalTenantQuery : IGlobalTenant
    {       
        public int TenantId { get; set; }
    }
}
