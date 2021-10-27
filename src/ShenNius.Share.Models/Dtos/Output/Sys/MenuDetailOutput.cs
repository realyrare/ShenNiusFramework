using ShenNius.Share.Model.Entity.Sys;
using System.Collections.Generic;

/*************************************
* 类名：MenuOutput
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/3 10:31:54
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Output.Sys
{
    /// <summary>
    /// 菜单编辑页面使用的model
    /// </summary>
    public class MenuDetailOutput
    {
        public Menu MenuOutput { get; set; }
        public List<Config> ConfigOutputs { get; set; }
    }

}