// ***********************************************************************
// Project          : MessageManager
// File             : ISendMessageService.cs
// Created          : 2015-12-01  13:34
//
// Last Modified By : 张政平
// Last Modified On : 2015-12-01  13:34
// ***********************************************************************
// <copyright file="ISendMessageService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.MessageManager;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     Interface ISendMessageService
    /// </summary>
    public interface ISendMessageService
    {
        /// <summary>
        ///     Sends the message action.
        /// </summary>
        /// <param name="messageModels"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task SendMessageActionAsync(params MessageModel[] messageModels);
    }
}