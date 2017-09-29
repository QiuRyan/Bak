// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ModifyLoginCellphoneRequest.cs
// Created          : 2016-12-14  20:16
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:17
// ***********************************************************************
// <copyright file="ModifyLoginCellphoneRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.User.Interface.Request
{
    public class ModifyLoginCellphoneRequest
    {
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}