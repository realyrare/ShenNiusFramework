using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Output.Sys
{
    /// <summary>
    /// 左侧树形菜单
    /// </summary>
    public class MenuTreeInitOutput
    {
        public HomeInfo HomeInfo { get; set; } = new HomeInfo();
        public LogoInfo LogoInfo { get; set; } = new LogoInfo();
        public List<MenuInfo> MenuInfo { get; set; } = new List<MenuInfo>();
    }
    public class HomeInfo
    {
        public string Title { get; set; }
        public string Href { get; set; }
    }
    public class LogoInfo
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Href { get; set; }

    }
    public class MenuInfo
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Href { get; set; }
        public string Target { get; set; }

        public List<MenuInfo> Child { get; set; } = new List<MenuInfo>();
    }

}
