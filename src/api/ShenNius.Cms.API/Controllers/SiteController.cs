/*************************************
* 类名：SiteController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:22:57
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

using ShenNius.Share.Service.Cms;

namespace ShenNius.Cms.API.Controllers
{
    public class SiteController:ApiControllerBase
    {
        private readonly ISiteService _siteService;

        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }
    }
}