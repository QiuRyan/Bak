// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : Program.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-28  10:54
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries;
using Moe.Lib.Jinyinmao;
using MoeLib.Jinyinmao.Azure;
using MoeLib.Jinyinmao.Web.Diagnostics;
using Orleans.Runtime;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Jinyinmao.AuthManager.Silos.LocalHost
{
    internal class Program
    {
        private static OrleansHostWrapper hostWrapper;

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private static void InitSilo(string[] args)
        {
            App.Initialize().ConfigForAzure().UseGovernmentServerConfigManager<AuthSiloConfig>();
            string dataConnectionString = App.Configurations.GetConfig<AuthSiloConfig>().DataConnectionString;

            hostWrapper = new OrleansHostWrapper();

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        private static void Main(string[] args)
        {
            try
            {
                TraceLogger.LogConsumers.Add(new JinyinmaoSiloTraceWriter());
                // The Orleans silo environment is initialized in its own app domain in order to more
                // closely emulate the distributed situation, when the client and the server cannot
                // pass data via shared memory.
                AppDomain hostDomain = AppDomain.CreateDomain("Jinyinmao.AuthManager.Domain.Service", null, new AppDomainSetup
                {
                    AppDomainInitializer = InitSilo,
                    AppDomainInitializerArguments = args
                });

                string command;

                do
                {
                    command = (Console.ReadLine() ?? "").ToUpperInvariant();
                } while (command != "SHUTDOWN");

                hostDomain.DoCallBack(ShutdownSilo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }
    }
}