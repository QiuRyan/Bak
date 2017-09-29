// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : MessageManagerServices.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:20
// ***********************************************************************
// <copyright file="MessageManagerServices.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries;
using Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult;
using Jinyinmao.AuthManager.Service.Misc.Request;
using Moe.Lib.Jinyinmao;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jinyinmao.AuthManager.Service.Misc.Interface;

namespace Jinyinmao.AuthManager.Service.Misc
{
    public class MessageManagerServices : IMessageManagerServices
    {
        private string MessageManagerRole
        {
            get { return App.Configurations.GetConfig<AuthApiConfig>().MessageManagerRole; }
        }

        #region IMessageManagerServices Members

        public async Task<VerifyVeriCodeResult> SendMessageAsync(string bizCode, int channel, string cellphone, Dictionary<string, string> dic = null, TraceEntry traceEntry = null)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.MessageManagerRole, traceEntry);

            if (dic == null) dic = new Dictionary<string, string>();

            SendWithTemplateRequest request = new SendWithTemplateRequest
            {
                BizCode = bizCode,
                Channel = channel,
                TemplateParams = dic,
                UserInfoList = new List<UserInfo> { new UserInfo { PhoneNum = cellphone } }
            };

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/MessageManager/SendWithTemplate", request);

            return await responseMessage.Content.ReadAsAsync<VerifyVeriCodeResult>();
        }

        #endregion IMessageManagerServices Members
    }
}