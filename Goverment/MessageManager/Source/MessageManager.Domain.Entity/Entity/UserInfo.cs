// ***********************************************************************
// Project          : MessageManager
// File             : UserInfo.cs
// Created          : 2015-11-30  17:29
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  17:29
// ***********************************************************************
// <copyright file="UserInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Jinyinmao.MessageManager.Domain.Entity
{
    /// <summary>
    /// UserInfo.
    /// </summary>
    public partial class UserInfo
    {
        /// <summary>
        /// 是否有效：1代表有效，0代表无效
        /// </summary>
        /// <value>The is valid.</value>
        public int IsValid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        /// <value>The phone number.</value>
        public string PhoneNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <value>The remark.</value>
        public string Remark { get; set; }

        /// <summary>
        /// 终端Id
        /// </summary>
        /// <value>The u application identifier.</value>
        public string UAppId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        /// <value>The u identifier.</value>
        public string UId { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        /// <value>The user key.</value>
        public string UserKey { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        /// <value>The we chat number.</value>
        public string WeChatNum { get; set; }
    }
}