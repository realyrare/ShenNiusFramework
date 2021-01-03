using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using ShenNius.Client.Admin.Dtos;
using ShenNius.Client.Admin.Extensions;
using ShenNius.Client.Admin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShenNius.Client.Admin.Pages.Sys
{
    public partial class User
    {
        ITable _table;
        UserDto[] _users;
        IEnumerable<UserDto> _selectedRows;
        int _pageIndex = 1;
        int _pageSize = 10;
        int _total = 0;
        string _name = string.Empty;
        bool _tableIsLoading = false;
        [Inject]
        MessageService messageService { get; set; }
        [Inject]
        public HttpClient http { get; set; }
        [Inject]
        ConfirmService confirmService { get; set; }
        [Inject]
        DrawerService drawerService { get; set; }
        /// <summary>
        /// 页面初始化完成
        /// </summary>
        /// <returns></returns>
        protected override void OnInitialized()
        {
            messageService.Config(new MessageGlobalConfig()
            {
                Top = 24,
                Duration = 1,
                MaxCount = 3,
                Rtl = true,
            });
        }
        /// <summary>
        /// 重新加载table
        /// </summary>
        /// <returns></returns>
        private async Task ReLoadTable()
        {
            _tableIsLoading = true;
            var pagedListResult = await http.GetFromJsonAsync<ApiResult<Page<UserDto>>>($"user/getlistpages?page={_pageIndex}&key={_name}");
            if (pagedListResult != null)
            {
                var pagedList = pagedListResult;
                _users = pagedList.data.item.ToArray();
                _total = pagedList.data.count;
            }
            else
            {
               await messageService.Error("加载失败");
            }
            _tableIsLoading = false;
        }
        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <returns></returns>
        private async Task OnReLoadTable()
        {
            await ReLoadTable();
        }
        /// <summary>
        /// 查询变化
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        private async Task onChange(QueryModel<UserDto> queryModel)
        {
            await ReLoadTable();
        }
        /// <summary>
        /// 点击删除按钮
        /// </summary>
        /// <param name="id"></param>
        private async Task OnDeleteClick(int id)
        {
            if (await confirmService.YesNoDelete() == ConfirmResult.Yes)
            {
                //var result = await userService.FakeDelete(id);
                //if (result)
                //{
                //    _users = _users.Remove(_users.FirstOrDefault(x => x.Id == id));
                //    messageService.Success("删除成功");
                //}
                //else
                //{
                //    messageService.Error("删除失败");
                //}
                //await InvokeAsync(StateHasChanged);
            }

        }
        /// <summary>
        /// 点击编辑按钮
        /// </summary>
        /// <param name="model"></param>
        private async Task OnEditClick(int userId)
        {
            //var result = await drawerService.CreateDialogAsync<UserEdit, int, bool>(userId, true, title: "编辑", width: 500);

            //if (result)
            //{
            //    await ReLoadTable();
            //}
        }
        /// <summary>
        /// 点击添加按钮
        /// </summary>
        private async Task OnAddClick()
        {
            //var result = await drawerService.CreateDialogAsync<UserEdit, int, bool>(0, true, title: "添加", width: 500);

            //if (result)
            //{
            //    //刷新列表
            //    _pageIndex = 1;
            //    _name = string.Empty;
            //    await ReLoadTable();
            //}
        }
        /// <summary>
        /// 点击删除选中按钮
        /// </summary>
        private async Task OnDeletesClick()
        {
            //if (_selectedRows == null || _selectedRows.Count() == 0)
            //{
            //    messageService.Warn("未选中任何行");
            //}
            //else
            //{
            //    if (await confirmService.YesNoDelete() == ConfirmResult.Yes)
            //    {
            //        var result = await userService.FakeDeletes(_selectedRows.Select(x => x.Id).ToArray());
            //        if (result)
            //        {
            //            _users = _users.Where(x => !_selectedRows.Any(y => y.Id == x.Id)).ToArray();
            //            messageService.Success("删除成功");
            //        }
            //        else
            //        {
            //            messageService.Error($"删除失败");
            //        }
            //        //await InvokeAsync(StateHasChanged);
            //    }
            //}
        }
        /// <summary>
        /// 点击锁定按钮
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isLocked"></param>
        private async Task OnChangeIsLocked(UserDto model, bool isLocked)
        {
            //Task.Run(async () =>
            //{
            //    var result = await userService.Lock(model.Id, isLocked);
            //    if (!result)
            //    {
            //        model.IsLocked = !isLocked;
            //        messageService.Error("锁定失败");
            //    }
            //});
        }

        /// <summary>
        /// 点击分配角色
        /// </summary>
        /// <param name="userId"></param>
        private async Task OnEditUserRoleClick(int userId)
        {
            //var result = await drawerService.CreateDialogAsync<UserRoleEdit, int, bool>(userId, true, title: "分配角色", width: 500);

            //if (result)
            //{
            //    await ReLoadTable();
            //}
        }
        /// <summary>
        /// 点击头像
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task OnAvatarClick(UserDto user)
        {
            int avatarDrawerWidth = 300;
            //await drawerService.CreateDialogAsync<UserUploadAvatar, UserUploadAvatarParams, string>(new UserUploadAvatarParams { User = user, SaveDb = true }, true, title: "上传头像", width: avatarDrawerWidth, placement: "left");
        }
    }
}


