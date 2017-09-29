// ***********************************************************************
// Project          : MessageManager
// File             : SmsManagerMessageRequest.cs
// Created          : 2015-12-09  11:29
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-09  15:41
// ***********************************************************************
// <copyright file="SmsManagerMessageRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Jinyinmao.Application.Constants;
using Moe.Lib.Web;
using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.MessageWorker
{
    /// <summary>
    ///     SmsMessageManagerRequest.
    /// </summary>
    public class SmsMessageManagerRequest
    {
        /// <summary>
        ///     参数列表，内含字符串
        /// </summary>
        [Required]
        [JsonProperty("args")]
        public List<string> Args { get; set; }

        /// <summary>
        ///     手机号，多个号码以,分隔，这里不会验证手机号的格式是否正确
        /// </summary>
        [Required]
        [JsonProperty("cellphones")]
        public string Cellphones { get; set; }

        /// <summary>
        ///     短信通道
        /// </summary>
        [Required]
        [AvailableValues(SmsChannel.YanZhengMa, SmsChannel.TongZhi, SmsChannel.YingXiao)]
        [JsonProperty("channel")]
        public SmsChannel Channel { get; set; }

        /// <summary>
        ///     短信类型
        /// </summary>
        [Required]
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        [Required]
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}