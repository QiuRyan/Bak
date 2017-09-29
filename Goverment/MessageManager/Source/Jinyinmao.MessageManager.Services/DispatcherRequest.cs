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
using Jinyinmao.Application.Constants;

namespace Jinyinmao.MessageManager.Services
{
    public class DispatcherRequest
    {
        public string BizCode { get; set; }

        public string ChannelCode { get; set; }

        public List<int> Gateway { get; set; }

        public int MessageType
        {
            get { return (int)SmsChannel.YanZhengMa; }
        }

        public string Priority { get; set; }

        public string SendRule { get; set; }

        public Dictionary<string, string> TemplateParams { get; set; }

        public List<UserInfo> UserInfoList { get; set; }
    }

    public class UserInfo
    {
        public string Email { get; set; }

        public string PhoneNum { get; set; }

        public string UAppId { get; set; }

        public string UId { get; set; }

        public string WeChatNum { get; set; }
    }
}