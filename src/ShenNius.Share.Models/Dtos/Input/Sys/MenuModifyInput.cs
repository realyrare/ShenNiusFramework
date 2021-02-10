﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Input.Sys
{
   public class MenuModifyInput
    {
        public int Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int ParentId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Url { get; set; }
  
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string HttpMethod { get; set; }

        public DateTime ModifyTime { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
    }
}
