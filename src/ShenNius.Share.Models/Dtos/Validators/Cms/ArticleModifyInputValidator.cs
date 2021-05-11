using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;

/*************************************
* 类 名： ArticleModifyInputValidator
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 16:49:48
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public class ArticleModifyInputValidator : AbstractValidator<ArticleModifyInput>
    {
        public ArticleModifyInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id必须填写");
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.KeyWord).NotEmpty().WithMessage("关键字必须填写");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("描述必须填写");
            RuleFor(x => x.Author).NotEmpty().WithMessage("文章作者必须填写");
            RuleFor(x => x.Source).NotEmpty().WithMessage("文章来源必须填写");
        }
    }
}