using AutoMapper;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Shop;

namespace ShenNius.Shop.API.Controllers
{
    public class CategoryController : ApiTenantBaseController<Category, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, CategoryInput, CategoryModifyInput>
    {
        private readonly IBaseServer<Category> _service;

        public CategoryController(IBaseServer<Category> service, IMapper mapper) : base(service, mapper)
        {
            _service = service;
        }
        /// <summary>
        /// 所有父分类
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public Task<ApiResult> GetAllParentColumn()
        //{
        //    return _service.GetAllParentColumnAsync();
        //}
    }
}
