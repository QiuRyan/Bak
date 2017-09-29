// ***********************************************************************
// Project          : MessageManager
// File             : MessageManagerDbContext.cs
// Created          : 2015-11-28  15:21
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  15:31
// ***********************************************************************
// <copyright file="MessageManagerDbContext.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using Jinyinmao.MessageManager.Api.Config;
using Jinyinmao.MessageManager.Domain.Entity.DbMap;
using Moe.Lib.Data;

namespace Jinyinmao.MessageManager.Domain.Entity
{
    /// <summary>
    ///     MessageManagerDbContext. This class cannot be inherited.
    /// </summary>
    public sealed class MessageManagerDbContext : DbContextBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageManagerDbContext" /> class.
        /// </summary>
        public MessageManagerDbContext()
            : base(ConfigsManager.MessageManagerDbContext)
        {
        }

        /// <summary>
        ///     Gets or sets the message templates.
        /// </summary>
        /// <value>The message templates.</value>
        public DbSet<MessageTemplate> MessageTemplates { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageTemplateMap());

            //modelBuilder.Configurations.Add(new UserInfoMap());
        }
    }
}