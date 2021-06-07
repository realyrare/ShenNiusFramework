using AutoMapper;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Infrastructure.Utils;
using ShenNius.Share.Infrastructure.ApiResponse;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.Extension;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

/*************************************
* 类名：KeywordController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/31 19:08:12
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Cms.API.Controllers
{
    public class KeywordController : ApiTenantBaseController<Keyword, DetailTenantQuery, DeletesTenantInput, ListTenantQuery, KeywordInput, KeywordModifyInput>
    {
        private readonly IBaseServer<Keyword> _service;

        public KeywordController(IBaseServer<Keyword> service, IMapper mapper) : base(service, mapper)
        {
            _service = service;
        }
        /// <summary>
        /// 一键匹配内链，只替换第一个匹配到的字符
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> ContentReplace([FromBody] ContentHrefInput input)
        {

            if (input.Type == "edit")
            {
                input.Content = WebHelper.ReplaceStrHref(input.Content);
            }

            var list = await _service.GetListAsync();
            if (list == null || list.Count == 0)
            {
                throw new FriendlyException("关键字库中没有数据,检查集合是否抛异常了");
            }
            // 第一次
            Regex reg = null;
            int n = -1;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < list.Count; i++)
            {
                if (Regex.Match(input.Content, list[i].Title).Success)
                {
                    string pattern = $"<a href=\"{list[i].Url}\" target=\"_blank\">{list[i].Title}</a>";
                    reg = new Regex(list[i].Title);
                    input.Content = reg.Replace(input.Content, pattern, 1);
                    if (Regex.Match(input.Content, pattern).Success)
                    {
                        //如果当前关键字链接信息成功，将当前的文本链接信息提取出来添加到字典中（dic）,并以唯一标识符代替文本链接信息作为占位符。
                        reg = new Regex(pattern);
                        n++;
                        input.Content = reg.Replace(input.Content, "{" + n + "}", 1);
                        dic.Add("{" + n + "}", pattern);
                    }
                }
            }
            //将匹配到的文本链接信息format
            if (dic.Count != 0)
            {
                int m = -1;
                foreach (string key in dic.Keys)
                {
                    m++;
                    if (input.Content.Contains("{" + m + "}"))
                    {
                        input.Content = input.Content.Replace("{" + m + "}", dic[key]);
                    }
                }
            }
            input.Content = WebHelper.RemoveStrImgAlt(input.Content);
            return new ApiResult(data: input.Content);
        }

    }
}