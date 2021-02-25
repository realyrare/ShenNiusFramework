using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Sys;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
    public class RoleMenuBtnInputValidator : AbstractValidator<RoleMenuBtnInput>
    {
        public RoleMenuBtnInputValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("角色Id必须传递");
            RuleFor(x => x.MenuId).NotEmpty().WithMessage("菜单Id必须传递");
            RuleFor(x => x.BtnCodeId).NotEmpty().WithMessage("菜单按钮Id必须传递");
        }
    }
}
