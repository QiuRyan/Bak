// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : IMessageRoleService.cs
// Created          : 2016-04-25  10:23 AM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-04-25  1:25 PM
// ***********************************************************************
// <copyright file="IMessageRoleService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using MoeLib.Diagnostics;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Service.Coupon.Interface
{
    public interface IMessageRoleService
    {
        Task SendRegisterMessageAsync(long? clientType, UserInfo userInfo, TraceEntry traceEntry);
    }
}