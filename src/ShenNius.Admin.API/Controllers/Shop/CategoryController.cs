using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Shop;
using System.Threading.Tasks;

namespace ShenNius.Admin.API.Controllers.Shop
{
    /// <summary>
    /// 商品分类控制器
    /// </summary>
    public class CategoryController : ApiTenantBaseController<Category, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, CategoryInput, CategoryModifyInput>
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(IBaseServer<Category> service, IMapper mapper, ICategoryService categoryService) : base(service, mapper)
        {
            _categoryService = categoryService;
        }
       
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="keywordListTenantQuery"></param>
        /// <returns></returns>
        [HttpGet]
        public override Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        {
            return _categoryService.GetListPagesAsync(keywordListTenantQuery);
        }
        [HttpPost]
        public override Task<ApiResult> Add([FromBody] CategoryInput input)
        {
            return _categoryService.AddToUpdateAsync(input);
        }
        [HttpPut]
        public override Task<ApiResult> Modify([FromBody] CategoryModifyInput input)
        {
            return _categoryService.ModifyAsync(input);
        }
        /// <summary>
        /// 所有父栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult> GetAllParentCategory()
        {
            return _categoryService.GetAllParentCategoryAsync();
        }
    }
}
