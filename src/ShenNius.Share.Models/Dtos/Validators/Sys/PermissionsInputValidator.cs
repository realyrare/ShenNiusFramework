using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
  public  class PermissionsInputValidator : AbstractValidator<PermissionsInput>
    {
        public PermissionsInputValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("角色Id必须传递");
            RuleFor(x => x.MenuId).NotEmpty().WithMessage("菜单Id必须传递");
        }
    }
}
