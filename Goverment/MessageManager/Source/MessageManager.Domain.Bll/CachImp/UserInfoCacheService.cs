// ***********************************************************************
// Project          : MessageManager
// File             : UserInfoCacheService.cs
// Created          : 2015-11-30  18:08
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  18:08
// ***********************************************************************
// <copyright file="UserInfoCacheService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jinyinmao.MessageManager.Domain.Entity;
using Moe.Lib;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     UserInfoCacheService.
    /// </summary>
    public class UserInfoCacheService : IUserInfoService
    {
        /// <summary>
        ///     The inner service
        /// </summary>
        private readonly IUserInfoService innerService;

        /// <summary>
        ///     The messge template cache
        /// </summary>
        private readonly Dictionary<string, Tuple<UserInfo, DateTime>> messgeTemplateCache =
            new Dictionary<string, Tuple<UserInfo, DateTime>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInfoCacheService" /> class.
        /// </summary>
        /// <param name="messageUserInfoService">The message user information service.</param>
        public UserInfoCacheService(IUserInfoService messageUserInfoService)
        {
            this.innerService = messageUserInfoService;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInfoCacheService" /> class.
        /// </summary>
        public UserInfoCacheService()
        {
            this.innerService = new UserInfoService();
        }

        #region IUserInfoService Members

        /// <summary>
        ///     Creates the asynchronous.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> CreateAsync(UserInfo messageTemplate)
        {
            return this.innerService.CreateAsync(messageTemplate);
        }

        /// <summary>
        ///     get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="userKeyIdentifier">The user key identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> GetByIdAsync(string userKeyIdentifier)
        {
            Tuple<UserInfo, DateTime> cacheObject;
            if (this.messgeTemplateCache.TryGetValue(userKeyIdentifier, out cacheObject))
            {
                if (cacheObject.Item1 == null)
                {
                    cacheObject = new Tuple<UserInfo, DateTime>(
                        await this.innerService.GetByIdAsync(userKeyIdentifier), DateTime.UtcNow);
                }
                else if (cacheObject.Item2.IsBefore(DateTime.UtcNow, 5.Minutes()))
                {
#pragma warning disable 4014
                    Task.Run(async () =>
                    {
                        cacheObject = new Tuple<UserInfo, DateTime>(
                            await this.innerService.GetByIdAsync(userKeyIdentifier), DateTime.UtcNow);
                    }).Forget();
#pragma warning restore 4014
                }

                return cacheObject.Item1;
            }

            UserInfo messageTemplate = await this.innerService.GetByIdAsync(userKeyIdentifier);
            this.messgeTemplateCache[userKeyIdentifier] = new Tuple<UserInfo, DateTime>(messageTemplate, DateTime.UtcNow);
            return messageTemplate;
        }

        /// <summary>
        ///     Gets the by phone asynchronous.
        /// </summary>
        /// <param name="phonedentifier">The phonedentifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetByPhoneAsync(string phonedentifier)
        {
            return this.innerService.GetByPhoneAsync(phonedentifier);
        }

        /// <summary>
        ///     Gets the by u identifier asynchronous.
        /// </summary>
        /// <param name="uIdentifier">The u identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetByUIdAsync(string uIdentifier)
        {
            return this.innerService.GetByUIdAsync(uIdentifier);
        }

        /// <summary>
        ///     Gets the message templates.
        /// </summary>
        /// <returns>Task&lt;List&lt;UserInfo&gt;&gt;.</returns>
        public Task<List<UserInfo>> GetMessageTemplates()
        {
            return this.innerService.GetMessageTemplates();
        }

        /// <summary>
        ///     Updates the asynchronous.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> UpdateAsync(UserInfo messageTemplate)
        {
            return this.innerService.UpdateAsync(messageTemplate);
        }

        #endregion IUserInfoService Members
    }
}