using System;
using System.Collections.Generic;

namespace SqlTranferToBlob
{
    /// <summary>
    ///     BankCard.
    /// </summary>
    public class BankCard
    {
        /// <summary>
        ///     Gets or sets the adding time.
        /// </summary>
        /// <value>The adding time.</value>
        public DateTime AddingTime { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BankCard" /> is dispaly.
        /// </summary>
        /// <value><c>true</c> if dispaly; otherwise, <c>false</c>.</value>
        public bool Dispaly { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BankCard" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [verified by yilian].
        /// </summary>
        /// <value><c>true</c> if [verified by yilian]; otherwise, <c>false</c>.</value>
        public bool VerifiedByYilian { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime? VerifiedTime { get; set; }

        /// <summary>
        ///     Gets or sets the withdraw amount.
        /// </summary>
        /// <value>The withdraw amount.</value>
        public long WithdrawAmount { get; set; }

        /// <summary>
        ///     Determines whether [is not verified].
        /// </summary>
        /// <returns><c>true</c> if [is not verified]; otherwise, <c>false</c>.</returns>
        public bool IsNotVerified()
        {
            return !this.Verified && !this.VerifiedByYilian;
        }
    }
}