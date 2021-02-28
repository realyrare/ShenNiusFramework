using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Output.Sys
{
  public  class RoleListOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
