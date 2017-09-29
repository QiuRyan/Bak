// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : MessageRoleService.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  21:01
// ***********************************************************************
// <copyright file="MessageRoleService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Service.Coupon.Interface;
using Jinyinmao.AuthManager.Service.Coupon.Interface.Dtos;
using Jinyinmao.AuthManager.Service.User;
using Jinyinmao.AuthManager.Service.User.Interface;
using Moe.Lib;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Service.Coupon
{
    public class MessageRoleService : IMessageRoleService
    {
        private readonly IUserService userService = new UserService();

        #region IMessageRoleService Members

        public async Task SendRegisterMessageAsync(long? clientType, UserInfo userInfo, TraceEntry traceEntry)
        {
            RegisterMessageRequest registerMessageRequest;
            //默认为没有填写邀请码的用户
            if (string.IsNullOrWhiteSpace(userInfo.InviteBy) || userInfo.InviteBy.ToUpper() == "JYM" || userInfo.InviteBy.ToUpper() == "JINYINMAO")
            {
                registerMessageRequest = new RegisterMessageRequest
                {
                    Cellphone = userInfo.Cellphone,
                    ContractId = userInfo.ContractId,
                    InviterCellphone = "",
                    InviterIdentifier = "",
                    InviterInviteFor = "",
                    UserIdentifier = userInfo.UserId.ToGuidString(),
                    IsNeedToBuildRelationship = true
                };
            }
            else
            {
                registerMessageRequest = await this.GetRegisterMessage(clientType, userInfo, traceEntry);
            }

            HttpClient client = JYMInternalHttpClientFactory.Create("Jinyinmao.Coupon.Api", traceEntry);
            await client.PostAsJsonAsync("/api/MessageRole/ReceiveRegisterMessage", registerMessageRequest);
        }

        #endregion IMessageRoleService Members

        private async Task<RegisterMessageRequest> GetRegisterMessage(long? clientType, UserInfo userInfo, TraceEntry traceEntry)
        {
            UserInfo inviterUserInfo = await this.userService.GetInviterUserByInviteByAsync(userInfo.InviteBy.ToUpper());
            RegisterMessageRequest registerMessageRequest = new RegisterMessageRequest
            {
                Cellphone = userInfo.Cellphone,
                ContractId = userInfo.ContractId,
                InviterCellphone = "",
                InviterIdentifier = "",
                InviterInviteFor = userInfo.InviteBy.ToUpper(),
                UserIdentifier = userInfo.UserId.ToGuidString(),
                IsNeedToBuildRelationship = false
            };
            switch (clientType)
            {
                case 900:
                    registerMessageRequest.Terminal = 0;
                    break;

                case 901:
                case 902:
                    registerMessageRequest.Terminal = 1;
                    break;

                case 903:
                    registerMessageRequest.Terminal = 2;
                    break;
            }
            if (inviterUserInfo != null)
            {
                registerMessageRequest.InviterCellphone = inviterUserInfo.Cellphone;
                registerMessageRequest.InviterIdentifier = inviterUserInfo.UserId.ToGuidString();
                registerMessageRequest.IsNeedToBuildRelationship = true;
            }

            return registerMessageRequest;
        }
    }
}