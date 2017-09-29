using System;
using System.Collections.Generic;

namespace DataTransfer
{
    /// <summary>
    ///     Class UserInfo.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public long Balance { get; set; }

        /// <summary>
        ///     Gets or sets the bank cards count.
        /// </summary>
        /// <value>The bank cards count.</value>
        public int BankCardsCount { get; set; }

        /// <summary>
        ///     Gets or sets the bank investing interest.
        /// </summary>
        /// <value>The bank investing interest.</value>
        public long BankInvestingInterest { get; set; }

        /// <summary>
        ///     Gets or sets the bank investing principal.
        /// </summary>
        /// <value>The bank investing principal.</value>
        public long BankInvestingPrincipal { get; set; }

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
        ///     Gets or sets a value indicating whether this <see cref="UserInfo" /> is closed.
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
        ///     Gets or sets the crediting.
        /// </summary>
        /// <value>The crediting.</value>
        public long Crediting { get; set; }

        /// <summary>
        ///     Gets or sets the debiting.
        /// </summary>
        /// <value>The debiting.</value>
        public long Debiting { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has set password.
        /// </summary>
        /// <value><c>true</c> if this instance has set password; otherwise, <c>false</c>.</value>
        public bool HasSetPassword { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has set payment password.
        /// </summary>
        /// <value><c>true</c> if this instance has set payment password; otherwise, <c>false</c>.</value>
        public bool HasSetPaymentPassword { get; set; }

        /// <summary>
        ///     Gets or sets the investing interest.
        /// </summary>
        /// <value>The investing interest.</value>
        public long InvestingInterest { get; set; }

        /// <summary>
        ///     Gets or sets the investing principal.
        /// </summary>
        /// <value>The investing principal.</value>
        public long InvestingPrincipal { get; set; }

        /// <summary>
        ///     Gets or sets the invite by.
        /// </summary>
        /// <value>The invite by.</value>
        public string InviteBy { get; set; }

        /// <summary>
        ///     Gets or sets the jby accrual amount.
        /// </summary>
        /// <value>The jby accrual amount.</value>
        public long JBYAccrualAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby last interest.
        /// </summary>
        /// <value>The jby last interest.</value>
        public long JBYLastInterest { get; set; }

        /// <summary>
        ///     Gets or sets the jby total amount.
        /// </summary>
        /// <value>The jby total amount.</value>
        public long JBYTotalAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby total interest.
        /// </summary>
        /// <value>The jby total interest.</value>
        public long JBYTotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the jby total pricipal.
        /// </summary>
        /// <value>The jby total pricipal.</value>
        public long JBYTotalPricipal { get; set; }

        /// <summary>
        ///     Gets or sets the jby withdrawalable amount.
        /// </summary>
        /// <value>The jby withdrawalable amount.</value>
        public long JBYWithdrawalableAmount { get; set; }

        /// <summary>
        ///     Gets or sets the login names.
        /// </summary>
        /// <value>The login names.</value>
        public List<string> LoginNames { get; set; }

        /// <summary>
        ///     Gets or sets the month withdrawal count.
        /// </summary>
        /// <value>The month withdrawal count.</value>
        public int MonthWithdrawalCount { get; set; }

        /// <summary>
        ///     Gets or sets the oil card permission.
        /// </summary>
        /// <value>The oil card permission.</value>
        public bool OilCardPermission { get; set; }

        /// <summary>
        ///     Gets or sets the outlet code.
        /// </summary>
        /// <value>The outlet code.</value>
        public string OutletCode { get; set; }

        /// <summary>
        ///     Gets or sets the password error count.
        /// </summary>
        /// <value>The password error count.</value>
        public int PasswordErrorCount { get; set; }

        /// <summary>
        ///     Gets or sets the payment password error count.
        /// </summary>
        /// <value>The payment password error count.</value>
        public int PaymentPasswordErrorCount { get; set; }

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
        ///     Gets or sets the shang investing interest.
        /// </summary>
        /// <value>The shang investing interest.</value>
        public long ShangInvestingInterest { get; set; }

        /// <summary>
        ///     Gets or sets the shang investing principal.
        /// </summary>
        /// <value>The shang investing principal.</value>
        public long ShangInvestingPrincipal { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="UserInfo" /> is signed.
        /// </summary>
        /// <value><c>true</c> if signed; otherwise, <c>false</c>.</value>
        public bool Signed { get; set; }

        /// <summary>
        ///     Gets or sets the sign times in activity.
        /// </summary>
        /// <value>The sign times in activity.</value>
        public int SignTimesInActivity { get; set; }

        /// <summary>
        ///     Gets or sets the today jby withdrawal amount.
        /// </summary>
        /// <value>The today jby withdrawal amount.</value>
        public long TodayJBYWithdrawalAmount { get; set; }

        /// <summary>
        ///     Gets or sets the today withdrawal count.
        /// </summary>
        /// <value>The today withdrawal count.</value>
        public int TodayWithdrawalCount { get; set; }

        /// <summary>
        ///     Gets or sets the total interest.
        /// </summary>
        /// <value>The total interest.</value>
        public long TotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the total principal.
        /// </summary>
        /// <value>The total principal.</value>
        public long TotalPrincipal { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="UserInfo" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime? VerifiedTime { get; set; }

        /// <summary>
        ///     Gets or sets the withdrawalable amount.
        /// </summary>
        /// <value>The withdrawalable amount.</value>
        public long WithdrawalableAmount { get; set; }

        /// <summary>
        ///     Gets or sets the yin investing interest.
        /// </summary>
        /// <value>The yin investing interest.</value>
        public long YinInvestingInterest { get; set; }

        /// <summary>
        ///     Gets or sets the yin investing principal.
        /// </summary>
        /// <value>The yin investing principal.</value>
        public long YinInvestingPrincipal { get; set; }
    }
}