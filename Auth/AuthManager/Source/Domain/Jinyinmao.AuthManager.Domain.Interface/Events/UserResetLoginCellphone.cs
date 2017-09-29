// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserResetLoginCellphone.cs
// Created          : 2016-08-16  11:23 AM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-16  2:47 PM
// ***********************************************************************
// <copyright file="UserResetLoginCellphone.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Events;
using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Domain.Interface.Events
{
    public class UserResetLoginCellphone : Event, IEvent
    {
        #region IEvent Members

        [JsonIgnore]
        public string EventName
        {
            get { return "jym-user-reset-login-cellphone"; }
        }

        #endregion IEvent Members
    }
}