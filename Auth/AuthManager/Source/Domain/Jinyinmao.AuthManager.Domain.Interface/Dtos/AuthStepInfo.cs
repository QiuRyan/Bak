// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : AuthStepInfo.cs
// Created          : 2016-08-12  6:23 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-15  5:41 PM
// ***********************************************************************
// <copyright file="AuthStepInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Dtos
{
    public class AuthStepInfo
    {
        public DateTime CreateTime { get; set; }

        public string PreviousToken { get; set; }

        public string SMSToken { get; set; }

        public string Token { get; set; }
        public string UserIdentifier { get; set; }

        public string ValidateMessage { get; set; }
    }
}