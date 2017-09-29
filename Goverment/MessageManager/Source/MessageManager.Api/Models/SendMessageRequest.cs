// ***********************************************************************
// Project          : MessageManager
// File             : SendMessageRequest.cs
// Created          : 2015-12-06  12:52
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-06  13:40
// ***********************************************************************
// <copyright file="SendMessageRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Jinyinmao.Application.Constants;
using Jinyinmao.Application.ViewModel.MessageManager;
using Moe.Lib;
using MoeLib.Diagnostics;
using Newtonsoft.Json;

namespace Jinyinmao.MessageManager.Api.Models
{
    /// <summary>
    ///     SendMessageRequest.
    /// </summary>
    public class SendMessageRequest
    {
        /// <summary>
        ///     手机号码，验证使用正则表达式：^(13|14|15|16|17|18)\d{9}$
        /// </summary>
        [Required]
        [JsonProperty("cellphone")]
        [Display(Name = "手机号")]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     短信通道
        /// </summary>
        [Required]
        [JsonProperty("channel")]
        public SmsChannel Channel { get; set; }

        /// <summary>
        ///     短信网关列表
        /// </summary>
        [JsonProperty("gateway")]
        public List<int> Gateway { get; set; }

        /// <summary>
        ///     短信内容
        /// </summary>
        [Required]
        [JsonProperty("message")]
        [StringLength(300)]
        public string Message { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        [Required]
        [JsonProperty("signature")]
        [StringLength(50)]
        public string Signature { get; set; }
    }

    internal static class SendMessageRequestEx
    {
        internal static MessageModel ToModel(this SendMessageRequest request, TraceEntry traceEntry)
        {
            return new MessageModel
            {
                Args = traceEntry.ToJson(),
                Cellphone = request.Cellphone,
                Channel = request.Channel,
                Gateway = request.Gateway,
                Message = request.Message,
                Signature = request.Signature
            };
        }
    }
}