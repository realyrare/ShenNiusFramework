using FluentValidation;
using ShenNius.Share.Models.Dtos.Input;

namespace ShenNius.Share.Models.Dtos.Validators
{
    public  class ModifyPwdInputValidator : AbstractValidator<ModifyPwdInput>
    {
        public ModifyPwdInputValidator()
        {
            // CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).NotEmpty().WithMessage("请填写用户Id");
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("请填写用户旧密码");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("请填写用户新密码");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("确认密码必须填写");

        }
    }
}
