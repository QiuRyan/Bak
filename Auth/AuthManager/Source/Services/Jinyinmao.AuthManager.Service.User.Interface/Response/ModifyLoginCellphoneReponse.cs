// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ModifyLoginCellphoneReponse.cs
// Created          : 2016-12-14  20:16
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:17
// ***********************************************************************
// <copyright file="ModifyLoginCellphoneReponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Service.User.Interface.Response
{
    /// <summary>
    ///     Class ModifyLoginCellphoneReponse.
    /// </summary>
    public class ModifyLoginCellphoneReponse
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        [JsonProperty("cellphone")]
        [RegularExpression("^(13|14|15|16|17|18)\\d{9}$")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}