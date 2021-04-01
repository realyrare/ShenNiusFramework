using AutoMapper;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Repository;

/*************************************
* 类名：KeywordController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/31 19:08:12
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Shop.API.Controllers
{
    public class KeywordController : ApiTenantBaseController<Keyword, DetailSiteQuery, DeletesSiteInput, ListSiteQuery, KeywordInput, KeywordModifyInput>
    {
        public KeywordController(IBaseServer<Keyword> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}