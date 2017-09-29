// ***********************************************************************
// Project          : MessageManager
// File             : UserInfoController.cs
// Created          : 2015-12-07  20:34
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-08  15:06
// ***********************************************************************
// <copyright file="UserInfoController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Jinyinmao.MessageManager.Api.Models;
using Jinyinmao.MessageManager.Domain.Bll;
using Jinyinmao.MessageManager.Domain.Entity;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web;

namespace Jinyinmao.MessageManager.Api.Controllers
{
    /// <summary>
    /// </summary>
    [RoutePrefix("api/UserInfo")]
    public class UserInfoController : JinyinmaoApiController
    {
        private readonly IUserInfoService messageUserinfoService;

        /// <summary>
        /// </summary>
        /// <param name="messageuserinfoService"></param>
        public UserInfoController(IUserInfoService messageuserinfoService)
        {
            this.messageUserinfoService = messageuserinfoService;
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="request">对象</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> Create(UserInfoRequest request)
        {
            //this.Warn("This is the log message of Warn level.");

            UserInfo objuserinfoisexist = await this.messageUserinfoService.GetByUIdAsync(request.UId);
            if (objuserinfoisexist == null)
            {
                UserInfo messageTemplate = await this.messageUserinfoService.CreateAsync(request.ToEntity());

                if (messageTemplate == null)
                {
                    return this.NotFound();
                }
                return this.Ok(messageTemplate);
            }
            else
            {
                UserInfo objuserinfo = request.ToEntity();
                objuserinfo.IsValid = 1;
                objuserinfo.UserKey = objuserinfoisexist.UserKey;
                UserInfo messageTemplate = await this.messageUserinfoService.UpdateAsync(objuserinfo);
                if (messageTemplate == null)
                {
                    return this.NotFound();
                }
                return this.Ok(objuserinfo);
            }
        }

        /// <summary>
        ///     根据手机号获取
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ByPhoneNum/{PhoneNum}")]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> GetByPhoneAsync(string phoneNum)
        {
            return this.Ok((await this.messageUserinfoService.GetByPhoneAsync(phoneNum)).ToResponse());
        }

        /// <summary>
        ///     根据唯一标示获取对象
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ByUId/{UId}")]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> GetByUIdAsync(string uid)
        {
            return this.Ok((await this.messageUserinfoService.GetByUIdAsync(uid)).ToResponse());
        }

        /// <summary>
        ///     通过用户Id获取数据
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userKey" />
        [HttpGet]
        [Route("ById/{userKey}")]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> GetUserInfoById(string userKey)
        {
            return this.Ok((await this.messageUserinfoService.GetByIdAsync(userKey)).ToResponse());
        }

        /// <summary>
        ///     查询所有用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UserList")]
        [ResponseType(typeof(List<MessageTemplateResponse>))]
        public async Task<IHttpActionResult> Index()
        {
            return this.Ok((await this.messageUserinfoService.GetMessageTemplates()).Select(t => t.ToResponse()));
        }

        /// <summary>
        ///     逻辑删除数据
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete/{requestId}")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> LogicDeleteAsync(string requestId)
        {
            UserInfo objuserinfo = await this.messageUserinfoService.GetByUIdAsync(requestId);
            if (objuserinfo != null)
            {
                objuserinfo.IsValid = 0;
                UserInfo saveusInfo = await this.messageUserinfoService.UpdateAsync(objuserinfo);
                if (saveusInfo == null)
                {
                    return this.Ok(false);
                }
            }
            return this.Ok(true);
        }

        /// <summary>
        ///     编辑用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [Route("UpdateAsync")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> UpdateAsync(UserInfoRequest request)
        {
            UserInfo objuserinfonotmodify = await this.messageUserinfoService.GetByUIdAsync(request.UId);
            if (objuserinfonotmodify != null)
            {
                UserInfo objuserinfo = request.ToEntity();
                objuserinfo.IsValid = objuserinfonotmodify.IsValid;
                objuserinfo.UserKey = objuserinfonotmodify.UserKey;
                UserInfo messageTemplate = await this.messageUserinfoService.UpdateAsync(objuserinfo);
                if (messageTemplate == null)
                {
                    return this.NotFound();
                }
                return this.Ok(objuserinfo);
            }
            return this.NotFound();
        }
    }
}