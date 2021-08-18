using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Shop;

/*************************************
* 类名：CategoryInputValidator
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/10 9:46:45
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Shop
{
    public class CategoryInputValidator : AbstractValidator<CategoryInput>
    {
        public CategoryInputValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.CreateTime).NotNull().WithMessage("创建时间必须填写");
           
        }
    }
}