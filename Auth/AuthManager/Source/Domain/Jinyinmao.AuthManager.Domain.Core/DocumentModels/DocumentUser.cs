// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : DocumentUsers.cs
// Created          : 2016-11-30  18:35
//
// Last Modified By : 余亮(nigel-yu@hotmail.com)
// Last Modified On : 2016-11-30  18:35
// ***********************************************************************
// <copyright file="DocumentUsers.cs" company="Shanghai Jinyinmao Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Jinyinmao Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries;
using System;

namespace Jinyinmao.AuthManager.Domain.Core.DocumentModels
{
    /// <summary>
    ///     User.
    /// </summary>
    public class DocumentUser : DocumentBase
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
        /// Gets or sets a value indicating whether this <see cref="DocumentUser" /> is closed.
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