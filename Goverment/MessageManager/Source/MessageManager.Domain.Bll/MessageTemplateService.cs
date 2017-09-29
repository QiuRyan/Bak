// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplateService.cs
// Created          : --
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-01  11:56
// ***********************************************************************
// <copyright file="MessageTemplateService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using MessageManager.Domain.Entity;
using Moe.Lib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MessageManager.Domain.Bll
{
    public partial class MessageTemplateService : IMessageTemplateService
    {
        #region IMessageTemplateService Members

        public async Task<MessageTemplate> CreateAsync(MessageTemplate messageTemplate)
        {
            messageTemplate.TemplateId = Guid.NewGuid().ToGuidString();
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                if (messageTemplate.IsDefault)
                {
                    List<MessageTemplate> defaultMessageTemplates = await db.Query<MessageTemplate>().Where(t => t.IsDefault).ToListAsync();
                    if (defaultMessageTemplates.Count > 0)
                    {
                        defaultMessageTemplates.ForEach(t => t.IsDefault = false);
                    }
                }
                db.Add(messageTemplate);

                await db.ExecuteSaveChangesAsync();
            }

            return messageTemplate;
        }

        public async Task<MessageTemplate> GetByIdAsync(string templateIdentifier)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<MessageTemplate>().FirstOrDefaultAsync(t => t.TemplateId == templateIdentifier);
            }
        }

        public async Task<List<MessageTemplate>> GetMessageTemplates()
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                return await db.ReadonlyQuery<MessageTemplate>().ToListAsync();
            }
        }

        public async Task<MessageTemplate> UpdateAsync(MessageTemplate messageTemplate)
        {
            using (MessageManagerDbContext db = new MessageManagerDbContext())
            {
                db.Set<MessageTemplate>().Attach(messageTemplate);
                db.Entry(messageTemplate).State = EntityState.Modified;
                await db.ExecuteSaveChangesAsync();
            }
            return messageTemplate;
        }

        #endregion IMessageTemplateService Members
    }
}