// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : ResultCodeExtrends.cs
// Created          : 2017-08-10  13:45
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:45
// ******************************************************************************************************
// <copyright file="ResultCodeExtrends.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

namespace Jinyinmao.Deposit.Lib
{
    public static class ResultCodeExtrends
    {
        public static int GetResultCode(this string result)
        {
            switch (result)
            {
                case "S":
                    return 1; //success
                case "F":
                    return -1; //failed
                case "AS":
                case "I":
                    return 0; //accepting
                default:
                    return -2;
            }
        }
    }
}