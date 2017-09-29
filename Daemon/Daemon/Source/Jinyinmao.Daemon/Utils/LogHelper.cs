// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : LogHelper.cs
// Created          : 2015-09-04  17:14
//
// Last Modified By :
// Last Modified On : 2015-09-04  17:21
// ***********************************************************************
// <copyright file="LogHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text;
using Exceptionless;
using Moe.Lib;
using NLog;

namespace Jinyinmao.Daemon.Utils
{
    /// <summary>
    ///     LogHelper.
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        ///     The errorlogger
        /// </summary>
        private static readonly Logger Errorlogger;

        /// <summary>
        ///     Initializes static members of the <see cref="LogHelper" /> class.
        /// </summary>
        static LogHelper()
        {
            Errorlogger = LogManager.GetLogger("ErrorLogger");
        }

        /// <summary>
        ///     Errors the specified message.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message">The message.</param>
        /// <param name="jsonString">The json string.</param>
        /// <param name="tag">The tag.</param>
        public static void Error(Exception ex, string message, string jsonString, string tag)
        {
            ExceptionlessClient.Default?.CreateException(ex).AddObject(message, "message").AddObject(jsonString).AddTags(tag).Submit();
        }

        /// <summary>
        ///     Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="jsonString">The json string.</param>
        /// <param name="tag">The tag.</param>
        public static void Log(string message, string jsonString, string tag)
        {
            ExceptionlessClient.Default?.CreateLog(message).AddObject(jsonString).AddTags(tag).Submit();
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void LogError(string message, Exception exception)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("   ");
            messageBuilder.Append(message);
            messageBuilder.Append("   ");
            messageBuilder.Append(exception.GetExceptionString());

            Errorlogger.Error(messageBuilder);
        }
    }
}