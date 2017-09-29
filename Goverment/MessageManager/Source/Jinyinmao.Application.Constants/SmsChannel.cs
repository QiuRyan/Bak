using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Moe.Lib;

namespace Jinyinmao.Application.Constants
{
    /// <summary>
    ///     Enum SmsChannel
    /// </summary>
    public enum SmsChannel
    {
        /// <summary>
        ///     The default
        /// </summary>
        Default = 0,

        /// <summary>
        ///     验证码
        /// </summary>
        [Description("验证码")]
        YanZhengMa = 100001,

        /// <summary>
        ///     通知
        /// </summary>
        [Description("通知")]
        TongZhi = 100002,

        /// <summary>
        ///     营销
        /// </summary>
        [Description("营销")]
        YingXiao = 100003,

        /// <summary>
        ///     提醒
        /// </summary>
        [Description("提醒")]
        Tixing = 100004
    }

    public static class SmsChannelEx
    {
        private static readonly Dictionary<int, SmsChannel> smsChannels = new Dictionary<int, SmsChannel>
        {
            { 0, SmsChannel.Default },
            { 100001, SmsChannel.YanZhengMa },
            { 100002, SmsChannel.TongZhi },
            { 100003, SmsChannel.YingXiao },
            { 100004, SmsChannel.Tixing }
        };

        public static SmsChannel ToSmsChannel(this int code)
        {
            if (code == 0 || !smsChannels.ContainsKey(code))
            {
                return SmsChannel.TongZhi;
            }

            return smsChannels.GetOrDefault(code);
        }
    }

    internal static class SmsChannelEnumHelper
    {
        private static readonly Lazy<Dictionary<int, string>> Channels = new Lazy<Dictionary<int, string>>(
            () => Enum.GetValues(typeof(SmsChannel)).Cast<SmsChannel>().ToDictionary(value => Convert.ToInt32(value), value => value.Description()));

        internal static Dictionary<int, string> GetChannels() => Channels.Value;
    }
}