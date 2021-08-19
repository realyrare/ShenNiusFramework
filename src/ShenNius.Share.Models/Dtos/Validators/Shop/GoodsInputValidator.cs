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
    public class GoodsInputValidator : AbstractValidator<GoodsInput>
    {
        public GoodsInputValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.CreateTime).NotNull().WithMessage("创建时间必须填写");
            RuleFor(x => x.Content).NotEmpty().WithMessage("商品详情必须填写");
            RuleFor(x => x.ImgUrl).NotEmpty().WithMessage("商品图片至少需要上传一张！");
            RuleFor(x => x.DeductStockType).NotEmpty().WithMessage("商品图片至少需要上传一张！");
            RuleFor(x=>x.CategoryId).NotEmpty().GreaterThan(0).WithMessage("商品分类必须选择！");
            RuleFor(x => x.SpecType).NotEmpty().WithMessage("库存计算方式必须选择！");
            RuleFor(x => x.GoodsStatus).NotEmpty().WithMessage("商品状态必须选择！");
        }
    }
}