// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : OldQuickAuthController.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:24
// ***********************************************************************
// <copyright file="OldQuickAuthController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Api.Helper;
using Jinyinmao.AuthManager.Api.Models.Request;
using Jinyinmao.AuthManager.Api.Models.Response;
using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Libraries.Parameter;
using Jinyinmao.AuthManager.Service.Misc.Interface;
using Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult;
using Jinyinmao.AuthManager.Service.User.Interface;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web.Diagnostics;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Jinyinmao.AuthManager.Api.Controllers
{
    /// <summary>
    ///     Class OldQuickAuthController.
    /// </summary>
    public class OldQuickAuthController : ApiControllerBase
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
        public OldQuickAuthController(IUserService userService, IVeriCodeService veriCodeService)
        {
            this.userService = userService;
            this.veriCodeService = veriCodeService;
        }

        /// <summary>
        ///     (P)快速登录
        /// </summary>
        /// <param name="request">快速登录请求.</param>
        /// <response code="400">
        ///     AMOQAQSI1:验证码错误，请重新输入
        ///     <br />
        ///     AMOQAQSI2:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMOQAQSI3:未能加载用户信息，请稍后再试
        /// </response>
        /// <remarks>
        ///     登录成功返回加密token，token为字符串
        /// </remarks>
        [Route("SignIn")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMOQAQSI1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMOQAQSI2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMOQAQSI3")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> QuickSignIn(OldQuickSignInRequest request)
        {
            VerifyVeriCodeResult veriCodeResult = await this.veriCodeService.VerifyAsync(request.Cellphone, request.Code, VeriCodeType.QuickSignIn.Code);
            if (!veriCodeResult.Success)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMOQAQSI1")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMOQAQSI1"));
            }
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(veriCodeResult.Token, VeriCodeType.QuickSignIn.Code, this.Request.GetTraceEntry());
            if (!result.Result)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMOQAQSI2")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMOQAQSI2"));
            }

            UserInfo userInfo = await this.userService.GetUserByCellphoneAsync(result.Cellphone);
            if (userInfo == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMOQAQSI3")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMOQAQSI3"));
            }

            if (userInfo.PasswordErrorCount < 10)
            {
                string auth = this.SetCookie(userInfo.UserId, userInfo.Cellphone);
                return this.Ok(userInfo.ToSignInResponse(auth));
            }
            return this.Ok(userInfo.ToSignInResponse());
        }

        /// <summary>
        ///     (P)快速注册.
        /// </summary>
        /// <param name="request">快速注册请求.</param>
        /// <response code="400">
        ///     AMOQAQSU1:验证码错误，请重新输入
        ///     <br />
        ///     AMOQAQSU2:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMOQAQSU3:此号码已注册，请直接登录
        ///     <br />
        ///     AMOQAQSU4:注册失败
        /// </response>
        /// <remarks>
        ///     注册成功登录，登录返回加密token，token为字符串
        /// </remarks>
        [HttpPost]
        [Route("SignUp")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMOQAQSU1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMOQAQSU2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMOQAQSU3")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMOQAQSU4")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> QuickSignUp(OldQuickSignUpRequest request)
        {
            VerifyVeriCodeResult veriCodeResult = await this.veriCodeService.VerifyAsync(request.Cellphone, request.Code, VeriCodeType.QuickSignIn.Code);
            if (!veriCodeResult.Success)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMOQAQSU1")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMOQAQSU1"));
            }

            UseVeriCodeResult result = await this.veriCodeService.UseAsync(veriCodeResult.Token, VeriCodeType.SignUp.Code, this.Request.GetTraceEntry());
            if (!result.Result)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMQAQSU2")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSU2"));
            }
            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(result.Cellphone);
            //bool cellphoneExist = await this.userService.CheckCellphoneExistAsync(result.Cellphone);
            if (checkCellphoneResult.Result)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMQAQSU3")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSU3"));
            }

            UserRegister command = this.BuildUserRegisterCommand(request, result.Cellphone);
            UserInfo userInfo = await this.userService.RegisterUserAsync(command, this.Request.GetTraceEntry());
            if (userInfo == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMQAQSU4")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSU4"));
            }
            string auth = this.SetCookie(userInfo.UserId, userInfo.Cellphone);
            return this.Ok(userInfo.ToSignUpResponse(auth));
        }

        private UserRegister BuildUserRegisterCommand(OldQuickSignUpRequest request, string cellphone)
        {
            return new UserRegister
            {
                CommandId = this.Request.BuildCommandId(),
                Cellphone = cellphone,
                ClientType = request.ClientType.GetValueOrDefault(),
                ContractId = request.ContractId.GetValueOrDefault(),
                Info = new Dictionary<string, object>(),
                InviteBy = request.InviteBy ?? "JYM",
                OutletCode = request.OutletCode ?? "JYM",
                Password = "MOCKJYM" + GuidUtility.GuidShortCode().GetFirst(10), //"◎" + GuidUtility.GuidShortCode().GetFirst(6)
                UserId = GuidUtility.NewSequentialGuid()
            };
        }

        /// <summary>
        ///     Sets the cookie.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cellphone">The cellphone.</param>
        private string SetCookie(Guid userId, string cellphone)
        {
            bool isMobileDevice = HttpUtils.IsFromMobileDevice(this.Request);
            DateTime expiry = isMobileDevice ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(4);
            string userData = $"{userId},{cellphone},{expiry.ToBinary()}";
            FormsAuthentication.SetAuthCookie(userData, true);
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(userData, true);
            HttpContext.Current.Response.Headers.Add("X-JYM-AUTH", cookie.Value);
            return cookie.Value;
        }
    }
}