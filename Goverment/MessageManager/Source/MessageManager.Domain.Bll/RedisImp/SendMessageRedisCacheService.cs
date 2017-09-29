// ***********************************************************************
// Project          : MessageManager
// File             : SendMessageRedisCacheService.cs
// Created          : 2015-12-01  17:39
//
// Last Modified By : 张政平
// Last Modified On : 2015-12-01  17:39
// ***********************************************************************
// <copyright file="SendMessageRedisCacheService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.MessageManager;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     SendMessageRedisCacheService.
    /// </summary>
    public class SendMessageRedisCacheService : ISendMessageService
    {
        /// <summary>
        ///     The inner service
        /// </summary>
        private readonly ISendMessageService innerService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendMessageRedisCacheService" /> class.
        /// </summary>
        /// <param name="innerService">The inner service.</param>
        public SendMessageRedisCacheService(ISendMessageService innerService)
        {
            this.innerService = innerService;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendMessageRedisCacheService" /> class.
        /// </summary>
        public SendMessageRedisCacheService()
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