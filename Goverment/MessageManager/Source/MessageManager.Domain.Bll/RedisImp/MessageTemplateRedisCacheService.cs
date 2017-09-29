// ***********************************************************************
// Project          : Jinyinmao.MessageManager
// File             : MessageTemplateRedisCacheService.cs
// Created          : 2015-12-13  20:08
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-13  21:58
// ***********************************************************************
// <copyright file="MessageTemplateRedisCacheService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.MessageManager.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     MessageTemplateRedisCacheService.
    /// </summary>
    public class MessageTemplateRedisCacheService : IMessageTemplateService
    {
        /// <summary>
        ///     The inner service
        /// </summary>
        private readonly IMessageTemplateService innerService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageTemplateRedisCacheService" /> class.
        /// </summary>
        /// <param name="innerService">The inner service.</param>
        public MessageTemplateRedisCacheService(IMessageTemplateService innerService)
        {
            this.innerService = innerService;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageTemplateRedisCacheService" /> class.
        /// </summary>
        public MessageTemplateRedisCacheService()
        {
            this.innerService = new MessageTemplateService();
        }

        #region IMessageTemplateService Members

        /// <summary>
        ///     Creates the asynchronous.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public Task<MessageTemplate> CreateAsync(MessageTemplate messageTemplate)
        {
            return this.innerService.CreateAsync(messageTemplate);
        }

        /// <summary>
        ///     Fills the tempalete.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="messageTemplate"></param>
        /// <returns>System.String.</returns>
        public string FillTempalete(Dictionary<string, string> args, MessageTemplate messageTemplate)
        {
            return this.innerService.FillTempalete(args, messageTemplate);
        }

        public Task<MessageTemplate> GetByBizcodeAsync(string bizCode)
        {
            return this.innerService.GetByBizcodeAsync(bizCode);
        }

        /// <summary>
        ///     Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="templateIdentifier">The template identifier.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public Task<MessageTemplate> GetByIdAsync(string templateIdentifier)
        {
            // 从redis中获取数据
            return this.innerService.GetByIdAsync(templateIdentifier);
        }

        /// <summary>
        ///     Gets the entity.
        /// </summary>
        /// <param name="bizCode"></param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public Task<MessageTemplate> GetEntityAsync(string bizCode)
        {
            return this.innerService.GetEntityAsync(bizCode);
        }

        /// <summary>
        ///     Gets the message templates.
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public Task<List<MessageTemplate>> GetMessageTemplatesAsync()
        {
            return this.innerService.GetMessageTemplatesAsync();
        }

        /// <summary>
        ///     Updates the asynchronous.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public Task<MessageTemplate> UpdateAsync(MessageTemplate messageTemplate)
        {
            return this.innerService.UpdateAsync(messageTemplate);
        }

        #endregion IMessageTemplateService Members
    }
}