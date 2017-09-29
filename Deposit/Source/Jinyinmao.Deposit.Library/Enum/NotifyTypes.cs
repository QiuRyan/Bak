// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : NotifyTypes.cs
// Created          : 2017-08-10  13:38
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:39
// ******************************************************************************************************
// <copyright file="NotifyTypes.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

namespace Jinyinmao.Deposit.Lib.Enum
{
    /// <summary>
    /// 回调类别
    /// </summary>
    public class NotifyTypes
    {
        /// <summary>
        ///     预约批量投资==>booknotificationqueue
        /// </summary>
        public static readonly int BatchBookInvest = 7003;

        /// <summary>
        ///     流标 ==>businessnotificationqueue
        /// </summary>
        public static readonly int BidCancelNotify = 5003;

        /// <summary>
        ///     标的信息创建回调  ==>bidnotificationqueue
        /// </summary>
        public static readonly int BidCreatedNotify = 2001;

        /// <summary>
        ///     放款回调 ==>businessnotificationqueue
        /// </summary>
        public static readonly int BidLoansNotidy = 5002;

        /// <summary>
        ///     还款回调==> businessnotificationqueue
        /// </summary>
        public static readonly int BidRepayNotify = 5004;

        /// <summary>
        ///     标的信息修改回调  ==>bidnotificationqueue
        /// </summary>
        public static readonly int BidUpdateNotify = 2002;

        /// <summary>
        ///     预约批量债权转让投资==>booknotificationqueue
        /// </summary>
        public static readonly int BookCreditCreateBatch = 7004;

        /// <summary>
        ///     预约冻结==>booknotificationqueue
        /// </summary>
        public static readonly int BookFreeze = 7001;

        /// <summary>
        ///     取消预约冻结==>booknotificationqueue
        /// </summary>
        public static readonly int BookFreezeCancel = 7002;

        /// <summary>
        ///     代偿还款回调==> businessnotificationqueue
        /// </summary>
        public static readonly int CompensationRepayNotify = 5006;

        /// <summary>
        ///     债权转让回调 ==>businessnotificationqueue
        /// </summary>
        public static readonly int CreditAssignmentCreate = 5008;

        /// <summary>
        ///     债权转让放款回调   businessnotificationqueue
        /// </summary>
        public static readonly int CreditAssignmentGrant = 5010;

        /// <summary>
        ///     企业用户开户回调==>financingusernotificationqueue
        /// </summary>
        public static readonly int EnterpriseUserCreatedAccountNotify = 1003;

        /// <summary>
        ///     企业用户修改回调 ==>financingusernotificationqueue
        /// </summary>
        public static readonly int EnterpriseUserModifyAccountNotify = 1004;

        /// <summary>
        ///     流标回调 ==>businessnotificationqueue
        /// </summary>
        public static readonly int FailedAuctionsNotify = 5003;

        /// <summary>
        ///     用户充值回调(融资) ==loanrechargenotificationqueue
        /// </summary>
        public static readonly int LoanRechargeNotify = 3002;

        /// <summary>
        ///     个人用户开户回调(融资)==>financingusernotificationqueue
        /// </summary>
        public static readonly int LoanUserCreatedAccountNotify = 1002;

        /// <summary>
        ///     用户验密提现(融资) ==>loanwithdrawnotificationqueue
        /// </summary>
        public static readonly int LoanWithdrawByPWDNotify = 4002;

        /// <summary>
        ///     用户受托支付提现 ==>loanwithdrawnotificationqueue
        /// </summary>
        public static readonly int LoanWithdrawByTrusteeNotify = 4003;

        /// <summary>
        ///     The none
        /// </summary>
        public static readonly int None = 0;

        /// <summary>
        ///     返利回调==> businessnotificationqueue
        /// </summary>
        public static readonly int RebateNotify = 5005;

        /// <summary>
        ///     用户充值回调(投资) ==>rechargenotificationqueue
        /// </summary>
        public static readonly int RechargeNotify = 3001;

        /// <summary>
        ///     还代偿款回调 ==>businessnotificationqueue
        /// </summary>
        public static readonly int RepayCompensationSectionNotify = 5007;

        /// <summary>
        ///     个人用户开户回调(投资) ==>investmentusernotificationqueue
        /// </summary>
        public static readonly int UserCreatedAccountNotify = 1001;

        /// <summary>
        ///     用户验密提现(投资) ==>withdrawnotificationqueue
        /// </summary>
        public static readonly int WithdrawNotify = 4001;

        /// <summary>
        ///     免密债权转让回调 ==>businessnotificationqueue
        /// </summary>
        public static readonly int WithoutPWDDebentureTransferNotify = 5009;

        /// <summary>
        ///     用户投资回调  ==>businessnotificationqueue
        /// </summary>
        public static int UserInvertmentNotify => 5001;
    }
}