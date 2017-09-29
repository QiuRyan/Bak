// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserBizInfo.cs
// Created          : 2016-08-17  11:00 AM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-19  11:16 AM
// ***********************************************************************
// <copyright file="UserBizInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;

namespace Jinyinmao.AuthManager.Domain.Interface.Dtos
{
    [Immutable]
    public class UserBizInfo
    {
        public string CredentialNo { get; set; }

        public string RealName { get; set; }

        public bool Verified { get; set; }
    }
}