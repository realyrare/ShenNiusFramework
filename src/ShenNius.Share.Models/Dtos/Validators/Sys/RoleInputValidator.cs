﻿using FluentValidation;
using ShenNius.Share.Models.Dtos.Input;

namespace ShenNius.Share.Models.Dtos.Validators
{
    public class RoleInputValidator : AbstractValidator<RoleInput>
    {
        public RoleInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("请填写角色名称");
            RuleFor(x => x.Description).NotEmpty().WithMessage("请填写角色备注");
        }
    }
}
