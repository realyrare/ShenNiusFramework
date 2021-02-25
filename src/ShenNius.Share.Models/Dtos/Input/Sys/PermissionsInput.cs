using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Input.Sys
{
   public class PermissionsInput
    {
        public int RoleId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int MenuId { get; set; }
    }
}
