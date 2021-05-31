using ShenNius.Share.Models.Entity.Common;
using SqlSugar;
using System;

/*************************************
* 类名：Tenant
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:10:38
*┌───────────────────────────────────┐　    
*│　      版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Sys
{
    [SugarTable("Sys_Tenant")]
    public class Tenant : BaseEntity
    {

        /// <summary>
        /// Desc:系统用户ID
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Desc:网站名称
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Desc:网站域名
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Desc:网站Logo
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Desc:网站描述
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Desc:公司电话
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// Desc:公司传真
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Desc:公司人事邮箱
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Desc:公司客服QQ
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// Desc:微信公众号图片
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string WeiXin { get; set; }

        /// <summary>
        /// Desc:微博链接地址或者二维码
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string WeiBo { get; set; }

        /// <summary>
        /// Desc:公司地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Desc:网站备案号其它等信息
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Desc:网站SEO标题
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Desc:网站SEO关键字
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Desc:网站SEO描述
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Desc:网站版权等信息
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Desc:如果状态关闭，请输入关闭网站原因
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string CloseInfo { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        public bool IsCurrent { get; set; }
    }
}