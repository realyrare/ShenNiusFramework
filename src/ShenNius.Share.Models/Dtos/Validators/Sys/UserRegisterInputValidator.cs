using FluentValidation;
using ShenNius.Share.Models.Dtos.Input;

namespace ShenNius.Share.Models.Dtos.Validators
{
    public class UserRegisterInputValidator : AbstractValidator<UserRegisterInput>
    {
        public UserRegisterInputValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name).NotEmpty().WithMessage("请填写用户名称");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请填写用户密码");
            RuleFor(x => x.TrueName).NotEmpty().WithMessage("真实姓名必须填写");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("手机号必须填写");
            RuleFor(x => x.Sex).NotEmpty().WithMessage("您的性别必须填写");
            RuleFor(x => x.Email).NotEmpty().WithMessage("真实姓名必须填写");
        }
    }
}
