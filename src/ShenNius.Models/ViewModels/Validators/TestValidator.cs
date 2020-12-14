using FluentValidation;
using ShenNius.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Models.ViewModels.Validators
{
   public class TestValidator:AbstractValidator<Test>
    {
        public TestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Title).NotEmpty().WithMessage("请输入直播标题");
        }
    }
}
