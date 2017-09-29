// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserAuthController.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-01-10  20:37
// ***********************************************************************
// <copyright file="UserAuthController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.AuthManager.Api.Controllers;
using Jinyinmao.AuthManager.Api.Helper;
using Jinyinmao.AuthManager.Api.Models.Request;
using Jinyinmao.AuthManager.Api.Models.Request.Basic;
using Jinyinmao.AuthManager.Api.Models.Response;
using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Libraries.Parameter;
using Jinyinmao.AuthManager.Service.Coupon.Interface;
using Jinyinmao.AuthManager.Service.Misc.Interface;
using Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult;
using Jinyinmao.AuthManager.Service.User.Interface;
using Jinyinmao.AuthManager.Service.User.Interface.Dtos;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web.Auth;
using MoeLib.Jinyinmao.Web.Diagnostics;
using MoeLib.Jinyinmao.Web.Filters;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.AuthManager.Api.Models
{
    /// <summary>
    ///     Class UserAuthController.
    /// </summary>
    [RoutePrefix("api/User/Auth")]
    public class UserAuthController : ApiControllerBase
    {
        private readonly IMessageRoleService messageRoleService;

        private readonly IUserService userService;

        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        public UserAuthController(IUserService userService, IVeriCodeService veriCodeService, IMessageRoleService messageRoleService)
        {
            this.userService = userService;
            this.veriCodeService = veriCodeService;
            this.messageRoleService = messageRoleService;
        }

        /// <summary>
        ///     1、老手机号码发送短信验证验证
        ///     2、姓名、身份证号码验证
        ///     3、新手机号码发送短信验证
        ///     (A)修改登录手机号.
        /// </summary>
        /// <param name="request">The request.</param>
        [Route("ChangeLoginCellphone")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "修改登录手机号失败")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ChangeLoginCellphone(ChangeLoginCellphoneRequest request)
        {
            UserInfo info = await this.userService.ChangeLoginCellphoneAsync(this.BuildCommand(request));
            return info == null ? (IHttpActionResult)this.BadRequest("修改登录手机号失败") : this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     (P)手机号是否已注册
        /// </summary>
        /// <param name="cellphone">
        ///     手机号
        /// </param>
        /// <response code="400">
        ///     AMUACC1:手机号格式不正确
        /// </response>
        /// <remarks>
        ///     如果手机号已经注册过，则不能再用于注册
        /// </remarks>
        [HttpGet]
        [Route("CheckCellphone")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CheckCellphoneResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUACC1")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> CheckCellphone(string cellphone)
        {
            cellphone = cellphone ?? "";
            Match match = CellphoneRegex.Match(cellphone);
            if (!match.Success || match.Index != 0 || match.Length != cellphone.Length)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUACC1")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUACC1"));
            }
            CheckCellphoneResult result = await this.userService.CheckCellphoneAsync(cellphone);
            return this.Ok(result.ToResponse());
        }

        /// <summary>
        ///     (P)检查手机号码和登录密码是否正确.
        /// </summary>
        /// <response code="400">
        ///     AMUACCAP1:手机号格式不正确
        ///     <br />
        ///     AMUACCAP2:登录密码格式不正确
        /// </response>
        /// <remarks>
        ///     该接口是检查登录密码
        /// </remarks>
        [HttpPost]
        [Route("CheckCellphoneAndPassword")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SignInResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUACCAP1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUACCAP2")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> CheckCellphoneAndPasswordFromBody(CheckCellphoneAndPasswordRequest request)
        {
            SignInResult result = await this.userService.CheckPasswordViaCellphoneAsync(request.Cellphone, request.Password);
            return this.Ok(result.ToResponse());
        }

        /// <summary>
        ///     (A)检查用户唯一标识和密码是否正确.
        /// </summary>
        /// <response code="400">
        ///     AMUACP1:用户唯一标识格式不正确
        /// </response>
        /// <remarks>
        ///     仅限于校验支付密码是否和交易密码相同
        /// </remarks>
        [HttpPost]
        [Route("CheckPassword")]
        [ApplicationAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CheckPasswordResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUACP1")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> CheckPassword(CheckPasswordRequest request)
        {
            return this.Ok(new CheckPasswordResponse { Result = await this.userService.CheckPasswordAsync(request.UserIdentifier, request.Password) });
        }

        /// <summary>
        ///     (A)LockUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        [Route("LockUser/{userIdentifier:length(32)}")]
        [ApplicationAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "用户唯一标识错误")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "无法获取用户信息")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> LockUser(string userIdentifier)
        {
            if (userIdentifier.AsGuid("N", Guid.Empty) == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            UserInfo info = await this.userService.LockAsync(userIdentifier);
            if (info == null)
            {
                return this.BadRequest("无法获取用户信息");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     (P)重置登录密码
        /// </summary>
        /// <remarks>
        ///     重置密码前，必须要认证手机号，并且获得认证手机号的token
        /// </remarks>
        /// <param name="request">
        ///     重置登录密码请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     AMUARLP1:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AUUARLP2:手机号码不存在，密码修改失败
        ///     <br />
        ///     AMUARLP3:无法获取用户信息，请稍后重试
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("ResetLoginPassword")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUARLP1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AUUARLP2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUARLP3")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ResetLoginPassword(ResetPasswordRequest request)
        {
            UseVeriCodeResult veriCodeResult = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.ResetLoginPassword.Code, this.Request.GetTraceEntry());
            if (!veriCodeResult.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUARLP1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUARLP1"));
            }

            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(veriCodeResult.Cellphone);
            if (!checkCellphoneResult.Result || checkCellphoneResult.UserIdentifier.IsNullOrEmpty())
            {
                this.Warn(App.Configurations.GetResourceString("err:AUUARLP2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AUUARLP2"));
            }

            UserInfo userInfo = await this.userService.ResetLoginPasswordAsync(this.BuildResetLoginPasswordCommand(request, checkCellphoneResult.UserIdentifier));
            if (userInfo == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUARLP3")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUARLP3"));
            }

            return this.Ok(userInfo.ToResponse());
        }

        /// <summary>
        ///     (U)设置登录密码.
        /// </summary>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     AMUAUI1:无法获取用户信息，请稍后重试
        /// </response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: Bearer access_token, access_token是登录返回的结果
        /// </remarks>
        [Route("SetPassword")]
        [AuthorizationRequired("Bearer")]
        [UserAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAUI1")] //
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SetPassword(SetPasswodRequest request)
        {
            string userIdentifier = this.User.Identity.Name;

            UserInfo info = await this.userService.GetUserInfoAsync(userIdentifier.ToGuid());
            if (info == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUAUI1")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAUI1"));
            }

            info = await this.userService.SetLoginPasswordAsync(this.BuilSetLoginPasswordCommand(request, info));
            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     (P)登录
        /// </summary>
        /// <remarks>
        ///     通过账户名和密码登录，现在账户名即为用户的手机号
        ///     <br />
        ///     Header添加X-JYM-Authorization: "Bearer =="
        /// </remarks>
        /// <response code="200">
        ///     登录返回加密token，token为字符串，可以通过返回类型的auth字段获取
        /// </response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="500"></response>
        [HttpPost]
        [Route("SignIn")]
        [AuthorizationRequired("Bearer")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SignInResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SignIn(SignInRequest request)
        {
            SignInResult signInResult = await this.userService.CheckPasswordViaCellphoneAsync(request.LoginName, request.Password);
            if (!signInResult.Success)
            {
                return this.Ok(signInResult.ToResponse());
            }

            string authToken = this.BuildAuthToken(signInResult.UserId.ToGuidString(), JYMAuthScheme.Bearer);
            return this.Ok(signInResult.ToResponse(authToken));
        }

        /// <summary>
        ///     (P)金银猫客户端注销接口
        /// </summary>
        /// <remarks>
        ///     客户端可以通过直接清除Cookie或者本地的token值实现注销
        /// </remarks>
        /// <response code="200">注销成功</response>
        [HttpGet]
        [Route("SignOut")]
        public HttpResponseMessage SignOut()
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.AddCookies(new List<CookieHeaderValue>
            {
                new CookieHeaderValue("X-JYM-AUTH-PC", "") { HttpOnly = true, Domain = ".jinyinmao.com.cn", Expires = DateTimeOffset.Now.AddYears(-3), Path = "/" },
                new CookieHeaderValue("X-JYM-AUTH", "") { HttpOnly = false, Domain = ".jinyinmao.com.cn", Expires = DateTimeOffset.Now.AddYears(-3), Path = "/" },
                new CookieHeaderValue("JYM-AUTH", "") { HttpOnly = false, Domain = ".jinyinmao.com.cn", Expires = DateTimeOffset.Now.AddYears(-3), Path = "/" },
                new CookieHeaderValue("X-JYM-AT", "") { HttpOnly = false, Domain = ".jinyinmao.com.cn", Expires = DateTimeOffset.Now.AddYears(-3), Path = "/" }
            });
            return response;
        }

        /// <summary>
        ///     (P)金银猫客户端注册接口
        /// </summary>
        /// <param name="request">注册请求</param>
        /// <response code="400">
        ///     AMUASU1:该验证码已经失效，请重新获取验证码
        ///     <br />
        ///     AMUASU2:此号码已注册，请直接登录
        ///     <br />
        ///     AMUASU3:注册失败
        /// </response>
        /// <remarks>
        ///     注册成功登录，登录返回加密token，token为字符串
        ///     <br />
        ///     在金银猫的客户端注册，包括PC网页、M版网页、iPhone、Android
        ///     <br />
        ///     前置条件：已经通过验证码验证手机号码的真实性
        ///     <br />
        ///     Header添加X-JYM-Authorization: Bearer ==
        /// </remarks>
        [Route("SignUp")]
        [AuthorizationRequired("Bearer")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "请求格式不合法")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUASU1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUASU2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUASU3")]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SignUp(SignUpRequest request)
        {
            UseVeriCodeResult veriCodeResult = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.SignUp.Code, this.Request.GetTraceEntry());
            if (!veriCodeResult.Result)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUASU1")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUASU1"));
            }

            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(veriCodeResult.Cellphone);
            if (checkCellphoneResult.Result)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUASU2")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUASU2"));
            }

            UserInfo userInfo = await this.userService.RegisterUserAsync(this.BuildUserRegisterCommand(request, veriCodeResult.Cellphone), this.Request.GetTraceEntry());
            if (userInfo == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUASU3")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUASU3"));
            }

            this.BuildPrincipal(userInfo.UserId.ToGuidString(), JYMAuthScheme.Bearer);

            try
            {
                await this.messageRoleService.SendRegisterMessageAsync(request.ClientType, userInfo, this.Request.GetTraceEntry());
            }
            // ReSharper disable once UnusedVariable
            catch (Exception ex)
            {
                this.Warn("写入注册队列异常,异常信息:" + ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        ///     (A)UnlockUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        [HttpGet]
        [Route("UnlockUser/{userIdentifier:length(32)}")]
        [ApplicationAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "用户唯一标识错误")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "无法获取用户信息")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> UnlockUser(string userIdentifier)
        {
            if (userIdentifier.AsGuid("N", Guid.Empty) == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            UserInfo info = await this.userService.UnLockAsync(userIdentifier);
            if (info == null)
            {
                return this.BadRequest("无法获取用户信息");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     (A)UnregisteredCellphone
        /// </summary>
        /// <param name="cellphone">手机号</param>
        [Route("UnregisteredCellphone/{cellphone:length(11)}")]
        [ApplicationAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "手机号格式不正确")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> UnregisteredCellphone(string cellphone)
        {
            cellphone = cellphone ?? "";
            Match match = CellphoneRegex.Match(cellphone);
            if (!match.Success || match.Index != 0 || match.Length != cellphone.Length)
            {
                return this.BadRequest("手机号格式不正确");
            }

            await this.userService.UnregisterAsync(cellphone);

            return this.Ok();
        }

        /// <summary>
        ///     (U)获取用户Auth信息.
        /// </summary>
        /// <response code="400">
        ///     AMUAUAI:无法获取用户信息，请稍后重试
        /// </response>
        /// <remarks>
        ///     Header添加X-JYM-Authorization: "Bearer " + access_token,access_token是登录返回的结果
        /// </remarks>
        [HttpGet]
        [Route("UserAuthInfo")]
        [UserAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAUAI")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> UserAuthInfo()
        {
            UserInfo info = await this.userService.GetUserInfoAsync(this.User.Identity.Name.AsGuid());
            if (info == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUAUAI")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAUAI"));
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     (A)通过手机号码获取用户唯一标识.
        /// </summary>
        /// <param name="cellphone">手机号码.</param>
        /// <response code="400">
        ///     AMUAUAIBC1:手机号格式不正确
        ///     <br />
        ///     AMUAUAIBC2:无法获取用户信息，请稍后重试
        /// </response>
        [HttpGet]
        [Route("UserAuthInfo/{cellphone}")]
        [ApplicationAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAUAIBC1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAUAIBC2")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> UserAuthInfoByCellphone(string cellphone)
        {
            Match match = CellphoneRegex.Match(cellphone);
            if (cellphone.IsNullOrEmpty() || !match.Success || match.Index != 0 || match.Length != cellphone.Length)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUAUAIBC1")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAUAIBC1"));
            }

            UserInfo info = await this.userService.GetUserByCellphoneAsync(cellphone);
            if (info == null)
            {
                this.Warn(new ApplicationException(App.Configurations.GetResourceString("err:AMUAUAIBC2")));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAUAIBC2"));
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     Users the register.
        /// </summary>
        /// <param name="request">The request.</param>
        [Route("UserRegister")]
        [ApplicationAuthorize]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> UserRegister(UserRegisterRequest request)
        {
            UserRegister command = new UserRegister
            {
                CommandId = this.Request.BuildCommandId(),
                Cellphone = request.Cellphone,
                ClientType = request.ClientType.GetValueOrDefault(),
                ContractId = request.ContractId.GetValueOrDefault(),
                Info = new Dictionary<string, object>(),
                InviteBy = request.InviteBy ?? "JYM",
                OutletCode = request.OutletCode ?? "JYM",
                Password = request.Password
            };
            UserInfo userInfo = await this.userService.RegisterUserAsync(command, this.Request.GetTraceEntry());
            if (userInfo == null)
            {
                return this.BadRequest("注册失败");
            }

            try
            {
                await this.messageRoleService.SendRegisterMessageAsync(request.ClientType, userInfo, this.Request.GetTraceEntry());
            }
            catch (Exception ex)
            {
                this.Warn("写入注册队列异常,异常信息:" + ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        ///     姓名、身份证号码验证
        /// </summary>
        /// <remarks>
        ///     1、原手机号码验证
        ///     2、姓名、身份证号码验证
        ///     3、新手机号码发送短信验证
        ///     (A)验证身份信息,将会把上一步的短信验证码Token和本次身份信息的Token一起传到客户端.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <response code="400">
        ///     err:AMUAVC1:原手机号码验证信息已经失效，请重新获取验证码
        ///     <br />
        ///     err:AMUAVC2:当前用户不存在
        ///     <br />
        ///     err:AMUAVC3:身份证和姓名信息不一致,请重新输入
        ///     <br />
        ///     err:AMUAVC4:非身份证认证的用户请联系客服
        /// </response>
        [Route("ValidateCredential")]
        [UserAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AuthStepInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVC1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVC2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVC3")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVC4")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ValidateCredential(ValidateCredentialNoRequest request)
        {
            if (request.CredentialNo.Length < 15)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVC4"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVC4"));
            }

            SetSecondAuthStep command = this.BuildSecondAuthStepCommand(request);
            bool isValidateOldCellphone = await this.userService.IsValidateOldCellphone(command);
            if (!isValidateOldCellphone)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVC1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVC1"));
            }

            UserBizInfo userBizInfo = await this.userService.GetUserInfoFromTirisferAsync(this.User.Identity.Name);
            if (userBizInfo == null)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVC2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVC2"));
            }

            if (userBizInfo.CredentialNo != request.CredentialNo || userBizInfo.RealName != request.Name)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVC3"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVC3"));
            }

            AuthStepInfo info = await this.userService.SetSecondAuthStepAsync(command);

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     新手机号码发送短信验证
        /// </summary>
        /// <remarks>
        ///     1、原手机号码验证
        ///     2、姓名、身份证号码验证
        ///     3、新手机号码发送短信验证
        ///     (A)验证身份信息,将会把上一步的短信验证码Token和本次身份信息的Token一起传到客户端.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <response code="400">
        ///     err:AMUAVNC1:该手机号已经注册，请重新验证
        ///     <br />
        ///     err:AMUAVNC2:身份证验证信息已经失效，请重新验证
        ///     <br />
        ///     err:AMUAVNC3:新手机号验证超时,请重新验证
        /// </response>
        [Route("ValidateNewCellphone")]
        [UserAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AuthStepInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVNC1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVNC2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVNC3")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ValidateNewCellphone(ValidateNewCellphoneRequest request)
        {
            CheckCellphoneResult checkCellphoneResult = await this.userService.CheckCellphoneAsync(request.NewCellphone);
            if (checkCellphoneResult.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVNC1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVNC1"));
            }

            SetThirdAuthStep command = this.BuildThirdAuthStepCommand(request);
            bool isValidateValidateCredential = await this.userService.IsValidateCredential(command);
            if (!isValidateValidateCredential)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVNC2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVNC2"));
            }

            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.SMSToken, VeriCodeType.VarifyNewCellphone.Code, this.Request.GetTraceEntry());
            if (!result.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVNC3"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVNC3"));
            }

            AuthStepInfo info = await this.userService.SetThirdAuthStepAsync(command);
            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     原手机号码验证
        /// </summary>
        /// <remarks>
        ///     1、原手机号码验证
        ///     2、姓名、身份证号码验证
        ///     3、新手机号码发送短信验证
        ///     (A)验证身份信息,将会把上一步的短信验证码Token和本次身份信息的Token一起传到客户端.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <response code="400">
        ///     err:AMUAVOC1:原手机号验证失败,请重新验证
        ///     <br />
        ///     err:AMUAVOC2:您输入的原手机号不是您的注册手机号,请重新验证
        ///     <br />
        ///     err:AMUAVOC3:当前用户不存在
        ///     <br />
        ///     err:AMUAVOC4:为保障您的账户安全，请在修改手机号前完成个人身份信息认证！
        /// </response>
        [Route("ValidateOldCellphone")]
        [UserAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVOC1")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVOC2")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVOC3")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "err:AMUAVOC4")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ValidateOldCellphone(ValidateOldCellphoneRequest request)
        {
            UserBizInfo userBizInfo = await this.userService.GetUserInfoFromTirisferAsync(this.User.Identity.Name);
            if (userBizInfo == null)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVOC3"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVOC3"));
            }

            if (!userBizInfo.Verified)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVOC4"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVOC4"));
            }

            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.OriginCellphoneToken, VeriCodeType.VarifyOldCellphone.Code, this.Request.GetTraceEntry());
            if (!result.Result)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVOC1"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVOC1"));
            }

            SetFirstAuthStep command = this.BuildFirstAuthStepCommand(request);

            UserInfo userInfo = await this.userService.GetUserInfoAsync(command.UserId);
            if (userInfo.Cellphone != request.OriginCellphone)
            {
                this.Warn(App.Configurations.GetResourceString("err:AMUAVOC2"));
                return this.BadRequest(App.Configurations.GetResourceString("err:AMUAVOC2"));
            }

            AuthStepInfo info = await this.userService.SetFirstAuthStepAsync(command);
            object response = new { info.Token };
            return this.Ok(response);
        }

        #region 私有方法

        private ChangeLoginCellphone BuildCommand(ChangeLoginCellphoneRequest request)
        {
            return new ChangeLoginCellphone
            {
                CommandId = this.Request.BuildCommandId(),
                LoginCellphone = request.LoginCellphone,
                NewCellphone = request.NewCellphone
            };
        }

        private SetFirstAuthStep BuildFirstAuthStepCommand(ValidateOldCellphoneRequest request)
        {
            return new SetFirstAuthStep
            {
                CommandId = this.Request.BuildCommandId(),
                SMSToken = request.OriginCellphoneToken,
                Token = GuidUtility.NewSequentialGuid().ToGuidString(),
                UserId = this.User.Identity.Name.ToGuid(),
                ValidateMessage = $"用户{this.User.Identity.Name}修改手机号,正在进行原手机号{request.OriginCellphone}和验证码{request.VerificationCode}验证"
            };
        }

        private ResetLoginPassword BuildResetLoginPasswordCommand(ResetPasswordRequest request, string userIdentifier)
        {
            return new ResetLoginPassword
            {
                CommandId = this.Request.BuildCommandId(),
                EntityId = userIdentifier.ToGuid(),
                Password = request.Password,
                Salt = userIdentifier,
                UserId = userIdentifier.ToGuid()
            };
        }

        private SetSecondAuthStep BuildSecondAuthStepCommand(ValidateCredentialNoRequest request)
        {
            return new SetSecondAuthStep
            {
                CommandId = this.Request.BuildCommandId(),
                CredentialNo = request.CredentialNo,
                Name = request.Name,
                Token = Guid.NewGuid().ToGuidString(),
                PreviousToken = request.StepFirstToken,
                UserId = this.User.Identity.Name.ToGuid(),
                ValidateMessage = $"用户{this.User.Identity.Name}修改手机号,正在进行姓名{request.Name}和身份证{request.CredentialNo}验证"
            };
        }

        private SetThirdAuthStep BuildThirdAuthStepCommand(ValidateNewCellphoneRequest request)
        {
            return new SetThirdAuthStep
            {
                CommandId = this.Request.BuildCommandId(),
                Token = Guid.NewGuid().ToGuidString(),
                SMSToken = request.SMSToken,
                PreviousToken = request.StepSecondToken,
                NewCellphone = request.NewCellphone,
                UserId = this.User.Identity.Name.ToGuid(),
                ValidateMessage = $"用户修改手机号,正在进行新手机号{request.NewCellphone}和验证码{request.VerificationCode}验证"
            };
        }

        private UserRegister BuildUserRegisterCommand(SignUpRequest request, string cellphone)
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
                Password = request.Password,
                UserId = GuidUtility.NewSequentialGuid()
            };
        }

        private SetLoginPassword BuilSetLoginPasswordCommand(SetPasswodRequest request, UserInfo info)
        {
            return new SetLoginPassword
            {
                CommandId = this.Request.BuildCommandId(),
                EntityId = info.UserId,
                Password = request.Password,
                Salt = info.UserId.ToGuidString(),
                UserId = info.UserId
            };
        }

        #endregion 私有方法
    }
}