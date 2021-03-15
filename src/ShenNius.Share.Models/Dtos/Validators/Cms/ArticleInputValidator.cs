using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public  class ArticleInputValidator : AbstractValidator<ArticleInput>
    {
        public ArticleInputValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.KeyWord).NotEmpty().WithMessage("关键字必须填写");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("描述必须填写");
            RuleFor(x => x.Author).NotEmpty().WithMessage("文章作者必须填写");
            RuleFor(x => x.Source).NotEmpty().WithMessage("文章来源必须填写");
        }
    }
}
