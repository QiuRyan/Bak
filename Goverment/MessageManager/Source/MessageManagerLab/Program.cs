// ***********************************************************************
// Project          : MessageManager
// File             : Program.cs
// Created          : 2015-11-28  14:51
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  16:50
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jinyinmao.MessageManagerLab
{
    internal class Program
    {
        private static async Task DoWork()
        {
            string requestUri = @"http://222.73.117.158/msg/HttpBatchSendSM?account=jinyinmao&pswd=Tch123456&mobile=18202199387&msg=11111&needstatus=true";
            using (HttpClient client = InitHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);

                string responseMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseMessage);
            }
        }

        private static HttpClient InitHttpClient()
        {
            return new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        }

        private static void Main(string[] args)
        {
            DoWork().Wait();
        }
    }
}