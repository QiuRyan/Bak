// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplateResponse.cs
// Created          : 2015-11-28  16:01
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  16:03
// ***********************************************************************
// <copyright file="MessageTemplateResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Jinyinmao.MessageManager.Domain.Entity;
using Newtonsoft.Json;

namespace Jinyinmao.MessageManager.Api.Models
{
    /// <summary>
    /// MessageTemplateResponse.
    /// </summary>
    public class MessageTemplateResponse
    {
        /// <summary>
        /// Gets or sets the biz code.
        /// </summary>
        /// <value>The biz code.</value>
        [Required]
        [JsonProperty("bizCode")]
        public string BizCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value><c>true</c> if this instance is default; otherwise, <c>false</c>.</value>
        [Required]
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the is valid.
        /// </summary>
        /// <value>The is valid.</value>
        [Required]
        [JsonProperty("IsValid")]
        public int IsValid { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        [Required]
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets the content of the template.
        /// </summary>
        /// <value>The content of the template.</value>
        [Required]
        [JsonProperty("templateContent")]
        public string TemplateContent { get; set; }

        /// <summary>
        /// Gets or sets the template identifier.
        /// </summary>
        /// <value>The template identifier.</value>
        [Required]
        [JsonProperty("templateIdentifier")]
        public string TemplateIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the template title.
        /// </summary>
        /// <value>The template title.</value>
        [Required]
        [JsonProperty("templateTitle")]
        public string TemplateTitle { get; set; }
    }

    /// <summary>
    /// MessageTemplateEx.
    /// </summary>
    internal static partial class MessageTemplateEx
    {
        /// <summary>
        /// To the response.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <returns>MessageTemplateResponse.</returns>
        internal static MessageTemplateResponse ToResponse(this MessageTemplate messageTemplate)
        {
            return new MessageTemplateResponse
            {
                BizCode = messageTemplate.BizCode,
                IsDefault = messageTemplate.IsDefault,
                Remark = messageTemplate.Remark ?? "",
                TemplateContent = messageTemplate.TemplateContent,
                TemplateIdentifier = messageTemplate.TemplateId,
                TemplateTitle = messageTemplate.TemplateTitle,
                IsValid = messageTemplate.IsValid
            };
        }
    }
}