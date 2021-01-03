
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ShenNius.Client.Admin.Components
{
    /// <summary>
    /// 仅加载资源时验证
    /// </summary>
    public partial class ResourceAuthorize
    {
        [Parameter]
        public RenderFragment ChildContent
        {
            get;
            set;
        }
        /// <summary>
        /// 未通过验证时展示
        /// </summary>
        [Parameter]
        public RenderFragment NotAuthorized
        {
            get;
            set;
        }
        /// <summary>
        /// 通过验证时展示
        /// </summary>
        [Parameter]
        public RenderFragment Authorized
        {
            get;
            set;
        }
        /// <summary>
        /// 验证中展示
        /// </summary>
        [Parameter]
        public RenderFragment Authorizing
        {
            get;
            set;
        }
        /// <summary>
        /// 用户要拥有资源的，资源key
        /// </summary>
        [Parameter]
        public string ResourceKey { get; set; }
        //[Inject]
       // private IAuthenticationStateManager authenticationStateManager { get; set; }
        /// <summary>
        /// 0 ing
        /// -1 false
        /// 1 true
        /// </summary>
        private short state = 0;


        /// <summary>
        /// 页面初始化完成
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
           // var isAuth = await authenticationStateManager.CheckCurrentUserHaveBtnResourceKey(ResourceKey);
           // state = isAuth ? 1 : -1;
        }
    }
}
