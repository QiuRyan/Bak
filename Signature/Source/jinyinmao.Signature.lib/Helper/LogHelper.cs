using System;
using Exceptionless;

namespace jinyinmao.Signature.lib.Helper
{
    /// <summary>
    ///     Class ExceptionLogger.
    /// </summary>
    public static class LogHelper
    {
        private static readonly Lazy<ExceptionlessClient> exceptionlessClient = new Lazy<ExceptionlessClient>(() => ConfigManager.JymLogger);

        private static ExceptionlessClient ExceptionClient => exceptionlessClient.Value;

        /// <summary>
        ///     Writes the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="obj">The object.</param>
        public static void WriteError(string message, string tag, object obj = null)
        {
            ExceptionClient.CreateLog($"{DateTime.Now:yyyyMMdd HH:mm:ss}:{message}").AddObject(obj).AddTags(tag).Submit();
        }

        /// <summary>
        ///     Writes the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="obj">The object.</param>
        public static void WriteException(Exception ex, string tag, object obj = null)
        {
            ExceptionClient.CreateException(ex).AddObject(obj).AddTags(tag).Submit();
        }

        /// <summary>
        ///     Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="obj">The object.</param>
        public static void WriteLog(string message, string tag, object obj = null)
        {
            Console.WriteLine(message);
            ExceptionClient.CreateLog($"{DateTime.Now:yyyyMMdd HH:mm:ss}:{message}").AddObject(obj).AddTags(tag).Submit();
        }
    }
}