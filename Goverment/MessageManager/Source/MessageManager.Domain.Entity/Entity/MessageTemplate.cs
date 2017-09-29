// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplate.cs
// Created          : 2015-11-28  15:21
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  15:29
// ***********************************************************************
// <copyright file="MessageTemplate.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Jinyinmao.MessageManager.Domain.Entity
{
    /// <summary>
    ///     MessageTemplate.
    /// </summary>
    public class MessageTemplate
    {
        /// <summary>
        ///     Gets or sets the biz code.
        /// </summary>
        /// <value>The biz code.</value>
        public string BizCode { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value><c>true</c> if this instance is default; otherwise, <c>false</c>.</value>
        public bool IsDefault { get; set; }

        /// <summary>
        ///     Gets or sets the is valid.
        /// </summary>
        /// <value>The is valid.</value>
        public int IsValid { get; set; }

        /// <summary>
        ///     Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        public string Remark { get; set; }

        /// <summary>
        ///     Gets or sets the content of the template.
        /// </summary>
        /// <value>The content of the template.</value>
        public string TemplateContent { get; set; }

        /// <summary>
        ///     Gets or sets the template identifier.
        /// </summary>
        /// <value>The template identifier.</value>
        public string TemplateId { get; set; }

        /// <summary>
        ///     Gets or sets the template title.
        /// </summary>
        /// <value>The template title.</value>
        public string TemplateTitle { get; set; }
    }
}