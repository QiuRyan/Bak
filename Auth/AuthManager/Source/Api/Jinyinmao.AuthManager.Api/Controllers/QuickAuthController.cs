// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : QuickAuthController.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-01-10  20:39
// ***********************************************************************
// <copyright file="QuickAuthController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.AuthManager.Api.Helper;
using Jinyinmao.AuthManager.Api.Models.Request;
using Jinyinmao.AuthManager.Api.Models.Response;
using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Libraries.Parameter;
using Jinyinmao.AuthManager.Service.Coupon.Interface;
using Jinyinmao.AuthManager.Service.Misc.Interface;
using Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult;
using Jinyinmao.AuthManager.Service.User.Interface;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web.Auth;
using MoeLib.Jinyinmao.Web.Diagnostics;
using MoeLib.Jinyinmao.Web.Filters;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.AuthManager.Api.Controllers
{
    /// <summary>
    ///     Class QuickAuthController.
    /// </summary>
    [RoutePrefix("api/User/Auth/Quick")]
    public class QuickAuthController : ApiControllerBase
    {
        /// <summary>
        ///     The user service
        /// </summary>
        /// <value>The user service.</value>
        private readonly IUserService userService;

        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        public QuickAuthController(IUserService userService, IVeriCodeService veriCodeService, IMessageRoleService messageRoleService)
        {
            this.userService = userService;
            this.veriCodeService = veriCodeService;
            this.MessageRoleService = messageRoleService;
        }

        private IMessageRoleService MessageRoleService { get; }

        /// <summary>
        ///     (P)快速登录
        /// </summary>
        /// <param name="request">快速登录请求.</param>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     AMQAQSI1:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMQAQSI2:未能加载用户信息，请稍后再试
        /// </response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer =="
        ///     <br />
        ///     登录成功返回加密token，token为字符串
        /// </remarks>
        [Route("SignIn")]
        [AuthorizationRequired("Bearer")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSI1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSI2")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> QuickSignIn(QuickSignInRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.QuickSignIn.Code, this.Request.GetTraceEntry());
            if (!result.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMQAQSI1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSI1"));
            }

            UserInfo userInfo = await this.userService.GetUserByCellphoneAsync(result.Cellphone);
            if (userInfo == null)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMQAQSI2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSI2"));
            }

            this.BuildPrincipal(userInfo.UserId.ToGuidString(), JYMAuthScheme.Bearer);
            return this.Ok();
        }

        /// <summary>
        ///     (P)快速注册.
        /// </summary>
        /// <param name="request">快速注册请求.</param>
        /// <response code="400">
        ///     AMQAQSU1:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMQAQSU2:此号码已注册，请直接登录
        ///     <br />
        ///     AMQAQSU3:注册失败
        /// </response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer =="
        ///     <br />
        ///     注册成功登录，登录返回加密token，token为字符串
        /// </remarks>
        [Route("SignUp")]
        [AuthorizationRequired("Bearer")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSU1")] //
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSU2")] //
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSU3")] //
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> QuickSignUp(QuickSignUpRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.SignUp.Code, this.Request.GetTraceEntry());
            if (!result.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMQAQSU1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSU1"));
            }

            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(result.Cellphone);
            if (checkCellphoneResult.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMQAQSU2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSU2"));
            }

            UserRegister command = this.BuildUserRegisterCommand(request, result.Cellphone);
            UserInfo userInfo = await this.userService.RegisterUserAsync(command, this.Request.GetTraceEntry());
            if (userInfo == null)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMQAQSU3"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSU3"));
            }

            this.BuildPrincipal(userInfo.UserId.ToGuidString(), JYMAuthScheme.Bearer);

            try
            {
                await this.MessageRoleService.SendRegisterMessageAsync(request.ClientType, userInfo, this.Request.GetTraceEntry());
            }
            catch (Exception ex)
            {
                this.Warn("快速注册写入注册队列异常,异常信息:" + ex.Message);
            }

            return this.Ok();
        }

        private UserRegister BuildUserRegisterCommand(QuickSignUpRequest request, string cellphone)
        {
            return new UserRegister
            {
                CommandId = this.Request.BuildCommandId(),
                Cellphone = cellphone,
                ClientType = request.ClientType.GetValueOrDefault(),
                ContractId = request.ContractId.GetValueOrDefault(),
                Info = request.Info,
                InviteBy = request.InviteBy ?? "JYM",
                OutletCode = request.OutletCode ?? "JYM",
                Password = "MOCKJYM", //"◎" + GuidUtility.GuidShortCode().GetFirst(6)
                UserId = GuidUtility.NewSequentialGuid()
            };
        }
    }
}