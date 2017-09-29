using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jinyinmao.Daemon.Models
{
    public class LockYemEntity : TableEntity
    {
        /// <summary>
        ///     等待分配的预约冻结信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public List<WaitForallotBuyOrderList> WaitForallotList;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockYemEntity" /> class.
        /// </summary>
        public LockYemEntity()
        {
            this.WaitForallotList = new List<WaitForallotBuyOrderList>();
        }

        /// <summary>
        ///     身份证
        /// </summary>
        /// <value>The name of the user.</value>
        public string CardNo { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        /// <value>The name of the user.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     是否全部赎回 true:全部赎回 false:非全部赎回
        /// </summary>
        /// <value>The is full redeem.</value>
        public bool IsFullRedeem { get; set; }

        /// <summary>
        ///     是否同步
        /// </summary>
        public bool IsSync { get; set; }

        /// <summary>
        ///     剩余可赎回金额
        /// </summary>
        /// <value>The jby left amount.</value>
        public long JBYLeftAmount { get; set; }

        /// <summary>
        ///     订单赎回时间
        /// </summary>
        /// <value>The order time.</value>
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     应该处理金额
        /// </summary>
        /// <value>The ought amount.</value>
        public long OughtAmount { get; set; }

        /// <summary>
        ///     资产配置系统的产品Id
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductId { get; set; }

        /// <summary>
        ///     交易系统的产品Id
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     赎回的金额
        /// </summary>
        public long RedemptionAmount { get; set; }

        /// <summary>
        ///     赎回订单号(赎回流水信息)
        /// </summary>
        /// <value>The order identifier.</value>
        public string SequenceNo { get; set; }

        /// <summary>
        ///     赎回手续费
        /// </summary>
        /// <value>The service charge.</value>
        public long ServiceCharge { get; set; }

        /// <summary>
        ///     交易系统流水标识（orderId）
        /// </summary>
        /// <value>The transaction identifier.</value>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
    }

    /// <summary>
    ///     赎回时传输等待处理的用户购买订单信息
    /// </summary>
    public class WaitForallotBuyOrderList
    {
        /// <summary>
        ///     购买订单号
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        ///     订单待处理的金额
        /// </summary>
        /// <value>The wait amount.</value>
        public long WaitAmount { get; set; }
    }
}