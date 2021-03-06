using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Utils;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Output;
using ShenNius.Share.Service.Repository;
using ShenNiusSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Sys
{
    public interface IUserService : IBaseServer<User>
    {
        Task<ApiResult<LoginOutput>> LoginAsync(LoginInput loginInput);
        Task<ApiResult> RegisterAsync(UserRegisterInput userRegisterInput);
        Task<ApiResult> ModfiyAsync(UserModifyInput userModifyInput);
        Task<ApiResult> ModfiyPwdAsync(ModifyPwdInput modifyPwdInput);
        Task<ApiResult> DeletesAsync(List<int> ids);
        Task<ApiResult> GetUserAsync(int id);

    }
    public partial class UserService : BaseServer<User>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICurrentUserContext _currentUserContext;

        public UserService(IMapper mapper, IHttpContextAccessor httpContextAccessor, ICurrentUserContext currentUserContext)
        {
            _mapper = mapper;
            _accessor = httpContextAccessor;
            _currentUserContext = currentUserContext;
        }



        public async Task<ApiResult<LoginOutput>> LoginAsync(LoginInput loginInput)
        {
            loginInput.Password = Md5Crypt.Encrypt(loginInput.Password);
            var loginModel = await GetModelAsync(d => d.Name.Equals(loginInput.LoginName) && d.Password.Equals(loginInput.Password));
            if (loginModel.Id == 0)
            {
                return new ApiResult<LoginOutput>("用户名或密码错误",500);
            }
            string ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string address = IpParse.GetAddressByIP(ip);
            await UpdateAsync(d => new User()
            {
                LastLoginTime = DateTime.Now,
                Ip = ip,
                Address = address
            }, d => d.Id == loginModel.Id);
            var data = _mapper.Map<LoginOutput>(loginModel);
            return new ApiResult<LoginOutput>(data);
        }
        public async Task<ApiResult> RegisterAsync(UserRegisterInput userRegisterInput)
        {
            userRegisterInput.Password = Md5Crypt.Encrypt(userRegisterInput.Password);
            var userModel = _mapper.Map<User>(userRegisterInput);
            userModel.CreateTime = DateTime.Now;
            userModel.Ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            userModel.Address = IpParse.GetAddressByIP(userModel.Ip);
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
                Sex = userModifyInput.Sex
            },
            d => d.Id == userModifyInput.Id);

            return new ApiResult(i);
        }
        public async Task<ApiResult> ModfiyPwdAsync(ModifyPwdInput modifyPwdInput)
        {
            if (modifyPwdInput.Id==0)
            {
                modifyPwdInput.Id = _currentUserContext.Id;
            }
            if (!modifyPwdInput.ConfirmPassword.Equals(modifyPwdInput.NewPassword))
            {
                throw new ArgumentNullException("两次输入的密码不一致");
            }
            modifyPwdInput.OldPassword = Md5Crypt.Encrypt(modifyPwdInput.OldPassword);
            var model = await GetModelAsync(d => d.Id == modifyPwdInput.Id);
            if (model.Id<=0)
            {
                throw new ArgumentNullException("用户信息为空");
            }
            if (model.Password == modifyPwdInput.OldPassword)
            {
                throw new ArgumentNullException("旧密码错误!");
            }
            modifyPwdInput.ConfirmPassword = Md5Crypt.Encrypt(modifyPwdInput.ConfirmPassword);
            var i = await UpdateAsync(d => new User() { Password = modifyPwdInput.ConfirmPassword }, d => d.Id == modifyPwdInput.Id);
            return new ApiResult(i);
        }
        public async Task<ApiResult> DeletesAsync(List<int> ids)
        {
            if (ids.Count == 0 || ids == null)
            {
                throw new ArgumentNullException("传递的id为空");
            }
            return new ApiResult(await DeleteAsync(ids));
        }
        public async Task<ApiResult> GetUserAsync(int id)
        {
            var model= await GetModelAsync(d=>d.Id==id);
           var data= _mapper.Map<UserOutput>(model);
            return new ApiResult(data);
        }
       
    }
}
