using System.Collections.Generic;

namespace ShenNius.Share.Models.Dtos.Input.Sys
{
    public  class SetRoleMenuInput
    {
        public int RoleId { get; set; }
        public List<int> MenuIds { get; set; }
    }
}
