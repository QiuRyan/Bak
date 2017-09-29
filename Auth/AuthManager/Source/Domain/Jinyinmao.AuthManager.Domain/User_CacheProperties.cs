using Moe.Lib;
using System;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Domain
{
    public partial class User
    {
        /// <summary>
        ///     Gets or sets the invite for.
        /// </summary>
        /// <value>The invite for.</value>
        public string InviteFor { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        private string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the type of the client.
        /// </summary>
        /// <value>The type of the client.</value>
        private long ClientType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="User" /> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        private bool Closed { get; set; }

        /// <summary>
        ///     Gets or sets the contract identifier.
        /// </summary>
        /// <value>The contract identifier.</value>
        private long ContractId { get; set; }

        /// <summary>
        ///     Gets or sets the encrypted password.
        /// </summary>
        /// <value>The encrypted password.</value>
        private string EncryptedPassword { get; set; }

        /// <summary>
        ///     微信是否绑定，不需要同步到User表.
        /// </summary>
        /// <value>The flag.</value>
        private bool Flag { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance has set password.
        /// </summary>
        /// <value><c>true</c> if this instance has set password; otherwise, <c>false</c>.</value>
        private bool HasSetPassword
        {
            get { return this.EncryptedPassword.IsNotNullOrEmpty(); }
        }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        private Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     Gets or sets the invite by.
        /// </summary>
        /// <value>The invite by.</value>
        private string InviteBy { get; set; }

        /// <summary>
        ///     Gets or sets the login names.
        /// </summary>
        /// <value>The login names.</value>
        private List<string> LoginNames { get; set; }

        /// <summary>
        ///     微信openId，不需要同步到User表.
        /// </summary>
        /// <value>The flag.</value>
        private string OpenId { get; set; }

        /// <summary>
        ///     Gets or sets the outlet code.
        /// </summary>
        /// <value>The outlet code.</value>
        private string OutletCode { get; set; }

        /// <summary>
        ///     Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        private int PasswordErrorCount { get; set; }

        /// <summary>
        ///     Gets or sets the register time.
        /// </summary>
        /// <value>The register time.</value>
        private DateTime RegisterTime { get; set; }

        /// <summary>
        ///     Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        private string Salt { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        private Guid UserId { get; set; }
    }
}