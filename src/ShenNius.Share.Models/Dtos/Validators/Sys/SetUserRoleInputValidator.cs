using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Sys;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
    public  class SetUserRoleInputValidator : AbstractValidator<SetUserRoleInput>
    {
        public SetUserRoleInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("用户id必须填写");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("角色id必须填写一个");
           
        }
    }
}
