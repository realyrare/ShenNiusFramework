using Senparc.CO2NET;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageContexts;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.IO;
using System.Threading.Tasks;

/*************************************
* 类名：CustomMessageHandler
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/6/8 19:52:41
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Wechat.API
{
    /// <summary>
    /// 自定义消息处理器
    /// </summary>
    //public class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>
    //{
    //    public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0, IServiceProvider serviceProvider = null)
    //        : base(inputStream, postModel, maxRecordCount, false, null, serviceProvider)
    //    {
    //    }

    //    /// <summary>
    //    /// 回复以文字形式发送的信息（可选）
    //    /// </summary>
    //    public override async Task<IResponseMessageBase> OnTextOrEventRequestAsync(RequestMessageText requestMessage)
    //    {
    //        var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
    //        await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(Config.SenparcWeixinSetting.MpSetting.WeixinAppId, OpenId, $"这是一条异步的客服消息");//注意：只有测试号或部署到正式环境的正式服务号可用此接口
    //        responseMessage.Content = $"你发送了文字：{requestMessage.Content}\r\n\r\n你的OpenId：{OpenId}";//以文字类型消息回复
    //        return responseMessage;
    //    }

    //    /// <summary>
    //    /// 默认消息
    //    /// </summary>
    //    public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
    //    {
    //        var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
    //        responseMessage.Content = $"欢迎来到我的公众号！当前时间：{SystemTime.Now}";//没有自定义的消息统一回复固定消息
    //        return responseMessage;
    //    }
    //}
}