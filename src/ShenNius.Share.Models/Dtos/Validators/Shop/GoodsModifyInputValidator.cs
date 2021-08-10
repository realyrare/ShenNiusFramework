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
    public class GoodsModifyInputValidator : AbstractValidator<GoodsModifyInput>
    {
        public GoodsModifyInputValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.TenantId).NotNull().GreaterThan(0).WithMessage("租户id必须大于0");
            RuleFor(x => x.ModifyTime).NotNull().WithMessage("修改时间必须填写");
            RuleFor(x => x.Content).NotEmpty().WithMessage("商品详情必须填写");

        }
    }
}