using Newtonsoft.Json;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Sys
{
    public interface IMenuService : IBaseServer<Menu>
    {
        Task<ApiResult> BtnCodeByMenuIdAsync(int menuId);
    }
    public class MenuService : BaseServer<Menu>, IMenuService
    {
        private readonly IConfigService _configService;

        public MenuService(IConfigService configService)
        {
            _configService = configService;
        }
        public async Task<ApiResult> BtnCodeByMenuIdAsync(int menuId)
        {
            var menuModel = await GetModelAsync(d => d.Status && d.Id == menuId);
            if (menuModel == null)
            {
                throw new FriendlyException(nameof(menuModel));
            }
            var btnList = JsonConvert.DeserializeObject<List<string>>(menuModel.BtnCodeIds.ToString());
           var containsBtnList= await _configService.GetListAsync(d => btnList.Contains(d.Id.ToString()));
            return new ApiResult(containsBtnList);
        }
    }   
}
