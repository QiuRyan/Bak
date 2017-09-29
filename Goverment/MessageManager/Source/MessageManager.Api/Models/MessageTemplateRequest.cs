// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplateRequest.cs
// Created          : 2015-11-28  16:15
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  16:21
// ***********************************************************************
// <copyright file="MessageTemplateRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Jinyinmao.MessageManager.Domain.Entity;
using Newtonsoft.Json;

namespace Jinyinmao.MessageManager.Api.Models
{
    /// <summary>
    ///     MessageTemplateRequest.
    /// </summary>
    public class MessageTemplateRequest
    {
        /// <summary>
        ///     业务编码
        /// </summary>
        [Required]
        [JsonProperty("bizCode")]
        [StringLength(20)]
        public string BizCode { get; set; }

        /// <summary>
        ///     是否默认
        /// </summary>
        [Required]
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        /// <summary>
        ///     是否有效
        /// </summary>
        [Required]
        [JsonProperty("IsValid")]
        public int IsValid { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        /// <value>The remark.</value>
        [JsonProperty("remark")]
        [StringLength(300)]
        public string Remark { get; set; }

        /// <summary>
        ///     模板内容
        /// </summary>
        [Required]
        [JsonProperty("templateContent")]
        [StringLength(500)]
        public string TemplateContent { get; set; }

        /// <summary>
        ///     模板Id
        /// </summary>
        [Required]
        [JsonProperty("templateId")]
        [StringLength(32)]
        public string TemplateId { get; set; }

        /// <summary>
        ///     模板标题
        /// </summary>
        [Required]
        [JsonProperty("templateTitle")]
        [StringLength(30)]
        public string TemplateTitle { get; set; }
    }

    /// <summary>
    ///     MessageTemplateEx.
    /// </summary>
    internal static partial class MessageTemplateEx
    {
        /// <summary>
        ///     To the entity.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>MessageTemplate.</returns>
        internal static MessageTemplate ToEntity(this MessageTemplateRequest request)
        {
            return new MessageTemplate
            {
                TemplateId = request.TemplateId,
                BizCode = request.BizCode,
                IsDefault = request.IsDefault,
                IsValid = request.IsValid,
                Remark = request.Remark,
                TemplateContent = request.TemplateContent,
                TemplateTitle = request.TemplateTitle
            };
        }
    }
}