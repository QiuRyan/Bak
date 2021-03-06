﻿// ***********************************************************************
// Project          : MessageManager
// File             : NinjectConfig.cs
// Created          : 2015-11-28  15:59
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-30  9:30
// ***********************************************************************
// <copyright file="NinjectConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Web;
using System.Web.Http;
using Jinyinmao.MessageManager.Services;
using Jinyinmao.ValidateCode.Api.App_Start;
using Jinyinmao.ValidateCode.Api.Config;
using Jinyinmao.ValidateCode.Domain.Bll;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using ValidateCode.Domain.Bll;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectConfig), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectConfig), "Stop")]

namespace Jinyinmao.ValidateCode.Api.App_Start
{
    /// <summary>
    ///     Ninject Configuration
    /// </summary>
    public static class NinjectConfig
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();
        private static readonly StandardKernel Kernel = new StandardKernel();

        /// <summary>
        ///     RegisterDependencyResolver to HttpConfiguration
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void RegisterDependencyResolver(HttpConfiguration config)
            => config.DependencyResolver = new NinjectDependencyResolver(Kernel);

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop() => Bootstrapper.ShutDown();

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices();
            return Kernel;
        }

        /// <summary>
        ///     Registers the services.
        /// </summary>
        private static void RegisterServices()
        {
#if DEBUG
            Kernel.Bind<IValidateCodeService>()
                .ToConstant(new ValidateCodeCacheService(new ValidateCodeService(
                    ConfigsManager.DefaultMaxSendTimes,
                    ConfigsManager.MaxSendTimeForQuickLogin,
                    ConfigsManager.VeriCodeExpiryMinites,
                    new MockSmsService())))
                .InSingletonScope();
#else
            Kernel.Bind<IValidateCodeService>()
                .ToConstant(new ValidateCodeCacheService(new ValidateCodeService(
                    ConfigsManager.DefaultMaxSendTimes,
                    ConfigsManager.MaxSendTimeForQuickLogin,
                    ConfigsManager.VeriCodeExpiryMinites,
                    new SmsService(ConfigsManager.InnerMessageSendUrl, ConfigsManager.MessageManagerServiceRoleName))))
                .InSingletonScope();
#endif
        }
    }
}