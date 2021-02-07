using AutoMapper;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Output;

namespace ShenNius.Share.Service
{
    public  class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, LoginOutput>().ForMember(d=>d.LoginName,s=>s.MapFrom(i=>i.Name));
            CreateMap<User, UserOutput>();
            CreateMap<UserRegisterInput,User>();

            CreateMap<RoleInput, Role>();
            CreateMap<RoleModifyInput,Role>();

        }
    }
}
