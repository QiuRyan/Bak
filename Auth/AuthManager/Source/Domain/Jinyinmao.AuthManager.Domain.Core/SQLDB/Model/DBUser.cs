// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserModel.cs
// Created          : 2016-12-28  13:49
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-02-03  11:19
// ***********************************************************************
// <copyright file="UserModel.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Jinyinmao.AuthManager.Domain.Core.SQLDB.Model
{
    /// <summary>
    ///     Class UserModel.
    /// </summary>
    public class DBUser
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the type of the client.
        /// </summary>
        /// <value>The type of the client.</value>
        public long ClientType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="DBUser" /> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        public bool Closed { get; set; }

        /// <summary>
        ///     Gets or sets the contract identifier.
        /// </summary>
        /// <value>The contract identifier.</value>
        public long ContractId { get; set; }

        /// <summary>
        ///     Gets or sets the encrypted password.
        /// </summary>
        /// <value>The encrypted password.</value>
        public string EncryptedPassword { get; set; }

        /// <summary>
        ///     Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info { get; set; }

        /// <summary>
        ///     Gets or sets the invite by.
        /// </summary>
        /// <value>The invite by.</value>
        public string InviteBy { get; set; }

        /// <summary>
        ///     Gets or sets the invite for.
        /// </summary>
        /// <value>The invite for.</value>
        public string InviteFor { get; set; }

        /// <summary>
        ///     Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///     Gets or sets the login names.
        /// </summary>
        /// <value>The login names.</value>
        public string LoginNames { get; set; }

        /// <summary>
        ///     Gets or sets the outlet code.
        /// </summary>
        /// <value>The outlet code.</value>
        public string OutletCode { get; set; }

        /// <summary>
        ///     Gets or sets the register time.
        /// </summary>
        /// <value>The register time.</value>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        ///     Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public string Salt { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }
    }
}