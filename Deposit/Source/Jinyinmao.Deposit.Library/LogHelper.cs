using System;
using Exceptionless;
using Jinyinmao.Deposit.Config;

namespace Jinyinmao.Deposit.Lib
{
    public static class LogHelper
    {
        public static void Error(Exception ex, string message, string data, string tag)
        {
            ConfigManager.ExceptionlessDefaultClient?.CreateException(ex).AddObject(message, "message").AddObject(data).AddTags(tag).Submit();
        }

        public static void Info(string message, string data, string tag)
        {
            ConfigManager.ExceptionlessDefaultClient?.CreateLog(message).AddObject(data).AddTags(tag).Submit();
        }

        /// <summary>
        ///     写入日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="tag">日志tag</param>
        /// <param name="data">请求或输出数据</param>
        public static void Info(string message, string tag, params string[] data)
        {
            ConfigManager.ExceptionlessDefaultClient?.CreateLog(message).AddObject(data).AddTags(tag).Submit();
        }
    }
}