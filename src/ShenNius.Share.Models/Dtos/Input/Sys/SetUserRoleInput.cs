using System.Collections.Generic;

namespace ShenNius.Share.Models.Dtos.Input.Sys
{
    public  class SetUserRoleInput
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; }
        public bool IsEnable { get; set; } = true;
    }
}
