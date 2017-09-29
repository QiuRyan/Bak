// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : WorkerRoleRegister.cs
// Created          : 2017-08-10  11:42
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  11:46
// ******************************************************************************************************
// <copyright file="WorkerRoleRegister.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Jinyinmao.Deposit.Config;
using Microsoft.ServiceBus.Messaging;
using Moe.Lib.Jinyinmao;
using MoeLib.Jinyinmao.Azure;

namespace Jinyinmao.Deposit
{
    public class WorkerRoleRegister
    {
        public static QueueClient CreateQueueClient(string queueName)
        {
            return QueueClient.CreateFromConnectionString(ConfigManager.ServiceBusConnectionString, queueName);
        }

        public static void Register()
        {
            App.Initialize().ConfigForAzure().UseGovernmentServerConfigManager<DepositConfig>();
        }
    }
}