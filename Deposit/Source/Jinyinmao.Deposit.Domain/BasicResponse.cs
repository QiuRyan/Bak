// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BasicResponse.cs
// Created          : 2017-08-10  13:38
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:38
// ******************************************************************************************************
// <copyright file="BasicResponse.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

namespace Jinyinmao.Deposit.Domain
{
    public class BaseResponse
    {
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
    }

    public class BasicResponse<T> : BaseResponse
    {
        /// <summary>
        ///     处理数据
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        ///     结果代码  1000 成功  2000 失败 3000其他
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }
    }
}