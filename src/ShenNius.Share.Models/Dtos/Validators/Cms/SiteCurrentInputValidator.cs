using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;
using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类 名： SiteCurrentInputValidator
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/18 17:40:26
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public class SiteCurrentInputValidator : AbstractValidator<SiteCurrentInput>
    {
        public SiteCurrentInputValidator()
        {
            RuleFor(x =>x.Id).NotEmpty().WithMessage("Id不能为空!");
        }
    }
}