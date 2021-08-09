using ShenNius.Share.Models.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
   public  class CategoryModifyInput : GlobalTenantInput
    {
        public int Id { get; set; }
        public int ParentId { get; set; }

        public string IconSrc { get; set; }
        public string Name { get; set; }
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}
