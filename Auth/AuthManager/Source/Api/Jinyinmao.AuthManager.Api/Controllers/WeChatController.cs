// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : WeChatController.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:26
// ***********************************************************************
// <copyright file="WeChatController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
using MoeLib.Jinyinmao.Web.Auth;
using MoeLib.Jinyinmao.Web.Diagnostics;
using MoeLib.Jinyinmao.Web.Filters;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Jinyinmao.AuthManager.Api.Controllers
{
    /// <summary>
    ///     Class WeChatController.
    /// </summary>
    [RoutePrefix("api/User/Auth/WeChat")]
    public class WeChatController : ApiControllerBase
    {
        /// <summary>
        ///     The user service
        /// </summary>
        /// <value>The user service.</value>
        private readonly IUserService userService;

        /// <summary>
        ///     Gets the veri code service.
        /// </summary>
        /// <value>The veri code service.</value>
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        public WeChatController(IUserService userService, IVeriCodeService veriCodeService)
        {
            this.userService = userService;
            this.veriCodeService = veriCodeService;
        }

        /// <summary>
        ///     (U)获取登录用户的微信绑定信息
        /// </summary>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer " + access_token,access_token是登录返回的结果
        /// </remarks>
        [HttpGet]
        [Route("BindInfo")]
        [AuthorizationRequired("Bearer")]
        [UserAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(BindInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCGBI")] // AMWCGBI:未找到绑定信息，请稍后再试
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetBindInfo()
        {
            string userIdentifier = this.User.Identity.Name;
            BindInfo info = await this.userService.GetWeChatBindInfoByIdAsync(userIdentifier);
            if (info == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUWCGBI")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUWCGBI"));
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     (U)微信解绑.
        /// </summary>
        /// <response code="400">
        ///     AMWCU:未发现绑定信息
        /// </response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer " + access_token,access_token是登录返回的结果
        /// </remarks>
        [Route("Unbind")]
        [AuthorizationRequired("Bearer")]
        [UserAuthorize]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCU")] //
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Unbind()
        {
            string userIdentifier = this.User.Identity.Name;
            BindInfo info = await this.userService.GetWeChatBindInfoByIdAsync(userIdentifier);
            if (info.Flag == 0)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMWCU")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCU"));
            }

            await this.userService.DeleteWeChatRelationAsync(userIdentifier);
            return this.Ok();
        }

        /// <summary>
        ///     微信绑定.
        /// </summary>
        /// <param name="request">微信绑定请求.</param>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     AMWCWCB1:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMWCWCB2:此号码未注册，请注册
        ///     <br />
        ///     AMWCWCB3:无法加载用户信息，请稍后重试
        ///     <br />
        ///     AMWCWCB4:已绑定微信账号
        /// </response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer =="
        ///     <br />
        ///     微信绑定，返回加密token，token为字符串
        /// </remarks>
        [Route("Bind")]
        [AuthorizationRequired("Bearer")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCB1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCB2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCB3")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCB4")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> WeChatBind(WeChatBindRequest request)
        {
            UseVeriCodeResult veriCodeResult = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.WeChatBind.Code, this.Request.GetTraceEntry());
            if (!veriCodeResult.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMWCWCB1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCB1"));
            }

            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(veriCodeResult.Cellphone);
            if (!checkCellphoneResult.Result || checkCellphoneResult.UserIdentifier.IsNullOrEmpty())
            {
                this.Warn(App.Configurations.GetResourceString("err:AMWCWCB2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCB2"));
            }

            BindInfo bindInfo = await this.userService.GetWeChatBindInfoByIdAsync(checkCellphoneResult.UserIdentifier);
            if (bindInfo.Flag == 1)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMWCWCB4")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCB4"));
            }

            await this.userService.WeChatBindAsync(this.BuildCommand(request, checkCellphoneResult.UserIdentifier));
            this.BuildPrincipal(checkCellphoneResult.UserIdentifier, JYMAuthScheme.Bearer);
            return this.Ok();
        }

        /// <summary>
        ///     (P)微信登录.
        /// </summary>
        /// <response code="200">
        ///     登录返回加密token，token为字符串
        /// </response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     AMWCWCSI1:获取openId失败
        ///     <br />
        ///     AMWCWCSI2:无法获取用户信息，请稍后重试
        /// </response>
        /// <response code="500"></response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer =="
        ///     <br />
        ///     登录返回加密token，token为字符串
        /// </remarks>
        [Route("SignIn")]
        [AuthorizationRequired("Bearer")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(WeChatSignInResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCSI1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCSI2")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> WeChatSignIn(WeChatSignInRequest request)
        {
            string openId = await this.userService.GetOpenIdAsync(request.Code);
            //string openId = "oY0rAs7ihkVhoeRXt8x9VxMiI_5Y";
            if (openId.IsNullOrEmpty())
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMWCWCSI1")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCSI1"));
            }

            string userIdentifier = await this.userService.GetUserIdentifierByOpenIdAsync(openId);
            if (userIdentifier.IsNullOrEmpty())
            {
                return this.Ok(new WeChatSignInResponse { OpenId = openId });
            }

            UserInfo info = await this.userService.GetUserInfoAsync(userIdentifier.ToGuid());
            if (info == null)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMWCWCSI2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCSI2"));
            }

            this.BuildPrincipal(info.UserId.ToGuidString(), JYMAuthScheme.Bearer);
            return this.Ok(new WeChatSignInResponse { OpenId = "" });
        }

        /// <summary>
        ///     (P)微信注册.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        /// <response code="200">
        ///     注册成功直接登录，登录返回加密token，token为字符串
        /// </response>
        /// <response code="400">
        ///     AMWCWCSU1:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMWCWCSU2:此号码已注册，请直接登录
        ///     <br />
        ///     AMWCWCSU3:注册失败
        /// </response>
        /// <response code="500"></response>
        /// ///
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer =="
        ///     <br />
        ///     注册成功直接登录，登录返回加密token，token为字符串
        /// </remarks>
        [HttpPost]
        [Route("SignUp")]
        [AuthorizationRequired("Bearer")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCSU1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCSU2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMWCWCSU3")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> WeChatSignUp(WeChatSignUpRequest request)
        {
            UseVeriCodeResult veriCodeResult = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.WeChatSignUp.Code, this.Request.GetTraceEntry());
            if (!veriCodeResult.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMWCWCSU1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCSU1"));
            }

            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(veriCodeResult.Cellphone);
            if (checkCellphoneResult.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMWCWCSU2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCSU2"));
            }

            UserInfo info = await this.userService.WeChatSignUpAsync(this.BuildCommand(request, veriCodeResult.Cellphone));
            if (info == null)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMWCWCSU3"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMWCWCSU3"));
            }

            this.BuildPrincipal(info.UserId.ToGuidString(), JYMAuthScheme.Bearer);
            return this.Ok();
        }

        /// <summary>
        ///     Builds the command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>WeChatSignIn.</returns>
        private WeChatSignIn BuildCommand(WeChatSignInRequest request)
        {
            return new WeChatSignIn
            {
                CommandId = this.Request.BuildCommandId(),
                EntityId = this.Request.BuildCommandId(),
                Code = request.Code
            };
        }

        /// <summary>
        ///     Builds the command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>WeChatRegister.</returns>
        private WeChatRegister BuildCommand(WeChatSignUpRequest request, string cellphone)
        {
            return new WeChatRegister
            {
                CommandId = this.Request.BuildCommandId(),
                Cellphone = cellphone,
                ClientType = request.ClientType.GetValueOrDefault(),
                ContractId = request.ContractId.GetValueOrDefault(),
                Info = request.Info,
                InviteBy = request.InviteBy ?? "JYM",
                OutletCode = request.OutletCode ?? "JYM",
                OpenId = request.OpenId,
                Token = request.Token
            };
        }

        /// <summary>
        ///     Builds the command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>WeChatBind.</returns>
        private WeChatBind BuildCommand(WeChatBindRequest request, string userIdentifier)
        {
            return new WeChatBind
            {
                CommandId = this.Request.BuildCommandId(),
                EntityId = userIdentifier.ToGuid(),
                OpenId = request.OpenId,
                UserIdentifier = userIdentifier
            };
        }
    }
}