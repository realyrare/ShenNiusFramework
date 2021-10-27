using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Cms
{

    [SugarTable("Cms_Message")]
    public class Message : BaseTenantEntity
    {
        public int BusinessId { get; set; }
        public string Types { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string IP { get; set; }
        public int ParentId { get; set; }
        public string Address { get; set; }
    }
}
