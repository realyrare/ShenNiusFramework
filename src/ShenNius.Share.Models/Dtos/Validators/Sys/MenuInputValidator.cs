using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Sys;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
    public class MenuInputValidator : AbstractValidator<MenuInput>
    {
        public MenuInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("菜单名称必须填写");
            //有些顶级菜单不用要验证
           // RuleFor(x => x.Url).NotEmpty().WithMessage("Url必须填写");
            RuleFor(x => x.HttpMethod).NotEmpty().WithMessage("HttpMethod必须填写");
            RuleFor(x => x.Icon).NotEmpty().WithMessage("菜单图标必须填写");
            RuleFor(x => x.NameCode).NotEmpty().WithMessage("菜单唯一码必须填写");
        }
    }
}
