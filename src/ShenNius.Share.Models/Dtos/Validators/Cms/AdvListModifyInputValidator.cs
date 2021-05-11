using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;

/*************************************
* 类 名： AdvListModifyInputValidator
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 18:03:52
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public class AdvListModifyInputValidator : AbstractValidator<AdvListModifyInput>
    {
        public AdvListModifyInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id必须填写");
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.Target).NotEmpty().WithMessage("跳转方式必须填写");
            RuleFor(x => x.Type).NotEmpty().WithMessage("类型必须填写");
            RuleFor(x => x.ImgUrl).NotEmpty().WithMessage("图片地址必须填写");
        }
    }
}