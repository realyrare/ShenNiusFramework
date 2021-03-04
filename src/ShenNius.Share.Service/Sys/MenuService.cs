﻿using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
        Task<ApiResult> GetListPagesAsync(int page, string key = null);
        Task<ApiResult> ModifyAsync(MenuModifyInput menuModifyInput);

        Task<ApiResult> LoadLeftMenuTreesAsync(int userId);
        /// <summary>
        /// 根据当前用户Id获取所有角色关联的菜单id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<int>> GetCurrentMenuByUser(int userId);
        /// <summary>
        /// 当前用户所有的权限集合
        /// </summary>
        /// <returns></returns>
        Task<List<Menu>> GetCurrentAuthMenus();
        Task<ApiResult> GetAllParentMenuAsync();
    }
    public class MenuService : BaseServer<Menu>, IMenuService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserContext _currentUserContext;

        public MenuService(IMapper mapper, IMemoryCache cache, ICurrentUserContext currentUserContext)
        {
            _mapper = mapper;
            _cache = cache;
            _currentUserContext = currentUserContext;
        }
        public async Task<List<Menu>> GetCurrentAuthMenus()
        {
           var data= _cache.Get<List<Menu>>($"authMenu:{_currentUserContext.Id}");
            if (data==null)
            {
                var allMenus = await GetCurrentMenuByUser(_currentUserContext.Id);
                data = await Db.Queryable<Menu>().WhereIF(allMenus.Count > 0, d => allMenus.Contains(d.Id))
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
                       .ToListAsync();
                //把当前用户拥有的权限存入到缓存里面
                _cache.Set($"authMenu:{_currentUserContext.Id}", allMenus);
            }                    
            return data;
        }
        public async Task<ApiResult> GetListPagesAsync(int page, string key = null)
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

        public async Task<ApiResult> GetAllParentMenuAsync()
        {
          var list= await GetListAsync(d => d.Status);
          var data = new List<Menu>();
            ChildModule(list, data, 0);
  
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Name = WebHelper.LevelName(item.Name, item.Layer);
                }
            }
            return new ApiResult(data);
        }

        public async Task<ApiResult> AddToUpdateAsync(MenuInput menuInput)
        {
            var menu = _mapper.Map<Menu>(menuInput);
            var menuId = await AddAsync(menu);
            string parentIdList = ""; int layer = 0;
            if (menuInput.ParentId>0)
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

        public async Task<ApiResult> ModifyAsync(MenuModifyInput menuModifyInput)
        {
            string parentIdList = ""; int layer = 0;
            if (menuModifyInput.ParentId > 0)
            {
                // 说明有父级  根据父级，查询对应的模型
                var model = await GetModelAsync(d => d.Id == menuModifyInput.ParentId);
                if (model.Id > 0)
                {
                    parentIdList = model.ParentIdList + menuModifyInput.Id + ",";
                    layer = model.Layer + 1;
                }
            }
            else
            {
                parentIdList = "," + menuModifyInput.Id + ",";
                layer = 1;
            }
            await UpdateAsync(d => new Menu()
            {
                Name = menuModifyInput.Name,
                Url = menuModifyInput.Url,
                ModifyTime = menuModifyInput.ModifyTime,
                HttpMethod = menuModifyInput.HttpMethod,
                Status = menuModifyInput.Status,
                ParentId = menuModifyInput.ParentId,
                Icon = menuModifyInput.Icon,
                Sort = menuModifyInput.Sort,
                BtnCodeIds = menuModifyInput.BtnCodeIds,
                Layer = layer,
                ParentIdList = parentIdList
            }, d => d.Id == menuModifyInput.Id);
            return new ApiResult();
        }

        /// <summary>
        /// 根据菜单id获取关联按钮
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
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

        public async Task<List<int>> GetCurrentMenuByUser(int userId)
        {
            var allRoleIds = await Db.Queryable<R_User_Role>().Where(d => d.UserId == userId).Select(d => d.RoleId).ToListAsync();
            if (allRoleIds == null || allRoleIds.Count == 0)
            {
                throw new FriendlyException("当前用户没有角色授权");
            }
            var allMenuIds = await Db.Queryable<R_Role_Menu>().Where(d => allRoleIds.Contains(d.RoleId)).Select(d => d.MenuId).ToListAsync();
            if (allMenuIds == null || allMenuIds.Count == 0)
            {
                throw new FriendlyException("当前角色没有菜单授权");
            }
            return allMenuIds;
        }
        public async Task<ApiResult> LoadLeftMenuTreesAsync(int userId)
        {
            //var allRoleIds = await Db.Queryable<R_User_Role>().Where(d => d.UserId == userId).Select(d => d.RoleId).ToListAsync();
            //if (allRoleIds == null || allRoleIds.Count == 0)
            //{
            //    return new ApiResult("当前用户没有角色授权", 500);
            //}
            //var allMenuIds = await Db.Queryable<R_Role_Menu>().Where(d => allRoleIds.Contains(d.RoleId)).Select(d => d.MenuId).ToListAsync();
            //if (allMenuIds == null|| allMenuIds.Count==0)
            //{
            //    return new ApiResult("当前角色没有菜单授权", 500);
            //}
            var allMenuIds=  await GetCurrentMenuByUser(userId);
            var allMenus = await GetListAsync(d => d.Status&&allMenuIds.Contains(d.Id));
           
            var model = new MenuTreeInitOutput()
            {
                HomeInfo = new HomeInfo() { Title = "首页", Href = "page/welcome-1.html" },
               LogoInfo=new LogoInfo() { Title="神牛系统平台",Image= "images/logo.jpg?v=99", Href="" },
            };
            List<MenuInfo> menuInfos = new List<MenuInfo>();
            foreach (var item in allMenus)
            {
                if (item.ParentId != 0)
                {
                    continue;
                }
                var menuInfo = new MenuInfo()
                {
                    Title=item.Name,
                    Icon=item.Icon,
                    Target= "_self",
                    Href=item.Url,
                    Child= AddMenuChildNode(allMenus,item.Id)
                };
                menuInfos.Add(menuInfo);
            }
            model.MenuInfo = menuInfos;
            return new ApiResult(model);
        }
        private List<MenuInfo> AddMenuChildNode(List<Menu> data, int parentId)
        {
            var list = new List<MenuInfo>();
            var data2 = data.Where(d => d.ParentId == parentId).ToList();
            foreach (var item in data2)
            {
                var menuTreeOutput = new MenuInfo()
                {
                    Title = item.Name,
                    Icon = item.Icon,
                    Target = "_self",
                    Href = item.Url,
                    Child = AddMenuChildNode(data, item.Id)
                };
                list.Add(menuTreeOutput);
            }
            return list;
        }
    }
}