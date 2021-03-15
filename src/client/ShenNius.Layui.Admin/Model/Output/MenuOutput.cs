using System;

namespace ShenNius.Layui.Admin.Model
{
    public class MenuOutput
    {
        public int Id { get; set; }          
        public int ParentId { get; set; }
        public string NameCode { get; set; }        
        public string Name { get; set; }        
        public string Url { get; set; }          
        public DateTime CreateTime { get; set; }          
        public string HttpMethod { get; set; }
        public bool Status { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
        public string[] BtnCodeIds { get; set; }

        public string ParentIdList { get; set; }
        public int Layer { get; set; }
        public string BtnCodeName { get; set; }
    }
}
