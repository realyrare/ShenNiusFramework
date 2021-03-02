using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Sys;

namespace ShenNius.Share.Models.Dtos.Validators.Sys
{
    public class CommonDeleteInputValidator : AbstractValidator<CommonDeleteInput>
    {
        public CommonDeleteInputValidator()
        {
            RuleFor(x => x.Ids.Count).NotEmpty().WithMessage("Id必须传递");
        }
    }
}
