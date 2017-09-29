// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : IVeriCodeService.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:20
// ***********************************************************************
// <copyright file="IVeriCodeService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult;
using MoeLib.Diagnostics;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Service.Misc.Interface
{
    public interface IVeriCodeService
    {
        /// <summary>
        ///     Uses the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;UseVeriCodeResult&gt;.</returns>
        Task<UseVeriCodeResult> UseAsync(string token, int type, TraceEntry traceEntry = null);

        /// <summary>
        ///     Verifies the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;VerifyVeriCodeResult&gt;.</returns>
        Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, int type, TraceEntry traceEntry = null);
    }
}