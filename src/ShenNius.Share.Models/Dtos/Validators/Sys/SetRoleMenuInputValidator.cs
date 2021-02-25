using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Sys;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
    public class SetRoleMenuInputValidator : AbstractValidator<SetRoleMenuInput>
    {
        public SetRoleMenuInputValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("用户id必须填写");
            RuleFor(x => x.MenuIds).NotEmpty().WithMessage("角色id至少填写一个");

        }
    }
}
