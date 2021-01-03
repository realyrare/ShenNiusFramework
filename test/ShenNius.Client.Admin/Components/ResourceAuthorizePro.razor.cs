
using ShenNius.Client.Admin.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ShenNius.Client.Admin.Components
{
    /// <summary>
    /// 页面有任何渲染变化，都会重新触发验证
    /// </summary>
    public partial  class ResourceAuthorizePro
    {
        [Parameter]
        public RenderFragment<AuthenticationState> ChildContent
        {
            get;
            set;
        }
        /// <summary>
        /// 未通过验证时展示
        /// </summary>
        [Parameter]
        public RenderFragment<AuthenticationState> NotAuthorized
        {
            get;
            set;
        }
        /// <summary>
        /// 通过验证时展示
        /// </summary>
        [Parameter]
        public RenderFragment<AuthenticationState> Authorized
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
        private string Policy = AuthConstant.ClientUIResourcePolicy;
        /// <summary>
        /// 用户要拥有资源的，资源key
        /// </summary>
        [Parameter]
        public string ResourceKey { get; set; }
    }
}
