using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Input
{
  public  class RoleModifyInput
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}
