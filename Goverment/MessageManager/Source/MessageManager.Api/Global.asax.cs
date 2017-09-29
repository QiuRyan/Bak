// ***********************************************************************
// Project          : MessageManager
// File             : Global.asax.cs
// Created          : 2015-12-07  14:27
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-08  15:02
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web;
using System.Web.Http;
using Moe.Lib.Jinyinmao;

namespace Jinyinmao.MessageManager.Api
{
    /// <summary>
    ///     WebApiApplication.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        ///     Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            App.Initialize().Config();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}