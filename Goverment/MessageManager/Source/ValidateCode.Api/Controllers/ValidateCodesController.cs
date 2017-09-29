// ***********************************************************************
// Project          : MessageManager
// File             : ValidateCodesController.cs
// Created          : 2015-12-10  10:30
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-10  10:41
// ***********************************************************************
// <copyright file="ValidateCodesController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.Application.ViewModel.ValicodeManager;
using Jinyinmao.ValidateCode.Api.Models;
using Jinyinmao.ValidateCode.Domain.Bll;
using Jinyinmao.ValidateCode.Domain.Entity;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web;
using MoeLib.Jinyinmao.Web.Diagnostics;
using MoeLib.Jinyinmao.Web.Filters;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.ValidateCode.Api.Controllers
{
    /// <summary>
    ///     ValidateCodesController.
    /// </summary>
    [RoutePrefix("api/ValidateCodes")]
    public class ValidateCodesController : JinyinmaoApiController
    {
        private readonly IValidateCodeService validateCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidateCodesController" /> class.
        /// </summary>
        /// <param name="validateCodeService">The validate code service.</param>
        public ValidateCodesController(IValidateCodeService validateCodeService)
        {
            this.validateCodeService = validateCodeService;
        }

        /// <summary>
        ///     (P)根据Token查询具体的验证码有没有被使用
        /// </summary>
        /// <remarks>
        ///     根据Token查询具体的验证码有没有被使用
        /// </remarks>
        /// <param name="token">
        ///     验签
        /// </param>
        [HttpGet]
        [Route("GetByToken")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, "{result: true/false}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:MMVCGBT")] // Token不存在
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetByToken(string token)
        {
            VeriCode veriCode = await this.validateCodeService.GetVeriCodeByTokenAsync(token);
            if (veriCode == null)
            {
                return this.BadRequest(App.Condigurations.GetResourceString("err:MMVCGBT"));
            }
            return this.Ok(new { result = veriCode.Used });
        }

        /// <summary>
        ///     (P)发送验证码
        /// </summary>
        /// <remarks>
        ///     一天(自然天)同种类型的验证码只能发送10次，防止恶意使用验证码
        /// </remarks>
        /// <param name="request">
        ///     验证码发送请求
        /// </param>
        [HttpPost]
        [Route("Send")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SendVeriCodeResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Send(SendVeriCodeRequest request)
        {
            return this.Ok((await this.validateCodeService.SendAsync(request.Cellphone, request.Type.ToVeriCodeType(), this.Request.GetTraceEntry())).ToResponse());
        }

        /// <summary>
        ///     (U/A)发送重置密码验证码，主要用于支付密码，需要用户登录
        /// </summary>
        /// <remarks>
        ///     一天(自然天)同种类型的验证码只能发送10次，防止恶意使用验证码，主要用于重置登录密码和支付密码，需要用户登录
        /// </remarks>
        /// <param name="request">
        ///     验证码发送请求
        /// </param>
        [HttpPost]
        [Route("Send/ResetPassword")]
        [Authorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SendVeriCodeResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err_MMVCSFRP")] // 发送的手机号不正确
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SendForResetPassword(SendVeriCodeForResetPasswordRequest request)
        {
            return this.Ok((await this.validateCodeService.SendAsync(request.Cellphone, request.Type.ToVeriCodeType(), this.Request.GetTraceEntry())).ToResponse());
        }

        /// <summary>
        ///     (A)Token使用
        /// </summary>
        /// <remarks>
        ///     使用验证码Token获取验证码对应的手机号
        /// </remarks>
        /// <param name="useVeriCodeRequest">
        ///     Token验证请求
        /// </param>
        [HttpPost]
        [Route("Use")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UseVeriCodeResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Use(UseVeriCodeRequest useVeriCodeRequest)
        {
            VeriCode veriCode = await this.validateCodeService.UseVeriCodeAsync(useVeriCodeRequest.Token, useVeriCodeRequest.Type.ToVeriCodeType());
            UseVeriCodeResponse result = new UseVeriCodeResponse
            {
                Cellphone = string.Empty,
                Result = false
            };

            if (veriCode != null)
            {
                result.Cellphone = veriCode.Cellphone;
                result.Result = true;
            }

            return this.Ok(result);
        }

        /// <summary>
        ///     (P)验证验证码是否正确
        /// </summary>
        /// <remarks>
        ///     验证码只能验证失败3次，并且只能使用一次，验证码有效期为30分钟(可配置)
        /// </remarks>
        /// <param name="request">
        ///     验证码验证请求
        /// </param>
        [HttpPost]
        [Route("Verify")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VerifyVeriCodeResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> VerifyCode(VerifyVeriCodeRequest request)
        {
            return this.Ok((await this.validateCodeService.VerifyAsync(request.Cellphone, request.Code, request.Type.ToVeriCodeType())).ToResponse());
        }
    }
}