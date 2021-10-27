﻿using System;

namespace ShenNius.Share.Models.Dtos.Input
{
    public class RoleInput
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
