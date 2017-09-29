// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : NinjectConfig.cs
// Created          : 2016-03-22  6:03 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-05-04  5:38 PM
// ***********************************************************************
// <copyright file="NinjectConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Api;
using Jinyinmao.AuthManager.Service.Coupon;
using Jinyinmao.AuthManager.Service.Coupon.Interface;
using Jinyinmao.AuthManager.Service.Misc;
using Jinyinmao.AuthManager.Service.Misc.Interface;
using Jinyinmao.AuthManager.Service.User;
using Jinyinmao.AuthManager.Service.User.Interface;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using System;
using System.Web;
using System.Web.Http;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectConfig), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectConfig), "Stop")]

namespace Jinyinmao.AuthManager.Api
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
        public static void RegisterDependencyResolver(HttpConfiguration config) => config.DependencyResolver = new NinjectDependencyResolver(Kernel);

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
            Kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            Kernel.Bind<IVeriCodeService>().To<VeriCodeService>().InSingletonScope();
            Kernel.Bind<IMessageRoleService>().To<MessageRoleService>().InSingletonScope();
        }
    }
}