// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ThirdPartAuthController.cs
// Created          : 2016-12-14  13:58
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  14:25
// ***********************************************************************
// <copyright file="ThirdPartAuthController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Api.Helper;
using Jinyinmao.AuthManager.Api.Models.Request;
using Jinyinmao.AuthManager.Api.Models.Response;
using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Service.Coupon.Interface;
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
    ///     Class ThirdPartAuthController.
    /// </summary>
    [RoutePrefix("api/User/Auth/ThirdPartAuth")]
    public class ThirdPartAuthController : ApiControllerBase
    {
        /// <summary>
        ///     The user service
        /// </summary>
        /// <value>The user service.</value>
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        public ThirdPartAuthController(IUserService userService, IMessageRoleService messageRoleService)
        {
            this.userService = userService;
            this.MessageRoleService = messageRoleService;
        }

        private IMessageRoleService MessageRoleService { get; }

        /// <summary>
        ///     (P)第三方合作快速注册 登入 合一.
        /// </summary>
        /// <param name="request">快速注册 登入合一 请求.</param>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     AMQAQSU1:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMQAQSU2:此号码已注册，请直接登录
        ///     <br />
        ///     AMQAQSU3:注册失败
        ///     <br />
        ///     AMWCWCB4:已绑定微信账号
        /// </response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer =="
        ///     <br />
        ///     注册成功登录，登录返回加密token，token为字符串
        /// </remarks>
        [Route("ThirdPartSignUpIn")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SignInUpResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSU1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSU2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMQAQSU3")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAUAI")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ThirdPartSignUpIn(ThirdPartAuthRequest request)
        {
            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(request.Cellphone);
            UserRegister command = this.BuildUserRegisterCommand(request, request.Cellphone);

            if (checkCellphoneResult.Result)
            {
                string authToken = this.BuildAuthToken(checkCellphoneResult.UserIdentifier, JYMAuthScheme.Bearer);
                UserInfo uinfo = await this.userService.GetUserInfoAsync(checkCellphoneResult.UserIdentifier.ToGuid());
                if (uinfo == null)
                {
                    this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUAUAI")));
                    return this.BadRequest(App.Configurations.GetResourceString("err:AMUAUAI"));
                }

                return this.Ok(uinfo.ToThirdPartInUpResponse(uinfo, false, authToken));
            }
            UserInfo userInfo = await this.userService.RegisterUserAsync(command, this.Request.GetTraceEntry());
            if (userInfo == null)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMQAQSU3"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMQAQSU3"));
            }
            this.BuildPrincipal(userInfo.UserId.ToGuidString(), JYMAuthScheme.Bearer);
            try
            {
                await this.MessageRoleService.SendRegisterMessageAsync(request.ClientType, userInfo, this.Request.GetTraceEntry()); //模版尚未确认
            }
            catch (Exception)
            {
                //igore
            }

            return this.Ok(userInfo.ToThirdPartInUpResponse(userInfo, true));
        }

        private UserRegister BuildUserRegisterCommand(ThirdPartAuthRequest request, string cellphone)
        {
            request.Info.Add("EMAIL", request.Email);
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