﻿using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Entity.Shop;

/*************************************
* 类名：AppUserService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/20 11:53:40
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Shop
{
    public interface IAppUserService : IBaseServer<AppUser>
    {
    }
    public class AppUserService : BaseServer<AppUser>, IAppUserService
    {

    }
}