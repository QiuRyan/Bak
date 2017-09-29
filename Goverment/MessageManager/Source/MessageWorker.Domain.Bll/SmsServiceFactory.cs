using System;
using System.Collections.Generic;
using System.Linq;
using Jinyinmao.Application.Constants;
using Jinyinmao.MessageWorker.Domain.Bll.Impl;
using Jinyinmao.Sms.Api.Services;

namespace Jinyinmao.MessageWorker.Domain.Bll
{
    /// <summary>
    ///     Class SmsServiceFactory.
    /// </summary>
    public static class SmsServiceFactory
    {
        /// <summary>
        ///     The default SMS gateway dic
        /// </summary>
        public static readonly Dictionary<SmsGateway, bool> defaultSmsGatewayDic = new Dictionary<SmsGateway, bool>
        {
            { SmsGateway.ChuangLan, false },
            { SmsGateway.ZhuTong, true }
        };

        /// <summary>
        ///     Gets the default SMS gateways.
        /// </summary>
        /// <value>The default SMS gateways.</value>
        private static List<SmsGateway> DefaultSmsGateways
        {
            get { return defaultSmsGatewayDic.Where(t => t.Value).Select(t => t.Key).ToList(); }
        }

        /// <summary>
        ///     Gets all SMS service by channel.
        /// </summary>
        /// <param name="smsChannel">The SMS channel.</param>
        /// <param name="smsGateways">The SMS gateways.</param>
        /// <returns>IEnumerable&lt;ISmsService&gt;.</returns>
        public static IEnumerable<ISmsService> GetAllSmsServiceByChannel(SmsChannel smsChannel, List<int> smsGateways = null)
        {
            if (smsGateways == null)
            {
                smsGateways = new List<int>();
                if (smsChannel == SmsChannel.YanZhengMa || smsChannel == SmsChannel.TongZhi)
                {
                    smsGateways.Add((int)SmsGateway.ChuangLan);
                }
                else
                {
                    smsGateways.Add((int)SmsGateway.ZhuTong);
                }
            }
            return smsGateways.Select(g => g.CreateSmsService(smsChannel)).ToList();
        }
    }

    /// <summary>
    ///     SmsGatewayEx.
    /// </summary>
    internal static class SmsGatewayEx
    {
        /// <summary>
        ///     Creates the SMS service.
        /// </summary>
        /// <param name="smsGateway">The SMS gateway.</param>
        /// <param name="smsChannel">The SMS channel.</param>
        /// <returns>ISmsService.</returns>
        /// <exception cref="System.ArgumentException">Invalid argument value.</exception>
        internal static ISmsService CreateSmsService(this int smsGateway, SmsChannel smsChannel)
        {
            switch (smsGateway)
            {
                case (int)SmsGateway.ChuangLan:
                    return new ClSmsService(smsChannel);

                case (int)SmsGateway.ZhuTong:
                    return new ZtSmsService(smsChannel);
            }

            throw new ArgumentException("Invalid argument value.", nameof(smsGateway));
        }
    }
}