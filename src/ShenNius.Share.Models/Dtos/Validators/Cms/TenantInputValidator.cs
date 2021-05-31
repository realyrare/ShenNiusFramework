using FluentValidation;
using ShenNius.Share.Models.Dtos.Input.Cms;
using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：TenantInputValidator
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 18:27:41
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Validators.Cms
{
    public class TenantInputValidator : AbstractValidator<TenantInput>
    {
        public TenantInputValidator()
        {
            RuleFor(x =>x.Title).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.Name).NotEmpty().WithMessage("站点名称必须填写");
            RuleFor(x => x.Keyword).NotEmpty().WithMessage("站点关键字必须填写");
            RuleFor(x => x.Description).NotEmpty().WithMessage("站点描述必须填写");
            RuleFor(x => x.Email).NotEmpty().WithMessage("站点邮箱必须填写");           
        }
    }
}