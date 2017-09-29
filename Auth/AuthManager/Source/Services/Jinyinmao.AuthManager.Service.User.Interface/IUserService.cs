// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : IUserService.cs
// Created          : 2016-07-06  2:01 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-22  5:10 PM
// ***********************************************************************
// <copyright file="IUserService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Service.User.Interface.Dtos;
using MoeLib.Diagnostics;
using System;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Service.User.Interface
{
    /// <summary>
    ///     Interface IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     Admins the cancel account asynchronous.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        Task<string> AdminCancelAccountAsync(AdminCancelAccount command);

        /// <summary>
        ///     Admins the modify cellphone asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task AdminModifyCellphoneAsync(AdminModifyCellphoneCommand command);

        /// <summary>
        ///     Changes the login cellphone asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> ChangeLoginCellphoneAsync(ChangeLoginCellphone command);

        /// <summary>
        ///     Checks the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;CheckCellphoneResult&gt;.</returns>
        Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone);

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CheckPasswordAsync(string userIdentifier, string password);

        /// <summary>
        ///     Checks the password via cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        Task<SignInResult> CheckPasswordViaCellphoneAsync(string cellphone, string password);

        /// <summary>
        ///     Deletes the we chat relation asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteWeChatRelationAsync(string userIdentifier);

        /// <summary>
        ///     Gets the inviter user by invite by asynchronous.
        /// </summary>
        /// <param name="inviteBy">The invite by.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetInviterUserByInviteByAsync(string inviteBy);

        /// <summary>
        ///     Gets the open identifier asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetOpenIdAsync(string code);

        /// <summary>
        ///     Gets the user by cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetUserByCellphoneAsync(string cellphone);

        //Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone);
        //Task<string> GetPartitionKeyAsync(string rowKey, int accountType);
        /// <summary>
        ///     Gets the user identifier by open identifier asynchronous.
        /// </summary>
        /// <param name="openId">The open identifier.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetUserIdentifierByOpenIdAsync(string openId);

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetUserInfoAsync(Guid userId);

        Task<UserBizInfo> GetUserInfoFromTirisferAsync(string userIdentifier, TraceEntry traceEntry = null);

        /// <summary>
        ///     Gets the we chat bind information by identifier asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>Task&lt;BindInfo&gt;.</returns>
        Task<BindInfo> GetWeChatBindInfoByIdAsync(string userIdentifier);

        Task<bool> IsValidateCredential(SetThirdAuthStep command);

        Task<bool> IsValidateOldCellphone(SetSecondAuthStep command);

        /// <summary>
        ///     Locks the asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> LockAsync(string userIdentifier);

        /// <summary>
        ///     Registers the user asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="traceEntry">The trace entry.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> RegisterUserAsync(UserRegister command, TraceEntry traceEntry = null);

        /// <summary>
        ///     Resets the login password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> ResetLoginPasswordAsync(ResetLoginPassword command);

        Task<AuthStepInfo> SetFirstAuthStepAsync(SetFirstAuthStep command);

        /// <summary>
        ///     Sets the login password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> SetLoginPasswordAsync(SetLoginPassword command);

        Task<AuthStepInfo> SetSecondAuthStepAsync(SetSecondAuthStep command);

        /// <summary>
        ///     Sets the third authentication step asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<AuthStepInfo> SetThirdAuthStepAsync(SetThirdAuthStep command);

        /// <summary>
        ///     Uns the lock asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> UnLockAsync(string userIdentifier);

        /// <summary>
        ///     Unregisters the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task.</returns>
        Task UnregisterAsync(string cellphone);

        /// <summary>
        ///     Wes the chat bind asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task WeChatBindAsync(WeChatBind command);

        /// <summary>
        ///     Wes the chat sign up asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> WeChatSignUpAsync(WeChatRegister command);
    }
}