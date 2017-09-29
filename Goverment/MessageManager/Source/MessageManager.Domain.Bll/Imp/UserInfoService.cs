// ***********************************************************************
// Project          : MessageManager
// File             : UserInfoService.cs
// Created          : 2015-11-30  17:54
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  17:54
// ***********************************************************************
// <copyright file="UserInfoService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Jinyinmao.MessageManager.Domain.Entity;
using Moe.Lib;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    /// UserInfoService.
    /// </summary>
    public class UserInfoService : IUserInfoService
    {
        #region IUserInfoService Members

        /// <summary>
        /// create as an asynchronous operation.
        /// </summary>
        /// <param name="userInfoobj">The user infoobj.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> CreateAsync(UserInfo userInfoobj)
        {
            userInfoobj.UserKey = Guid.NewGuid().ToGuidString();
            userInfoobj.IsValid = 1;

            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                db.Add(userInfoobj);

                await db.ExecuteSaveChangesAsync();
            }

            return userInfoobj;
        }

        /// <summary>
        /// get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="userKeyIdentifier">The user key identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> GetByIdAsync(string userKeyIdentifier)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<UserInfo>().FirstOrDefaultAsync(t => t.UserKey == userKeyIdentifier);
            }
        }

        /// <summary>
        /// get by phone as an asynchronous operation.
        /// </summary>
        /// <param name="phonedentifier">The phonedentifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> GetByPhoneAsync(string phonedentifier)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<UserInfo>().FirstOrDefaultAsync(t => t.PhoneNum == phonedentifier);
            }
        }

        /// <summary>
        /// Gets the message templates.
        /// </summary>
        /// <returns>Task&lt;List&lt;UserInfo&gt;&gt;.</returns>
        public async Task<List<UserInfo>> GetMessageTemplates()
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<UserInfo>().Where(c => c.IsValid == 1).ToListAsync();
            }
        }

        /// <summary>
        /// update as an asynchronous operation.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> UpdateAsync(UserInfo messageTemplate)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                db.Set<UserInfo>().Attach(messageTemplate);
                db.Entry(messageTemplate).State = EntityState.Modified;
                await db.ExecuteSaveChangesAsync();
            }
            return messageTemplate;
        }

        #endregion IUserInfoService Members

        /// <summary>
        /// get by u identifier as an asynchronous operation.
        /// </summary>
        /// <param name="uiddentifier">The uiddentifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> GetByUIdAsync(string uiddentifier)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<UserInfo>().FirstOrDefaultAsync(t => t.UId == uiddentifier);
            }
        }
    }
}