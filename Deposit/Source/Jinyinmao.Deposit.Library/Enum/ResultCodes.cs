// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : ResultCodes.cs
// Created          : 2017-08-10  13:25
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:25
// ******************************************************************************************************
// <copyright file="ResultCodes.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

namespace Jinyinmao.Deposit.Lib.Enum
{
    public static class Constants
    {
        public const string RESPSUBCODE = "000000";

        /// <summary>
        /// The coupondb
        /// </summary>
        public const int COUPONDB = 0;

        /// <summary>
        /// The bizdb
        /// </summary>
        public const int BIZDB = 1;
    }

    public class ResultCode
    {
        public static int Failed
        {
            get
            {
                return 2000;
            }
        }

        public static int Other
        {
            get
            {
                return 3000;
            }
        }

        public static int Successs
        {
            get
            {
                return 1000;
            }
        }
    }
}