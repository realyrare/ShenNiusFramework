using FluentValidation;
using ShenNius.Share.Models.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
   public class RoleModifyInputValidator : AbstractValidator<RoleModifyInput>
    {
        public RoleModifyInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("角色Id必须传递");
            RuleFor(x => x.Name).NotEmpty().WithMessage("请填写角色名称");
            RuleFor(x => x.Description).NotEmpty().WithMessage("请填写角色备注");
        }
    }
}
