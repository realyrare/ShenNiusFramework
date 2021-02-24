using Newtonsoft.Json;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Output.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Sys
{
    public interface IMenuService : IBaseServer<Menu>
    {
        Task<ApiResult> BtnCodeByMenuIdAsync(int menuId);
        Task<ApiResult> TreeRoleIdAsync(int roleId);
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
            if (menuId==0)
            {
                return new ApiResult(menuId);
            }
            var menuModel = await GetModelAsync(d => d.Status && d.Id == menuId);
            if (menuModel == null)
            {
                throw new FriendlyException(nameof(menuModel));
            }
           var btnList = JsonConvert.DeserializeObject<List<string>>(menuModel.BtnCodeIds.ToString());
           var containsBtnList= await _configService.GetListAsync(d => btnList.Contains(d.Id.ToString()));
            return new ApiResult(containsBtnList);
        }
        public async Task<ApiResult> TreeRoleIdAsync(int roleId)
        {
            var list =new List<MenuTreeOutput>();
            var existMenuId = await Db.Queryable<R_Role_Menu>().Where(d => d.IsPass && d.RoleId == roleId).Select(d => d.MenuId).ToListAsync();
           
            var allMenus= await GetListAsync(d => d.Status);
            if (allMenus.Count <= 0 || allMenus == null)
            {
                return new ApiResult(allMenus);
            }

            foreach (var item in allMenus)
            {
                if (item.ParentId!=0)
                {
                    continue;
                }
                var menuTreeOutput = new MenuTreeOutput() {
                    Id = item.Id,
                    Title = item.Name,
                    Checked = existMenuId.FirstOrDefault(d => d == item.Id) != 0,
                    Children = AddChildNode(allMenus, item.Id,existMenuId),                
                };
                list.Add(menuTreeOutput);
            }
            return new ApiResult(list);    
        }
        private List<MenuTreeOutput> AddChildNode(IEnumerable<Menu> data, int parentId,List<int> existMenuId)
        {
            var list = new List<MenuTreeOutput>();
          var data2=  data.Where(d => d.ParentId == parentId).OrderBy(d => d.Name).ToList();
            foreach (var item in data2)
            {
                var menuTreeOutput = new MenuTreeOutput()
                {
                    Id = item.Id,
                    Title = item.Name,
                    Checked = existMenuId.FirstOrDefault(d => d == item.Id) != 0,
                    Children = AddChildNode(data, item.Id, existMenuId)
                };
                list.Add(menuTreeOutput);
            }
            return list;
        }
    }   
}
