// ***********************************************************************
// Project          : MessageManager
// File             : SendMessageController.cs
// Created          : 2015-12-13  15:36
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-13  15:38
// ***********************************************************************
// <copyright file="SendMessageController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.Application.Constants;
using Jinyinmao.Application.ViewModel.MessageManager;
using Jinyinmao.Application.ViewModel.MessageManager.Enum;
using Jinyinmao.MessageManager.Api.Models;
using Jinyinmao.MessageManager.Domain.Bll;
using Jinyinmao.MessageManager.Domain.Entity;
using Moe.Lib;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web;
using MoeLib.Jinyinmao.Web.Diagnostics;
using MoeLib.Jinyinmao.Web.Filters;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.MessageManager.Api.Controllers
{
    /// <summary>
    ///     SendMessageController.
    /// </summary>
    [RoutePrefix("api/MessageManager")]
    public class SendMessageController : JinyinmaoApiController
    {
        private readonly ISendMessageService messageSendMessageService;

        private readonly IMessageTemplateService messageTemplateService;

        private readonly ISendMessageService sendMessageService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendMessageController" /> class.
        /// </summary>
        /// <param name="sendMessageServices">The send message services.</param>
        /// <param name="messageTemplateService"></param>
        /// <param name="messageSendMessageService"></param>
        public SendMessageController(ISendMessageService sendMessageServices, IMessageTemplateService messageTemplateService, ISendMessageService messageSendMessageService)
        {
            this.sendMessageService = sendMessageServices;
            this.messageTemplateService = messageTemplateService;
            this.messageSendMessageService = messageSendMessageService;
        }

        /// <summary>
        ///     (A)直接发送消息
        /// </summary>
        /// <remarks>
        ///     直接发送消息
        /// </remarks>
        /// <param name="request">发送到消息队列的参数</param>
        [HttpPost]
        [Route("Send")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SendMessageResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SendMessage(SendMessageRequest request)
        {
            await this.sendMessageService.SendMessageActionAsync(request.ToModel(this.Request.GetTraceEntry()));

            return this.Ok(new SendMessageResponse { Status = (long)SendMessageCode.Pass });
        }

        /// <summary>
        ///     (A)使用消息模板发送信息
        /// </summary>
        /// <remarks>
        ///     使用消息模板发送信息
        /// </remarks>
        /// <param name="request">发送模板请求</param>
        [HttpPost]
        [Route("SendWithTemplate")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MessageManagerResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SendWithTemplate(DispatcherRequest request)
        {
            MessageTemplate messageTemplate = await this.messageTemplateService.GetByBizcodeAsync(request.BizCode);
            if (messageTemplate == null)
            {
                return this.Ok(MessageManagerResponse.NoTemplates);
            }

            string content = this.messageTemplateService.FillTempalete(request.TemplateParams, messageTemplate);

            MessageModel[] models = request.UserInfoList.Select(userInfo => new MessageModel
            {
                Args = this.Request.GetTraceEntry().ToJson(),
                Cellphone = userInfo.PhoneNum,
                Message = content,
                Gateway = request.Gateway,
                Channel = request.MessageType.ToSmsChannel()
            }).ToArray();
            await this.messageSendMessageService.SendMessageActionAsync(models);

            return this.Ok(MessageManagerResponse.Success);
        }
    }
}