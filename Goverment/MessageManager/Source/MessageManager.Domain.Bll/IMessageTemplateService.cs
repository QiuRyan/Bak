// ***********************************************************************
// Project          : Jinyinmao.MessageManager
// File             : IMessageTemplateService.cs
// Created          : 2015-12-13  20:08
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-13  21:57
// ***********************************************************************
// <copyright file="IMessageTemplateService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.MessageManager.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     Interface IMessageTemplateService
    /// </summary>
    public interface IMessageTemplateService
    {
        /// <summary>
        ///     Creates the asynchronous.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        Task<MessageTemplate> CreateAsync(MessageTemplate messageTemplate);

        /// <summary>
        ///     Fills the tempalete.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="messageTemplate"></param>
        /// <returns>System.String.</returns>
        string FillTempalete(Dictionary<string, string> args, MessageTemplate messageTemplate);

        /// <summary>
        /// </summary>
        /// <param name="bizCode"></param>
        /// <returns></returns>
        Task<MessageTemplate> GetByBizcodeAsync(string bizCode);

        /// <summary>
        ///     Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="templateIdentifier">The template identifier.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        Task<MessageTemplate> GetByIdAsync(string templateIdentifier);

        /// <summary>
        ///     Gets the entity.
        /// </summary>
        /// <param name="bizCode"></param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        Task<MessageTemplate> GetEntityAsync(string bizCode);

        /// <summary>
        ///     Gets the message templates.
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        Task<List<MessageTemplate>> GetMessageTemplatesAsync();

        /// <summary>
        ///     Updates the asynchronous.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        Task<MessageTemplate> UpdateAsync(MessageTemplate messageTemplate);
    }
}