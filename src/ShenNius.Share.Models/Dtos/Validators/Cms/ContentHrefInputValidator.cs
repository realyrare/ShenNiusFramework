﻿using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;

/*************************************
* 类 名： ContentHrefInputValidator
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 17:35:06
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public class ContentHrefInputValidator : AbstractValidator<ContentHrefInput>
    {
        public ContentHrefInputValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("标题必须填写");
        }
    }
}