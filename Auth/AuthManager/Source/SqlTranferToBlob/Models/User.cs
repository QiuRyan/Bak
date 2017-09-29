﻿using System;

namespace Jinyinmao.AuthManager.Domain.Core.Models
{
    /// <summary>
    ///     User.
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; set; }

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
        ///     Gets or sets a value indicating whether this <see cref="User" /> is closed.
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

        ///// <summary>
        /////     Gets or sets the invite for.
        ///// </summary>
        ///// <value>The invite for.</value>
        //public string InviteFor { get; set; }

        ///// <summary>
        ///// Gets or sets the last modified.
        ///// </summary>
        ///// <value>The last modified.</value>
        //public DateTime LastModified { get; set; }

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
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }
    }
}