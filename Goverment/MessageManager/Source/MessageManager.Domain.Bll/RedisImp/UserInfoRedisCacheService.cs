// ***********************************************************************
// Project          : MessageManager
// File             : UserInfoRedisCacheService.cs
// Created          : 2015-11-30  18:12
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  18:12
// ***********************************************************************
// <copyright file="UserInfoRedisCacheService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using Jinyinmao.MessageManager.Domain.Entity;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    /// UserInfoRedisCacheService.
    /// </summary>
    public class UserInfoRedisCacheService : IUserInfoService
    {
        /// <summary>
        /// The inner service
        /// </summary>
        private readonly IUserInfoService innerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoRedisCacheService"/> class.
        /// </summary>
        /// <param name="innerService">The inner service.</param>
        public UserInfoRedisCacheService(IUserInfoService innerService)
        {
            this.innerService = innerService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoRedisCacheService"/> class.
        /// </summary>
        public UserInfoRedisCacheService()
        {
            this.innerService = new UserInfoService();
        }

        #region IUserInfoService Members

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="userinfoobj">The userinfoobj.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> CreateAsync(UserInfo userinfoobj)
        {
            return this.innerService.CreateAsync(userinfoobj);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="userKeyIdentifier">The user key identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetByIdAsync(string userKeyIdentifier)
        {
            // 从redis中获取数据
            return this.innerService.GetByIdAsync(userKeyIdentifier);
        }

        /// <summary>
        /// Gets the by phone asynchronous.
        /// </summary>
        /// <param name="phonedentifier">The phonedentifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetByPhoneAsync(string phonedentifier)
        {
            return this.innerService.GetByPhoneAsync(phonedentifier);
        }

        /// <summary>
        /// Gets the by u identifier asynchronous.
        /// </summary>
        /// <param name="uIdentifier">The u identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetByUIdAsync(string uIdentifier)
        {
            return this.innerService.GetByUIdAsync(uIdentifier);
        }

        /// <summary>
        /// Gets the message templates.
        /// </summary>
        /// <returns>Task&lt;List&lt;UserInfo&gt;&gt;.</returns>
        public Task<List<UserInfo>> GetMessageTemplates()
        {
            return this.innerService.GetMessageTemplates();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="userinfoobj">The userinfoobj.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> UpdateAsync(UserInfo userinfoobj)
        {
            return this.innerService.UpdateAsync(userinfoobj);
        }

        #endregion IUserInfoService Members
    }
}