using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Sys;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
    public class MenuModifyInputValidator : AbstractValidator<MenuModifyInput>
    {
        public MenuModifyInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("菜单名称必须填写");
            RuleFor(x => x.Name).NotEmpty().WithMessage("菜单名称必须填写");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Url必须填写");
            RuleFor(x => x.HttpMethod).NotEmpty().WithMessage("HttpMethod必须填写");
            RuleFor(x => x.Icon).NotEmpty().WithMessage("菜单图标必须填写");
        }
    }
}
