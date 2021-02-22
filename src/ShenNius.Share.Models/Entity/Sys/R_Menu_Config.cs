using SqlSugar;
using System;

namespace ShenNius.Share.Models.Entity.Sys
{
    [SugarTable("Sys_Menu_Config")]
    public  class R_Menu_Config
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int ConfigId { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
