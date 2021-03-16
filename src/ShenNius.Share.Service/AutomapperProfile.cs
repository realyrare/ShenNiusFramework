using AutoMapper;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Dtos.Output;
using ShenNius.Share.Models.Dtos.Output.Sys;
using ShenNius.Share.Models.Entity.Cms;

namespace ShenNius.Share.Service
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //sys
            CreateMap<User, LoginOutput>().ForMember(d => d.LoginName, s => s.MapFrom(i => i.Name));
            CreateMap<User, UserOutput>();
            CreateMap<UserRegisterInput, User>();

            CreateMap<RoleInput, Role>();
            CreateMap<RoleModifyInput, Role>();

            CreateMap<MenuModifyInput, Menu>();
            CreateMap<MenuInput, Menu>();
            //ParentMenuOutput
            CreateMap<Menu, ParentMenuOutput>();
            CreateMap<Menu, MenuAuthOutput>();

            CreateMap<ConfigInput, Config>();
            //cms
            CreateMap<SiteInput, Site>();
            CreateMap<ColumnInput, Column>();
            CreateMap<ArticleInput, Article>();
            CreateMap<ArticleModifyInput, Article>();
        }
    }
}
