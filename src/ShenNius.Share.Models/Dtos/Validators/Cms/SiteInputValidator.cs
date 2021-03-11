using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;
using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：SiteInputValidator
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 18:27:41
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public class SiteInputValidator : AbstractValidator<SiteInput>
    {
        public SiteInputValidator()
        {
            RuleFor(x =>x.Title).NotEmpty().WithMessage("Id必须传递");
            RuleFor(x => x.UserId>0).NotEmpty().WithMessage("Id必须传递");
        }
    }
}