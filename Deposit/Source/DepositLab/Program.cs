// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : Program.cs Created : 2017-08-10 18:39
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-19 13:26 ******************************************************************************************************
// <copyright file="Program.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Threading.Tasks;
using System.Timers;
using Jinyinmao.Deposit;
using Microsoft.ServiceBus.Messaging;

namespace DepositLab
{
    internal class Program
    {
        private static readonly WorkerRole worker = new WorkerRole();

        public static void Run()
        {
            Console.WriteLine("正在运行...");

            worker.OnStart();

            worker.Run();
        }

        private static async Task Deal(BrokeredMessage message)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            await message.CompleteAsync();
            Console.WriteLine($"{message.MessageId.ToUpper()}  Complete");
        }

        private static void Main(string[] args)
        {
            //App.Initialize().ConfigForAzure().UseGovernmentServerConfigManager<DepositConfig>();
            //
            //QueueClient testClient = WorkerRoleRegister.CreateQueueClient("jym-test");

            //Task.Factory.StartNew(async () =>
            //{
            //    while (true)
            //    {
            //        IEnumerable<BrokeredMessage> list = await testClient.ReceiveBatchAsync(3);

            //        Console.WriteLine($"List Count: {list.Count()}");

            //        list.ForEach(message =>
            //        {
            //            Console.WriteLine($"{message.MessageId.ToUpper()} Beging");
            //            Task.Factory.StartNew(async () => await Deal(message));
            //        });

            //        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)}");

            //        Thread.Sleep(TimeSpan.FromSeconds(5));
            //    }
            //});

            Run();

            Console.ReadKey();
        }

        private static void Time_Elapsed(object sender, ElapsedEventArgs e)
        {
            Run();
        }
    }
}