// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : IMessageManagerServices.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:20
// ***********************************************************************
// <copyright file="IMessageManagerServices.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult;
using MoeLib.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Service.Misc.Interface
{
    public interface IMessageManagerServices
    {
        Task<VerifyVeriCodeResult> SendMessageAsync(string bizCode, int channel, string cellphone, Dictionary<string, string> dic = null, TraceEntry traceEntry = null);
    }
}