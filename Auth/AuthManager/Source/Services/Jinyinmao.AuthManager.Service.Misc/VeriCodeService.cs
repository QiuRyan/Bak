// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : VeriCodeService.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:20
// ***********************************************************************
// <copyright file="VeriCodeService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries;
using Jinyinmao.AuthManager.Service.Misc.Interface;
using Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult;
using Jinyinmao.AuthManager.Service.Misc.Request;
using Moe.Lib.Jinyinmao;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Service.Misc
{
    public class VeriCodeService : IVeriCodeService
    {
        private string VeriCodeServiceRole
        {
            get { return App.Configurations.GetConfig<AuthApiConfig>().VeriCodeServiceRole; }
        }

        /// <summary>
        ///     Uses the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;UseVeriCodeResult&gt;.</returns>
        public async Task<UseVeriCodeResult> UseAsync(string token, int type, TraceEntry traceEntry)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.VeriCodeServiceRole, traceEntry);

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("/api/ValidateCodes/Use", new UseVeriCodeRequest
            {
                Token = token,
                Type = type
            });

            UseVeriCodeResult result = await responseMessage.Content.ReadAsAsync<UseVeriCodeResult>();
            return result;
        }

        /// <summary>
        ///     Verifies the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;VerifyVeriCodeResult&gt;.</returns>
        public async Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, int type, TraceEntry traceEntry = null)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.VeriCodeServiceRole, traceEntry);

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("/api/ValidateCodes/Verify", new VerifyVeriCodeRequest
            {
                Cellphone = cellphone,
                Code = code,
                Type = type
            });
            return await responseMessage.Content.ReadAsAsync<VerifyVeriCodeResult>();
        }
    }
}