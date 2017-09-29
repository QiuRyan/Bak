// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BasicResult.cs
// Created          : 2017-08-10  13:21
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:54
// ******************************************************************************************************
// <copyright file="BasicResult.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

namespace Jinyinmao.Deposit.Domain
{
    public class BasicResult<T>
    {
        /// <summary>
        ///     处理数据
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        public bool IsSendDeadLetter { get; set; }

        /// <summary>
        ///     处理结果说明
        /// </summary>
        /// <value>The remark.</value>
        public string Remark { get; set; }

        /// <summary>
        ///     处理结果
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     结果代码  1000 成功  2000 失败 3000其他
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        public static BasicResult<T> Failed(T t, string message, bool isSendDeadletter = false)
        {
            return new BasicResult<T>
            {
                Data = t,
                Remark = message,
                Result = false,
                ResultCode = Lib.Enum.ResultCode.Failed,
                IsSendDeadLetter = isSendDeadletter
            };
        }

        public static BasicResult<T> Successed(T t, string message = "成功")
        {
            return new BasicResult<T>
            {
                Data = t,
                Remark = message,
                Result = true,
                ResultCode = Lib.Enum.ResultCode.Successs
            };
        }
    }
}