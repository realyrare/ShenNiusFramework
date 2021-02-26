using AutoMapper;
using Newtonsoft.Json;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Infrastructure.Utils;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Dtos.Output.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Service.Repository;
using ShenNius.Share.Service.Repository.Extensions;
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

        Task<ApiResult> AddToUpdateAsync(MenuInput menuInput);
        Task<ApiResult> GetListPages(int page, string key = null);

    }
    public class MenuService : BaseServer<Menu>, IMenuService
    {
        private readonly IMapper _mapper;

        public MenuService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            var res = await Db.Queryable<Menu>().WhereIF(!string.IsNullOrEmpty(key), d => d.Name.Contains(key))
                      .OrderBy(m => m.Sort)
                          .Mapper((it, cache) =>
                          {
                              var codeList = cache.Get(t =>
                              {
                                  return Db.Queryable<Config>().Where(m => m.Type == "按钮").ToList();
                              });
                              var list = new List<string>();
                              if (it.BtnCodeIds != null)
                              {
                                  if (it.BtnCodeIds.Length > 0)
                                  {
                                      list = it.BtnCodeIds.ToList();
                                  }
                              }
                              if (list.Count > 0)
                              {
                                  it.BtnCodeName = string.Join(',', codeList.Where(g => list.Contains(g.Id.ToString())).Select(g => g.Name).ToList());
                              }
                          })
                   .ToPageAsync(page, 15);
            var result = new List<Menu>();
            if (!string.IsNullOrEmpty(key))
            {
                var menuModel = await GetModelAsync(m => m.Name.Contains(key));
                ChildModule(res.Items, result, menuModel.ParentId);
            }
            else
            {
                ChildModule(res.Items, result, 0);
            }

            //foreach (var item in res.Items)
            //{
            //    ChildModule(res.Items, result, item.ParentId);
            //}
            if (result?.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Name = WebHelper.LevelName(item.Name, item.Layer);
                }
            }
            return new ApiResult(data: new { count = res.TotalItems, items = result });
        }
        /// <summary>
        /// 递归模块列表
        /// </summary>
        private void ChildModule(List<Menu> list, List<Menu> newlist, int parentId)
        {
            var result = list.Where(p => p.ParentId == parentId).OrderBy(p => p.Layer).ThenBy(p => p.Sort).ToList();
            if (!result.Any()) return;
            for (int i = 0; i < result.Count(); i++)
            {
                newlist.Add(result[i]);
                ChildModule(list, newlist, result[i].Id);
            }
        }
        public async Task<ApiResult> AddToUpdateAsync(MenuInput menuInput)
        {
            var menu = _mapper.Map<Menu>(menuInput);
            var menuId = await AddAsync(menu);
            string parentIdList = ""; int layer = 0;
            if (!string.IsNullOrEmpty(menuInput.ParentId.ToString()))
            {
                // 说明有父级  根据父级，查询对应的模型
                var model = await GetModelAsync(d => d.Id == menuInput.ParentId);
                if (model.Id > 0)
                {
                    parentIdList = model.ParentIdList + menuId + ",";
                    layer = model.Layer + 1;
                }
            }
            else
            {
                parentIdList = "," + menuId + ",";
                layer = 1;
            }
            var i = await UpdateAsync(d => new Menu() { ParentIdList = parentIdList, Layer = layer }, d => d.Id == menuId);
            return new ApiResult(i);
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
            if (menuModel.BtnCodeIds.Length <= 0 || menuModel.BtnCodeIds == null)
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

            if (permissionsModel != null && permissionsModel.BtnCodeIds != null)
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
