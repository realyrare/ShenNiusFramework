using AutoMapper;
using Microsoft.AspNetCore.Http;
using NLog;
using ShenNius.Share.Common;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Output.Sys;
using ShenNius.Share.Models.Enums;
using ShenNius.Share.Models.Enums.Extension;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IUserService : IBaseServer<User>
    {
        Task<ApiResult<LoginOutput>> LoginAsync(LoginInput loginInput);
        Task<ApiResult> RegisterAsync(UserRegisterInput userRegisterInput);
        Task<ApiResult> ModfiyAsync(UserModifyInput userModifyInput);
        Task<ApiResult> ModfiyPwdAsync(ModifyPwdInput modifyPwdInput);
        Task<ApiResult> GetUserAsync(int id);

    }
    public partial class UserService : BaseServer<User>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ICacheHelper _cacheHelper;
        public UserService(IMapper mapper, IHttpContextAccessor httpContextAccessor, ICurrentUserContext currentUserContext, ICacheHelper cacheHelper)
        {
            _mapper = mapper;
            _accessor = httpContextAccessor;
            _currentUserContext = currentUserContext;
            _cacheHelper = cacheHelper;
        }

        public async Task<ApiResult<LoginOutput>> LoginAsync(LoginInput loginInput)
        {
            loginInput.Password = Md5Crypt.Encrypt(loginInput.Password);
            var loginModel = await GetModelAsync(d => d.Name.Equals(loginInput.LoginName) && d.Password.Equals(loginInput.Password));
            if (loginModel?.Id == 0)
            {
                new LogHelper().Process(loginModel?.Name, LogEnum.Login.GetEnumText(), $"{loginModel?.Name}登陆失败，用户名或密码错误！", LogLevel.Info);
                return new ApiResult<LoginOutput>("用户名或密码错误", 500);
            }
            if (!loginInput.ConfirmLogin)
            {
                if (loginModel.IsLogin)
                {
                    return new ApiResult<LoginOutput>($"该用户[{loginInput.LoginName}]已经登录，此时强行登录，其他地方会被挤下线！", 200);
                }
            }
            string ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string address = IpParseHelper.GetAddressByIP(ip);
            var lastLoginTime = DateTime.Now;
            await UpdateAsync(d => new User()
            {
                LastLoginTime = lastLoginTime,
                Ip = ip,
                Address = address,
                IsLogin = true
            }, d => d.Id == loginModel.Id);
            var data = _mapper.Map<LoginOutput>(loginModel);
            new LogHelper().Process(loginModel.Name, LogEnum.Login.GetEnumText(), $"{loginModel.Name}登陆成功！", LogLevel.Info);
            WebHelper.SendEmail("神牛系统用户登录", $"当前时间为{DateTime.Now}：有名为{loginModel.Name}的用户在{address}成功登录神牛系统", loginModel?.Name, loginModel?.Email);
            return new ApiResult<LoginOutput>(data);
        }
        public async Task<ApiResult> RegisterAsync(UserRegisterInput userRegisterInput)
        {
            userRegisterInput.Password = Md5Crypt.Encrypt(userRegisterInput.Password);
            var userModel = _mapper.Map<User>(userRegisterInput);
            userModel.CreateTime = DateTime.Now;
            userModel.Ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            userModel.Address = IpParseHelper.GetAddressByIP(userModel.Ip);
            var i = await AddAsync(userModel);
            return new ApiResult(i);
        }
        public async Task<ApiResult> ModfiyAsync(UserModifyInput userModifyInput)
        {
            userModifyInput.Password = Md5Crypt.Encrypt(userModifyInput.Password);

            var i = await UpdateAsync(d => new User()
            {
                Name = userModifyInput.Name,
                Password = userModifyInput.Password,
                Mobile = userModifyInput.Mobile,
                Status = userModifyInput.Status,
                Remark = userModifyInput.Remark,
                Email = userModifyInput.Email,
                TrueName = userModifyInput.TrueName,
                Sex = userModifyInput.Sex,
                ModifyTime = DateTime.Now
            },
            d => d.Id == userModifyInput.Id);

            return new ApiResult(i);
        }
        public async Task<ApiResult> ModfiyPwdAsync(ModifyPwdInput modifyPwdInput)
        {
            if (modifyPwdInput.Id == 0)
            {
                modifyPwdInput.Id = _currentUserContext.Id;
            }
            if (!modifyPwdInput.ConfirmPassword.Equals(modifyPwdInput.NewPassword))
            {
                throw new FriendlyException("两次输入的密码不一致");
            }
            modifyPwdInput.OldPassword = Md5Crypt.Encrypt(modifyPwdInput.OldPassword);
            var model = await GetModelAsync(d => d.Id == modifyPwdInput.Id);
            if (model.Id <= 0)
            {
                throw new FriendlyException("用户信息为空");
            }
            if (model.Password == modifyPwdInput.OldPassword)
            {
                throw new FriendlyException("旧密码错误!");
            }
            modifyPwdInput.ConfirmPassword = Md5Crypt.Encrypt(modifyPwdInput.ConfirmPassword);
            var i = await UpdateAsync(d => new User() { Password = modifyPwdInput.ConfirmPassword }, d => d.Id == modifyPwdInput.Id);
            return new ApiResult(i);
        }

        public async Task<ApiResult> GetUserAsync(int id)
        {
            var model = await GetModelAsync(d => d.Id == id);
            var data = _mapper.Map<UserOutput>(model);
            return new ApiResult(data);
        }

    }
}
