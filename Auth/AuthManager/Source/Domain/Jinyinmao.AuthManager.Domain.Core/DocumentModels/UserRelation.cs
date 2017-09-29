// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserRelation.cs
// Created          : 2016-12-14  17:46
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-15  13:18
// ***********************************************************************
// <copyright file="UserRelation.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries;
using System;

namespace Jinyinmao.AuthManager.Domain.Core.DocumentModels
{
    /// <summary>
    ///     Class UserRelation.
    /// </summary>
    public class UserRelation : DocumentBase
    {
        /// <summary>
        ///     Gets or sets the type of the account.
        /// </summary>
        /// <value>The type of the account.</value>
        public int AccountType { get; set; }

        /// <summary>
        ///     Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is alive.
        /// </summary>
        /// <value><c>true</c> if this instance is alive; otherwise, <c>false</c>.</value>
        public bool IsAlive { get; set; }

        /// <summary>
        ///     Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }

        /// <summary>
        ///     Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }
    }
}