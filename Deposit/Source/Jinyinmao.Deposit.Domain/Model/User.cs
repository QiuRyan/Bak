// ***********************************************************************
// Project          : Jinyinmao.Tirisfal
// File             : User.cs
// Created          : 2016-09-05  10:58
//
//
// Last Modified On : 2016-10-18  16:56
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    ///     User.
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [activation time].
        /// </summary>
        /// <value><c>null</c> if [activation time] contains no value, <c>true</c> if [activation time]; otherwise, <c>false</c>.</value>
        public DateTime? ActivationTime { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [authentication withdraw].
        /// </summary>
        /// <value><c>null</c> if [authentication withdraw] contains no value, <c>true</c> if [authentication withdraw]; otherwise, <c>false</c>.</value>
        public bool? AuthWithdraw { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the cg bank card cellphone.
        /// </summary>
        /// <value>The cg bank card cellphone.</value>
        public string CgBankCardCellphone { get; set; }

        /// <summary>
        ///     Gets or sets the cg bank card no.
        /// </summary>
        /// <value>The cg bank card no.</value>
        public string CgBankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the cg bank code.
        /// </summary>
        /// <value>The cg bank code.</value>
        public string CgBankCode { get; set; }

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
        ///     Gets or sets a value indicating whether [entrusted pay].
        /// </summary>
        /// <value><c>null</c> if [entrusted pay] contains no value, <c>true</c> if [entrusted pay]; otherwise, <c>false</c>.</value>
        public bool? EntrustedPay { get; set; }

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
        ///     Gets or sets a value indicating whether this instance is activation.
        /// </summary>
        /// <value><c>null</c> if [is activation] contains no value, <c>true</c> if [is activation]; otherwise, <c>false</c>.</value>
        public bool? IsActivation { get; set; }

        /// <summary>
        ///     Gets or sets the Kind Of Investor.
        /// </summary>
        /// <value>The Kind Of Investor.</value>
        public int KindOfInvestor { get; set; }

        /// <summary>
        ///     Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }

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
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="User" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets the verified key.
        /// </summary>
        /// <value>The verified key.</value>
        public string VerifiedKey { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime? VerifiedTime { get; set; }
    }
}