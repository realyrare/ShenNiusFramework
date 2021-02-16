using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Service.Sys;
using System.Threading.Tasks;

namespace ShenNius.Sys.API.Controllers
{
    public class MenuController : ApiControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuController(IMenuService menuService,IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult(await _menuService.DeleteAsync(commonDeleteInput.Ids));
        }
        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            var res = await _menuService.GetPagesAsync(page, 15);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            if (id == 0)
            {
                throw new FriendlyException(nameof(id));
            }
            var res = await _menuService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpPost]
        public async Task<ApiResult> Add([FromBody] MenuInput menuInput)
        {
            var menu = _mapper.Map<Menu>(menuInput);
            return new ApiResult(await _menuService.AddAsync(menu));
        }

        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] MenuModifyInput menuModifyInput)
        {
            return new ApiResult(await _menuService.UpdateAsync(d => new Menu()
            {
                Name = menuModifyInput.Name,
                Url = menuModifyInput.Url,
                ModifyTime = menuModifyInput.ModifyTime,
                HttpMethod=menuModifyInput.HttpMethod,
                Status=menuModifyInput.Status,
                ParentId=menuModifyInput.ParentId,
                Icon=menuModifyInput.Icon,
                Sort=menuModifyInput.Sort
            }, d => d.Id == menuModifyInput.Id));
        }
        [HttpGet]
        public async Task<ApiResult> GetAllParentMenu()
        {
          var data= await _menuService.GetListAsync(d => d.Status && d.ParentId == 0);
            return new ApiResult(data);
        }
    }
}
