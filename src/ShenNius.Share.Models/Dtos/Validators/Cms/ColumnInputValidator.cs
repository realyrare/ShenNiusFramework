using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;

/*************************************
* 类 名： ColumnInputValidator
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/15 19:28:24
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public class ColumnInputValidator : AbstractValidator<ColumnInput>
    {
        public ColumnInputValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.Keyword).NotEmpty().WithMessage("站点关键字必须填写");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("站点描述必须填写");
        }
    }
}