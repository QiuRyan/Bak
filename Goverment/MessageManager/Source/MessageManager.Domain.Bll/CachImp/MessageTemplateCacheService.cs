// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplateCacheService.cs
// Created          : 2015-11-28  15:40
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  17:04
// ***********************************************************************
// <copyright file="MessageTemplateCacheService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jinyinmao.MessageManager.Domain.Entity;
using Moe.Lib;

#pragma warning disable 4014

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     MessageTemplateCacheService.
    /// </summary>
    public class MessageTemplateCacheService : IMessageTemplateService
    {
        /// <summary>
        ///     The inner service
        /// </summary>
        private readonly IMessageTemplateService innerService;

        /// <summary>
        ///     The messge template cache
        /// </summary>
        private readonly Dictionary<string, Tuple<MessageTemplate, DateTime>> messgeTemplateCache =
            new Dictionary<string, Tuple<MessageTemplate, DateTime>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageTemplateCacheService" /> class.
        /// </summary>
        /// <param name="messageTemplateService">The message template service.</param>
        public MessageTemplateCacheService(IMessageTemplateService messageTemplateService)
        {
            this.innerService = messageTemplateService;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageTemplateCacheService" /> class.
        /// </summary>
        public MessageTemplateCacheService()
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

        /// <summary>
        /// </summary>
        /// <param name="bizCode"></param>
        /// <returns></returns>
        public async Task<MessageTemplate> GetByBizcodeAsync(string bizCode)
        {
            Tuple<MessageTemplate, DateTime> cacheObject;
            if (this.messgeTemplateCache.TryGetValue(bizCode + "template", out cacheObject))
            {
                if (cacheObject.Item1 == null)
                {
                    cacheObject = new Tuple<MessageTemplate, DateTime>(await this.innerService.GetByIdAsync(bizCode), DateTime.UtcNow);
                }
                else if (cacheObject.Item2.IsBefore(DateTime.UtcNow, 5.Minutes()))
                {
                    Task.Run(async () => { cacheObject = new Tuple<MessageTemplate, DateTime>(await this.innerService.GetByIdAsync(bizCode), DateTime.UtcNow); }).Forget();
                }

                return cacheObject.Item1;
            }

            MessageTemplate messageTemplate = await this.innerService.GetByBizcodeAsync(bizCode);
            this.messgeTemplateCache[bizCode + "template"] = new Tuple<MessageTemplate, DateTime>(messageTemplate, DateTime.UtcNow);
            return messageTemplate;
        }

        /// <summary>
        ///     get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="templateIdentifier">The template identifier.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public async Task<MessageTemplate> GetByIdAsync(string templateIdentifier)
        {
            Tuple<MessageTemplate, DateTime> cacheObject;
            if (this.messgeTemplateCache.TryGetValue(templateIdentifier, out cacheObject))
            {
                if (cacheObject.Item1 == null)
                {
                    cacheObject = new Tuple<MessageTemplate, DateTime>(await this.innerService.GetByIdAsync(templateIdentifier), DateTime.UtcNow);
                }
                else if (cacheObject.Item2.IsBefore(DateTime.UtcNow, 5.Minutes()))
                {
                    Task.Run(async () => { cacheObject = new Tuple<MessageTemplate, DateTime>(await this.innerService.GetByIdAsync(templateIdentifier), DateTime.UtcNow); }).Forget();
                }

                return cacheObject.Item1;
            }

            MessageTemplate messageTemplate = await this.innerService.GetByIdAsync(templateIdentifier);
            this.messgeTemplateCache[templateIdentifier] = new Tuple<MessageTemplate, DateTime>(messageTemplate, DateTime.UtcNow);
            return messageTemplate;
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