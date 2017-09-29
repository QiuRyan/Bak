using System;
using System.Collections.Generic;

namespace SqlTranferToBlob
{
    /// <summary>
    ///     Class UserState.
    /// </summary>
    public class UserState
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the bank cards.
        /// </summary>
        /// <value>The bank cards.</value>
        public Dictionary<string, BankCard> BankCards { get; set; }

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
        ///     Gets or sets a value indicating whether this <see cref="UserState" /> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        public bool Closed { get; set; }

        /// <summary>
        ///     Gets or sets the contract identifier.
        /// </summary>
        /// <value>The contract identifier.</value>
        public long ContractId { get; set; }

        /// <summary>
        ///     Gets or sets the credential.
        /// </summary>
        /// <value>The credential.</value>
        public int Credential { get; set; }

        /// <summary>
        ///     Gets or sets the credential no.
        /// </summary>
        /// <value>The credential no.</value>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     Gets or sets the encrypted password.
        /// </summary>
        /// <value>The encrypted password.</value>
        public string EncryptedPassword { get; set; }

        /// <summary>
        ///     Gets or sets the encrypted payment password.
        /// </summary>
        /// <value>The encrypted payment password.</value>
        public string EncryptedPaymentPassword { get; set; }

        /// <summary>
        ///     Gets or sets the jby account.
        /// </summary>
        /// <value>The jby account.</value>
        public Dictionary<Guid, JBYAccountTransaction> JBYAccount { get; set; }

        /// <summary>
        ///     Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }

        /// <summary>
        ///     Gets or sets the login names.
        /// </summary>
        /// <value>The login names.</value>
        public List<string> LoginNames { get; set; }

        /// <summary>
        ///     Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        public Dictionary<Guid, Order> Orders { get; set; }

        /// <summary>
        ///     Gets or sets the outlet code.
        /// </summary>
        /// <value>The outlet code.</value>
        public string OutletCode { get; set; }

        /// <summary>
        ///     Gets or sets the payment salt.
        /// </summary>
        /// <value>The payment salt.</value>
        public string PaymentSalt { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real.
        /// </summary>
        /// <value>The name of the real.</value>
        public string RealName { get; set; }

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
        ///     Gets or sets the settle account.
        /// </summary>
        /// <value>The settle account.</value>
        public Dictionary<Guid, SettleAccountTransaction> SettleAccount { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="UserState" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime? VerifiedTime { get; set; }
    }
}