// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : DepositConfig.cs Created : 2017-08-10 10:47
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-10 11:20 ******************************************************************************************************
// <copyright file="DepositConfig.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Collections.Generic;
using MoeLib.Jinyinmao.Configs;

namespace Jinyinmao.Deposit.Config
{
    public class DepositConfig : IConfig
    {
        /// <summary>
        ///     资产服务SF基地址
        /// </summary>
        /// <value>The asset servicefabric base URL.</value>
        public string AssetServicefabricBaseUrl
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the bid notification queue.
        /// </summary>
        /// <value>The bid notification queue.</value>
        public string BidNotificationQueue
        {
            get; set;
        }

        /// <summary>
        ///     标的还款队列
        /// </summary>
        /// <value>The bid repay queue.</value>
        public string BidRepayQueue
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the biz database connection string.
        /// </summary>
        /// <value>The biz database connection string.</value>
        public string BizDBConnectionString
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the biz redis connectiong string.
        /// </summary>
        /// <value>The biz redis connectiong string.</value>
        public string BizRedisConnectiongString
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the book notification queue.
        /// </summary>
        /// <value>The book notification queue.</value>
        public string BookNotificationQueue
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the business queue.
        /// </summary>
        /// <value>The business queue.</value>
        public string BusinessQueue
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the finish create account queue.
        /// </summary>
        /// <value>The finish create account queue.</value>
        public string BusinessRole
        {
            get; set;
        }
        public string CouponDBConnectionString
        {
            get; set;
        }
        /// <summary>
        ///     卡券系统
        /// </summary>
        /// <value>The coupon service role.</value>
        public string CouponServiceRole
        {
            get; set;
        }
        /// <summary>
        ///     卡券使用队列
        /// </summary>
        /// <value>The coupon use queue.</value>
        public string CouponUseQueue
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the data connection string.
        /// </summary>
        /// <value>The data connection string.</value>
        public string DataConnectionString
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the exceptionless API key.
        /// </summary>
        /// <value>The exceptionless API key.</value>
        public string ExceptionlessApiKey
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the exceptionless server URL.
        /// </summary>
        /// <value>The exceptionless server URL.</value>
        public string ExceptionlessServerUrl
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the finish create account queue.
        /// </summary>
        /// <value>The finish create account queue.</value>
        public string FinishCreateAccountQueue
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the book notification queue.
        /// </summary>
        /// <value>The book notification queue.</value>
        public string JYMCouponServiceRole
        {
            get; set;
        }
        /// <summary>
        ///     Gets or sets the biz redis connectiong string.
        /// </summary>
        /// <value>The biz redis connectiong string.</value>
        public string JYMMarketingIdentifier
        {
            get; set;
        }
        /// <summary>
        ///     队列请求重试次数
        /// </summary>
        /// <value>The maximum retry count.</value>
        public int MaxRetryCount
        {
            get; set;
        }

        /// <summary>
        ///     短信队列
        /// </summary>
        /// <value>The message queue.</value>
        public string MessageQueue
        {
            get; set;
        }

        /// <summary>
        ///     短信系统
        /// </summary>
        /// <value>The coupon service role.</value>
        public string MessageServiceRole
        {
            get; set;
        }

        /// <summary>
        ///     返利流水队列名
        /// </summary>
        /// <value>The rebate transaction queue.</value>
        public string RebateTransactionQueue
        {
            get; set;
        }

        /// <summary>
        ///     充值回调队列
        /// </summary>
        public string RechargeNotificationQueue
        {
            get; set;
        }

        /// <summary>
        ///     定期理财购买
        /// </summary>
        /// <value>The regular invest notification queue.</value>
        public string RegularInvestNotificationQueue
        {
            get; set;
        }

        /// <summary>
        ///     ServiceBus连接串
        /// </summary>
        /// <value>The service bus connection string.</value>
        public string ServiceBusConnectionString
        {
            get; set;
        }

        public int SubmissionBatchSize
        {
            get; set;
        }

        /// <summary>
        ///     交易系统角色
        /// </summary>
        public string TirisfalServiceRole
        {
            get; set;
        }

        public string WithdrawNotificationQueue
        {
            get; set;
        }

        #region IConfig Members

        /// <summary>
        ///     Gets the ip whitelists.
        /// </summary>
        /// <value>The ip whitelists.</value>
        public List<string> IPWhitelists
        {
            get; set;
        }

        /// <summary>
        ///     Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public Dictionary<string, string> Resources
        {
            get; set;
        }

        #endregion IConfig Members
    }
}