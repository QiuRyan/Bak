// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : NotifyBussinessResultResponse.cs Created : 2017-08-10 15:18
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-10 15:18 ******************************************************************************************************
// <copyright file="NotifyBussinessResultResponse.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     放款通知资产输出
    /// </summary>
    public class NotifyBussinessResultResponse
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [logic states].
        /// </summary>
        /// <value><c>true</c> if [logic states]; otherwise, <c>false</c>.</value>
        [JsonProperty("logicstates")]
        public bool LogicStates
        {
            get; set;
        }

        /// <summary>
        ///     产品标识 如果这个字段是100000030 表示是余额猫
        /// </summary>
        /// <value>The product identifier.</value>
        [JsonProperty("productCategoryCode")]
        public string ProductCategoryCode
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        [JsonProperty("productid")]
        public string ProductId
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        [JsonProperty("remark")]
        public string Remark
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [JsonProperty("result")]
        public bool Result
        {
            get; set;
        }
    }
}