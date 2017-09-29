// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : DevController.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-02-16  14:02
// ***********************************************************************
// <copyright file="DevController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Jinyinmao.AuthManager.Api.Filters;
using Jinyinmao.AuthManager.Api.Helper;
using Jinyinmao.AuthManager.Api.Models.Request.Basic;
using Jinyinmao.AuthManager.Api.Models.Response;
using Jinyinmao.AuthManager.Domain.Interface;
using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Service.User.Interface;
using Moe.Lib;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web.Filters;
using Orleans;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.AuthManager.Api.Controllers
{
    /// <summary>
    ///     Class DevController.
    /// </summary>
    [RoutePrefix("")]
    public class DevController : ApiControllerBase
    {
        /// <summary>
        /// </summary>
        /// <param name="userService"></param>
        public DevController(IUserService userService)
        {
            this.UserService = userService;
        }

        /// <summary>
        ///     The user service
        /// </summary>
        /// <value>The user service.</value>
        private IUserService UserService { get; }

        /// <summary>
        ///     人工注销账户[谨慎使用]
        /// </summary>
        [HttpGet]
        [Route("AdminCancelAccount")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> AdminCancelAccountAsync(string userIdentifier)
        {
            string cellphone = await this.UserService.AdminCancelAccountAsync(this.BuildAdminCancelAccountCommand(userIdentifier));
            return this.Ok(cellphone);
        }

        /// <summary>
        ///     人工修改手机号码[谨慎使用]
        /// </summary>
        [HttpPost]
        [Route("AdminModifyCellphone")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> AdminModifyCellphoneAsync(AdminModifyCellphoneRequest request)
        {
            CheckCellphoneResult checkCellphoneResult = await this.UserService.CheckCellphoneAsync(request.NewCellphone);
            if (checkCellphoneResult.Result)
            {
                return this.BadRequest("该手机号已经注册，请重新验证");
            }

            await this.UserService.AdminModifyCellphoneAsync(this.BuildAdminModifyCellphoneCommand(request));
            return this.Ok();
        }

        /// <summary>
        ///     将用户Relation信息从内存加载到数据库
        /// </summary>
        /// <remarks>
        ///     只能用于加载可以正常登陆的用户
        /// </remarks>
        /// <param name="cellphone">手机号码</param>
        [HttpGet]
        [Route("SyncUserRelationToDB/{cellphone}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpUserRelationToDB(string cellphone)
        {
            IUserRelationGrain grain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(cellphone);
            string userIdentifier = await grain.DumpUserRelationToDBAsync();
            return this.Ok(userIdentifier);
        }

        /// <summary>
        ///     将用户Relation信息从数据库加载到内存
        /// </summary>
        /// <remarks>
        ///     只能用于加载可以正常登陆的用户
        /// </remarks>
        /// <param name="cellphone">手机号码</param>
        [HttpGet]
        [Route("DumpUserRelationToMemory/{cellphone}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpUserRelationToMemory(string cellphone)
        {
            IUserRelationGrain grain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(cellphone);
            await grain.DumpUserRelationToMemoryAsync();
            return this.Ok();
        }

        /// <summary>
        ///     将User信息从内存更新到数据库.
        /// </summary>
        /// <remarks>
        ///     只能用于加载可以正常登陆的用户
        /// </remarks>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [IpAuthorize(OnlyLocalHost = true)]
        [HttpPost]
        [Route("DumpUserToDB/{userIdentifier:length(32)}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadGateway, "用户唯一标示错误")]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> DumpUserToDB(string userIdentifier)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            await user.DumpUserToDBAsync();
            return this.Ok();
        }

        /// <summary>
        ///     将User信息从数据库加载到内存.
        /// </summary>
        /// <remarks>
        ///     只能用于加载可以正常登陆的用户
        /// </remarks>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [IpAuthorize(OnlyLocalHost = true)]
        [HttpPost]
        [Route("DumpUserToMemory/{userIdentifier:length(32)}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadGateway, "用户唯一标示错误")]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> DumpUserToMemory(string userIdentifier)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            await user.DumpUserToMemoryAsync();
            return this.Ok();
        }

        /// <summary>
        ///     Gets this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return this.Ok(
                new
                {
                    this.Request.RequestUri,
                    this.Request.Headers,
                    QueryParameters = this.Request.GetQueryNameValuePairs(),
                    RequestProperties = this.Request.Properties.Keys,
                    this.RequestContext.ClientCertificate,
                    this.RequestContext.IsLocal,
                    this.RequestContext.VirtualPathRoot,
                    HttpContext.Current.Request.Browser.Browser,
                    HttpContext.Current.Request.IsSecureConnection,
                    HttpContext.Current.Request.Browser.IsMobileDevice,
                    IsFromMobileDevice = HttpUtils.IsFromMobileDevice(this.Request),
                    UserHostAddress = HttpUtils.GetUserHostAddress(this.Request),
                    UserAgent = HttpUtils.GetUserAgent(this.Request),
                    Cookie = this.Request.Headers.GetCookies(),
                    this.Request.Content,
                    ConfigurationProperties = this.Configuration.Properties,
                    ServerIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString()
                });
        }

        /// <summary>
        ///     获取用户信息.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [ApplicationAuthorize]
        [HttpGet]
        [Route("UserInfo/{userIdentifier:length(32)}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserAuthInfoResponse))]
        [SwaggerResponse(HttpStatusCode.BadGateway, "用户唯一标示错误")]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetUserInfo(string userIdentifier)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            UserInfo info = await user.GetUserInfoAsync();
            return this.Ok(info.ToResponse());
        }

        private AdminCancelAccount BuildAdminCancelAccountCommand(string userIdentifier)
        {
            return new AdminCancelAccount
            {
                CommandId = this.Request.BuildCommandId(),
                UserId = userIdentifier.ToGuid()
            };
        }

        private AdminModifyCellphoneCommand BuildAdminModifyCellphoneCommand(AdminModifyCellphoneRequest request)
        {
            return new AdminModifyCellphoneCommand
            {
                CommandId = this.Request.BuildCommandId(),
                UserId = request.UserIdentifier.ToGuid(),
                NewCellphone = request.NewCellphone
            };
        }
    }
}