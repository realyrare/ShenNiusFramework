using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Service.Sys
{
    public interface IMenuService : IBaseServer<Menu>
    {

    }
    public class MenuService : BaseServer<Menu>, IMenuService
    {
    }   
}
