using FluentValidation;
using ShenNius.Share.Models.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Validators
{
   public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
           // CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Loginname).NotEmpty().WithMessage("请填写用户名称");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请填写用户密码");
            RuleFor(x => x.NumberGuid).NotEmpty().WithMessage("用户编号必须传递");
        }
    }
}
