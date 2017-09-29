// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplateService.cs
// Created          : 2015-11-28  15:36
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  17:08
// ***********************************************************************
// <copyright file="MessageTemplateService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Jinyinmao.MessageManager.Domain.Entity;
using Moe.Lib;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     MessageTemplateService.
    /// </summary>
    public class MessageTemplateService : IMessageTemplateService
    {
        #region IMessageTemplateService Members

        /// <summary>
        ///     create as an asynchronous operation.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public async Task<MessageTemplate> CreateAsync(MessageTemplate messageTemplate)
        {
            messageTemplate.TemplateId = GuidUtility.NewSequentialGuid().ToGuidString();
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                if (messageTemplate.IsDefault)
                {
                    List<MessageTemplate> defaultMessageTemplates = await db.Query<MessageTemplate>().Where(t => t.IsDefault).ToListAsync();
                    if (defaultMessageTemplates?.Count > 0)
                    {
                        defaultMessageTemplates.ForEach(t => t.IsDefault = false);
                    }
                }
                db.Add(messageTemplate);

                await db.ExecuteSaveChangesAsync();
            }

            return messageTemplate;
        }

        /// <summary>
        ///     Fills the tempalete.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="messageTemplate"></param>
        /// <returns>System.String.</returns>
        public string FillTempalete(Dictionary<string, string> args, MessageTemplate messageTemplate)
        {
            return args.Aggregate(messageTemplate.TemplateContent, (current, arg) => current.Replace("{" + arg.Key + "}", arg.Value));
        }

        public async Task<MessageTemplate> GetByBizcodeAsync(string bizCode)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<MessageTemplate>().OrderByDescending(t => t.IsDefault).FirstOrDefaultAsync(t => t.BizCode == bizCode);
            }
        }

        /// <summary>
        ///     get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="templateIdentifier">The template identifier.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public async Task<MessageTemplate> GetByIdAsync(string templateIdentifier)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<MessageTemplate>().FirstOrDefaultAsync(t => t.TemplateId == templateIdentifier);
            }
        }

        /// <summary>
        ///     Gets the entity.
        /// </summary>
        /// <param name="bizCode"></param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public async Task<MessageTemplate> GetEntityAsync(string bizCode)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<MessageTemplate>().FirstOrDefaultAsync(t => t.BizCode == bizCode);
            }
        }

        /// <summary>
        ///     Gets the message templates.
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public async Task<List<MessageTemplate>> GetMessageTemplatesAsync()
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<MessageTemplate>().ToListAsync();
            }
        }

        /// <summary>
        ///     update as an asynchronous operation.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>Task&lt;MessageTemplate&gt;.</returns>
        public async Task<MessageTemplate> UpdateAsync(MessageTemplate messageTemplate)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                if (await db.ReadonlyQuery<MessageTemplate>().AnyAsync(t => t.TemplateId == messageTemplate.TemplateId))
                {
                    return null;
                }

                db.Set<MessageTemplate>().Attach(messageTemplate);
                db.Entry(messageTemplate).State = EntityState.Modified;
                await db.ExecuteSaveChangesAsync();
            }
            return messageTemplate;
        }

        #endregion IMessageTemplateService Members
    }
}