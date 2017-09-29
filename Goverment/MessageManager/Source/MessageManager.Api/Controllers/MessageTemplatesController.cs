// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplatesController.cs
// Created          : 2015-12-12  21:51
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-12  21:57
// ***********************************************************************
// <copyright file="MessageTemplatesController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.MessageManager.Api.Models;
using Jinyinmao.MessageManager.Domain.Bll;
using Jinyinmao.MessageManager.Domain.Entity;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web;
using MoeLib.Jinyinmao.Web.Filters;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.MessageManager.Api.Controllers
{
    /// <summary>
    ///     MessageTemplatesController.
    /// </summary>
    [RoutePrefix("api/MessageTemplates")]
    public class MessageTemplatesController : JinyinmaoApiController
    {
        /// <summary>
        ///     The message template service
        /// </summary>
        private readonly IMessageTemplateService messageTemplateService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageTemplatesController" /> class.
        /// </summary>
        /// <param name="messageTemplateService">The message template service.</param>
        public MessageTemplatesController(IMessageTemplateService messageTemplateService)
        {
            this.messageTemplateService = messageTemplateService;
        }

        /// <summary>
        ///     (A)创建模板
        /// </summary>
        /// <remarks>
        ///     创建模板
        /// </remarks>
        /// <param name="request">创建模板请求</param>
        [HttpPost]
        [Route("Create")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MessageTemplateResponse))]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Create(MessageTemplateRequest request)
        {
            MessageTemplate messageTemplate = await this.messageTemplateService.CreateAsync(request.ToEntity());

            if (messageTemplate == null)
            {
                return this.Conflict();
            }

            return this.Ok(messageTemplate.ToResponse());
        }

        /// <summary>
        ///     (A)返回模板列表
        /// </summary>
        /// <remarks>
        ///     返回模板列表
        /// </remarks>
        [HttpGet]
        [Route("messageTemplateList")]
        [ApplicationAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<MessageTemplateResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Index()
        {
            return this.Ok((await this.messageTemplateService.GetMessageTemplatesAsync()).Where(a => a.IsValid == 0).Select(t => t.ToResponse()));
        }

        /// <summary>
        ///     (A)逻辑删除模板
        /// </summary>
        /// <remarks>
        ///     逻辑删除模板
        /// </remarks>
        /// <param name="request">模板请求参数</param>
        [HttpPost]
        [Route("LogicDelete")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MessageTemplateResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> LogicDelete(MessageTemplateRequest request)
        {
            request.IsValid = 1;
            MessageTemplate messageTemplate = await this.messageTemplateService.UpdateAsync(request.ToEntity());

            if (messageTemplate == null)
            {
                return this.NotFound();
            }

            return this.Ok(messageTemplate.ToResponse());
        }

        /// <summary>
        ///     (A)更新模板
        /// </summary>
        /// <remarks>
        ///     更新模板
        /// </remarks>
        /// <param name="request">更新模板请求</param>
        [HttpPost]
        [Route("Update")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MessageTemplateResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Update(MessageTemplateRequest request)
        {
            MessageTemplate messageTemplate = await this.messageTemplateService.UpdateAsync(request.ToEntity());

            if (messageTemplate == null)
            {
                return this.NotFound();
            }

            return this.Ok(messageTemplate);
        }
    }
}