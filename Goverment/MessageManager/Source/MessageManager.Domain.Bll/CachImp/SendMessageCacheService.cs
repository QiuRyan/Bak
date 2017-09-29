// ***********************************************************************
// Project          : MessageManager
// File             : SendMessageCacheService.cs
// Created          : 2015-12-01  17:35
//
// Last Modified By : 张政平
// Last Modified On : 2015-12-01  17:35
// ***********************************************************************
// <copyright file="SendMessageCacheService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.MessageManager;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     SendMessageCacheService.
    /// </summary>
    public class SendMessageCacheService : ISendMessageService
    {
        /// <summary>
        ///     The inner service
        /// </summary>
        private readonly ISendMessageService innerService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendMessageCacheService" /> class.
        /// </summary>
        /// <param name="sendMessageService">The send message service.</param>
        public SendMessageCacheService(ISendMessageService sendMessageService)
        {
            this.innerService = sendMessageService;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendMessageCacheService" /> class.
        /// </summary>
        public SendMessageCacheService()
        {
            this.innerService = new SendMessageService();
        }

        #region ISendMessageService Members

        /// <summary>
        ///     Sends the message action.
        /// </summary>
        /// <param name="messageModels"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task SendMessageActionAsync(params MessageModel[] messageModels)
        {
            return this.innerService.SendMessageActionAsync(messageModels);
        }

        #endregion ISendMessageService Members
    }
}