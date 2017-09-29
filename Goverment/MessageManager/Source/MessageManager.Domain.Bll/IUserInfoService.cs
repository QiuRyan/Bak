// ***********************************************************************
// Project          : MessageManager
// File             : IUserInfoService.cs
// Created          : 2015-11-30  17:48
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  17:48
// ***********************************************************************
// <copyright file="IUserInfoService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using Jinyinmao.MessageManager.Domain.Entity;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    /// Interface IUserInfoService
    /// </summary>
    public interface IUserInfoService
    {
        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="userinfoobj">The userinfoobj.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> CreateAsync(UserInfo userinfoobj);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="userKeyIdentifier">The user key identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetByIdAsync(string userKeyIdentifier);

        /// <summary>
        /// Gets the by phone asynchronous.
        /// </summary>
        /// <param name="phonedentifier">The phonedentifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetByPhoneAsync(string phonedentifier);

        /// <summary>
        /// Gets the by u identifier asynchronous.
        /// </summary>
        /// <param name="uiddentifier">The uiddentifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetByUIdAsync(string uiddentifier);

        /// <summary>
        /// Gets the message templates.
        /// </summary>
        /// <returns>Task&lt;List&lt;UserInfo&gt;&gt;.</returns>
        Task<List<UserInfo>> GetMessageTemplates();

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="userinfoobj">The userinfoobj.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> UpdateAsync(UserInfo userinfoobj);
    }
}