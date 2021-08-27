//using AutoMapper.Configuration;
//using Microsoft.AspNetCore.Mvc;
//using ShenNius.Layui.Admin.Common;
//using ShenNius.Share.Domain.Services.Shop;
//using ShenNius.Share.Infrastructure.Caches;
//using ShenNius.Share.Models.Configs;
//using ShenNius.Share.Models.Dtos.Input.Shop;
//using ShenNius.Share.Models.Dtos.Output.MiniApp;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Web;

///*************************************
//* 类名：AppUserController
//* 作者：realyrare
//* 邮箱：mhg215@yeah.net
//* 时间：2021/8/27 16:37:29
//*┌───────────────────────────────────┐　    
//*│　   版权所有：神牛软件　　　　	     │
//*└───────────────────────────────────┘
//**************************************/

//namespace ShenNius.MiniApp.API.Controllers
//{
//    [Route("api/MiniApp/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly IAppUserService _memberService;
//        private readonly ICacheHelper _cacheHelper;
//        private readonly IOrderService _orderService;
//        private readonly HttpHelper _httpHelper;
//        private readonly IConfiguration _configuration;

//        public UserController(IAppUserService memberService, ICacheHelper cacheHelper, IOrderService orderService, HttpHelper httpHelper, IConfiguration configuration)
//        {
//            _memberService = memberService;
//            _cacheHelper = cacheHelper;
//            _orderService = orderService;
//            _httpHelper = httpHelper;
//            _configuration = configuration;
//        }

//        [HttpPost("login")]
//        public async Task<ApiResult> Login([FromForm] AppUserLoginInput input)
//        {
//            var result = new ApiResult();
//            try
//            {
//                // 微信登录 获取session_key
//                if (string.IsNullOrEmpty(input.Code))
//                {
//                    throw new ArgumentNullException($"{nameof(input.Code)} code为空");
//                }

//                //微信登录，根据code获得openid
//                //var wxres = await WxTools.GetOpenId(input.Code);
//                var url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + _configuration["WeiXin:AppID"] + "&secret=" + ConfigExtensions.Configuration["WeiXin:AppSecret"] + "&js_code=" + code + "&grant_type=authorization_code";
//                //Logger.Default.Process("wxApp", "info", "GetOpenId:" + url);
//                var resString = await _httpHelper.GetAsync<HttpMiniUser>(url);
//                if (wxres == null)
//                {
//                    throw new ArgumentNullException("获取用户实体信息为空");
//                }
//                if (!string.IsNullOrEmpty(wxres.errmsg))
//                {
//                    result.StatusCode = wxres.errcode;
//                    result.Msg = wxres.errmsg;
//                    return result;
//                }
//                // 自动注册用户
//                var userId = await WxAppLoginAndReg(wxres.openid, input.UserInfo);
//                // 生成token (session3rd)
//                var token = WxToken(input.TenantId, wxres.openid);
//                // 记录缓存, 7天
//                _cacheHelper.Set(token, wxres, TimeSpan.FromDays(7));
//                result.Data = new { userId, token };

//                return result;
//            }
//            catch (Exception e)
//            {
//                result.StatusCode = 500;
//                result.Success = false;
//                result.Message = e.Message;
//                return result;
//            }
//        }



//        private async Task<string> WxAppLoginAndReg(string openId, string userInfo)
//        {
//            var requestUser = JsonConvert.DeserializeObject<RequestUser>(userInfo.Replace("\\", ""));
//            //先判断是否存在
//            var dbRes = await _memberService.GetModelAsync(m => m.OpenId == openId);

//            dbRes.Data.NickName = requestUser.NickName;
//            dbRes.Data.Gender = requestUser.Gender;
//            dbRes.Data.HeadPic = requestUser.AvatarUrl;
//            dbRes.Data.Country = requestUser.Country;
//            dbRes.Data.Province = requestUser.Province;
//            dbRes.Data.City = requestUser.City;
//            dbRes.Data.LoginTime = DateTime.Now;
//            dbRes.Data.OpenId = openId;
//            if (dbRes.Data != null && !string.IsNullOrEmpty(dbRes.Data.Guid))
//            {
//                await _memberService.UpdateAsync(dbRes.Data);
//                return dbRes.Data.Guid;
//            }

//            //不存在，来注册
//            dbRes.Data.Guid = Guid.NewGuid().ToString();
//            dbRes.Data.Grade = "ce8691a1-2e7f-4866-abdc-36d0cb21aa97";
//            dbRes.Data.LoginName = "匿名：" + Utils.Number(6);
//            dbRes.Data.LoginPwd = DES3Encrypt.EncryptString("123456");
//            dbRes.Data.RegTime = DateTime.Now;
//            //保存到数据库
//            var sign = await _memberService.AddAsync(dbRes.Data);

//            return dbRes.Data.Guid;
//        }
//        private string WxToken(uint wxappId, string openId)
//        {
//            return $"{wxappId}_{DateTime.Now.ConvertToTimeStamp()}_{openId}_{Guid.NewGuid()}_${KeyHelper.EncryptKey}".GetMd5();
//        }


//    }
//}