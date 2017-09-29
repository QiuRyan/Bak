// ***********************************************************************
// Project          : MessageManager
// File             : DispatcherRequest.cs
// Created          : 2015-12-09  11:30
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-09  17:00
// ***********************************************************************
// <copyright file="DispatcherRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.MessageManager
{
    /// <summary>
    ///     消息发送请求
    /// </summary>
    public class DispatcherRequest
    {
        /// <summary>
        ///     业务编码
        /// </summary>
        [Required, JsonProperty("bizCode"), StringLength(10, MinimumLength = 2)]
        public string BizCode { get; set; }

        /// <summary>
        ///     频道编码【请求源】
        /// </summary>
        [Required, JsonProperty("channelCode"), StringLength(10, MinimumLength = 2)]
        public string ChannelCode { get; set; }

        /// <summary>
        ///     短信网关列表(助通,创蓝)
        /// </summary>
        [Required, JsonProperty("gateway")]
        public List<int> Gateway { get; set; }

        /// <summary>
        ///     短信类型:验证码,营销
        /// </summary>
        [Required, JsonProperty("messageType")]
        public int MessageType { get; set; }

        /// <summary>
        ///     发布优先级
        /// </summary>
        [Required, JsonProperty("priority")]
        public string Priority { get; set; }

        /// <summary>
        ///     发布规则
        /// </summary>
        [Required, JsonProperty("sendRule")]
        public string SendRule { get; set; }

        /// <summary>
        ///     信息模板相对应的参数列表
        /// </summary>
        [Required, JsonProperty("templateParams")]
        public Dictionary<string, string> TemplateParams { get; set; }

        /// <summary>
        ///     用户数据 (用List便于群发)
        /// </summary>
        [Required, JsonProperty("userInfoList")]
        public List<UserInfo> UserInfoList { get; set; }
    }

    /// <summary>
    ///     用户信息(如果不填写手机号、微信号、AppId等信息，系统就会更具UId获取默认信息)
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        ///     邮箱
        /// </summary>
        [Required]
        [JsonProperty("email")]
        [StringLength(30)]
        public string Email { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        [JsonProperty("phoneNum")]
        [StringLength(30)]
        public string PhoneNum { get; set; }

        /// <summary>
        ///     终端Id
        /// </summary>
        [Required]
        [JsonProperty("uAppId")]
        [StringLength(20, MinimumLength = 2)]
        public string UAppId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        [Required]
        [JsonProperty("uId")]
        [StringLength(50)]
        public string UId { get; set; }

        /// <summary>
        ///     微信号
        /// </summary>
        [Required]
        [JsonProperty("weChatNum")]
        [StringLength(30)]
        public string WeChatNum { get; set; }
    }
}