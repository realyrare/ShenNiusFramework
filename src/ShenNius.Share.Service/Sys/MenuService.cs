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
        Task<ApiResult> BtnCodeByMenuIdAsync(int menuId, int roleId);
        Task<ApiResult> TreeRoleIdAsync(int roleId);
    }
    public class MenuService : BaseServer<Menu>, IMenuService
    {
        public MenuService(IConfigService configService)
        {
           
        }
        public async Task<ApiResult> BtnCodeByMenuIdAsync(int menuId, int roleId)
        {
            if (menuId == 0)
            {
                return new ApiResult(menuId);
            }
            var menuModel = await GetModelAsync(d => d.Status && d.Id == menuId);
            if (menuModel == null)
            {
                throw new FriendlyException(nameof(menuModel));
            }
            if (menuModel.BtnCodeIds.Length <= 0|| menuModel.BtnCodeIds==null)
            {
                return new ApiResult(menuId);
            }
            var containsBtnList = await Db.Queryable<Config>().Where(d => menuModel.BtnCodeIds.Contains(d.Id.ToString())).Select(d => new ConfigBtnOutput()
            {
                Id = d.Id,
                Name = d.Name,
                Status = false
            }).ToListAsync();

            var permissionsModel = await Db.Queryable<R_Role_Menu>().Where(d => d.RoleId == roleId && d.MenuId == menuId).FirstAsync();

            if (permissionsModel != null &&  permissionsModel.BtnCodeIds!= null)
            {
                if (permissionsModel.BtnCodeIds.Length > 0)
                {
                    foreach (var item in containsBtnList)
                    {
                        if (permissionsModel.BtnCodeIds.Contains(item.Id.ToString()) && permissionsModel.BtnCodeIds != null)
                        {
                            item.Status = true;
                        }
                    }
                }                
            }
            return new ApiResult(containsBtnList);
        }
        public async Task<ApiResult> TreeRoleIdAsync(int roleId)
        {
            var list = new List<MenuTreeOutput>();
            var existMenuId = await Db.Queryable<R_Role_Menu>().Where(d => d.IsPass && d.RoleId == roleId).Select(d => d.MenuId).ToListAsync();

            var allMenus = await GetListAsync(d => d.Status);
            if (allMenus.Count <= 0 || allMenus == null)
            {
                return new ApiResult(allMenus);
            }

            foreach (var item in allMenus)
            {
                if (item.ParentId != 0)
                {
                    continue;
                }
                var menuTreeOutput = new MenuTreeOutput()
                {
                    Id = item.Id,
                    Title = item.Name,
                    Checked = existMenuId.FirstOrDefault(d => d == item.Id) != 0,
                    Children = AddChildNode(allMenus, item.Id, existMenuId),
                };
                list.Add(menuTreeOutput);
            }
            return new ApiResult(list);
        }
        private List<MenuTreeOutput> AddChildNode(IEnumerable<Menu> data, int parentId, List<int> existMenuId)
        {
            var list = new List<MenuTreeOutput>();
            var data2 = data.Where(d => d.ParentId == parentId).OrderBy(d => d.Name).ToList();
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
