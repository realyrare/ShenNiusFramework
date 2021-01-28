using AutoMapper;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Output;
using ShenNius.Share.Service.Repository;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Sys
{
    public interface IUserService : IBaseServer<User>
    {
        Task<ApiResult<LoginOutput>> Login(LoginInput loginInput);


    }
    public partial class UserService : BaseServer<User>, IUserService
    {
        private readonly IMapper _mapper;
        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ApiResult<LoginOutput>> Login(LoginInput loginInput)
        {
            var loginModel = await GetModelAsync(d => d.Name.Equals(loginInput.loginname) && d.Password.Equals(loginInput.password));
            if (loginModel == null)
            {
                return new ApiResult<LoginOutput>(null, statusCode: 400, success: false, msg: "用户名或密码错误");
            }
            var data = _mapper.Map<LoginOutput>(loginModel);
            return new ApiResult<LoginOutput>(data);
        }
    }
}
