// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : Program.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:22
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries.Helper;
using Jinyinmao.AuthManager.Libraries.Parameter;
using Jinyinmao.AuthManager.Service.User;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleLab
{
    public class Apple
    {
        public int Size { get; set; }

        public DateTime Time { get; set; }
    }

    public class SignUpRequest
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        [JsonProperty("clientType")]
        public long? ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        [JsonProperty("contractId")]
        public long? ContractId { get; set; }

        /// <summary>
        ///     额外信息.
        /// </summary>
        /// <value>额外信息.</value>
        [JsonProperty("info")]
        public Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     邀请码(推广相关)
        /// </summary>
        [JsonProperty("inviteBy")]
        public string InviteBy { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        [JsonProperty("outletCode")]
        public string OutletCode { get; set; }

        /// <summary>
        ///     用户设置的密码（6-18位的数字、字母、一般特殊字符组合）
        /// </summary>
        [Required]
        [StringLength(18, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z\d~!@#$%^&*_]{6,18}$")]
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码口令
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 32)]
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    internal class Program
    {
        private static void Decrypt(string str)
        {
            byte[] data = Convert.FromBase64String(str);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString("<RSAKeyValue><Modulus>m8cuFS77E7MyspqAYqPwUQSTHaCeB3ckMEkJidvRrBoTy99LbPhd8dEXjAVezpjtbCE8og3+1h+Cxg6iksiURVLVD7F2VlE+EybZfaryUS9OMPIguQMX5E+C4u3Ayv08qTm+5WUB+tfVsqYmBjO/GNIpCoKaRif/PrMNgm8sk4qrsjnNrxN2xnc7Tn80hy6ypJ1bIszQpEUInv37uW3JvhLrFTrq2HrXdtujc1oOgHgnMS+TRAQiE/BtgqFa82W1yrQVp3XCg5IEQWzaRBwzf1AK/3xj6RjAZkA1+0vwyYGrEsMuVKJJ6A6CMspE6H5NJS3HUeGH5vqE9gzcY1/3kQ==</Modulus><Exponent>AQAB</Exponent><P>yUP9ESI2lDHyR7CpLdAq6wcNjwyQ/6LxZvr9gk6CtWQvPC3NhFHXOq76rIGH7pvP69vdm2+KK/SJNsOLPkRU5l+UPcrbAIIjLCmVqRGNlqMx8hvOR7WVOZnflAyKkRLsj7qezXShmcXmh+Tpw86Ev2A158i6X7MwcfzPui72HDc=</P><Q>xiRfITc+6uJXBWpmC28OfjMB0WWrmaB1HptdSXcw2TZkFqFSLEpE1c3vuzgx8ebMTXw4ecF9QYnId7npXLqzTHw++1z+jM/QdcqIS7Bvqk6h2m8ASDT3Xq2TguVfcahMRb0eAmxD4cTAOJGesnMDrqa1oIRb6NZY5rG0e9kE9nc=</Q><DP>YMoWgM/gSYJ/jmRx57tNeHuK1LlpXdbhmvGnSqwxBcSpRpqMVE77X5hYqu8cDO7Xngi9WQvZ+et+tVxysT4xShy68MCGc4ciHRHejFEJs2DGGzWuDSRRIENUlyE03jGonDJWPl4RfR8ED2RR2z2M72/4XIBWyMEm0hqPV21QJB8=</DP><DQ>AjXHpdwy5HXP2KyeJMSBWeEBxS8oIdeLVuMOwFIHBnU32pTEEOBnMidQ5Dq1O+iCIN8g1iLVXdTGmqdFNhaTB2hfX3hjEnkC7z1qFcYLCNBFt+UDEMseczzmZ1BdpyvkZea9HfPNMgh6yGa/aWglA16yqe6wA8HYTgJva+44wvM=</DQ><InverseQ>RuCHHiXPBDU7u2xT7oXNmJqdbWIvcCorWbpklzA9+mYIj0pTJ+F5Cwleg8v4ckJKcLnb7XwzRobZ3GBcnETttU3EqJS2rtWS91QvQYN7XNPsTPisnb6fifWu+CVswWIYvUU4AdRHxmpO/Aa8i2IAdm92Wwu953nrVRssZKQCAn8=</InverseQ><D>R4n8VswhkBV+ldkwVpx1E6/nI/cMO99yJh6Um9PwgXnkV38vc1bIRfJBPxOES5qdhkfpQX7t5kXIV86GyKQGu6Njp3ZXIyLiQAdaYETuTWxNG1tGvdB222nMcQzAujf78LdNPKnbc+hFAmsdEUIYY3y4TlRWJxvdM10lOh52OTcBX8a+vF30T8aaGtM1cgFqet3DH+zLEdh+EqxmrCZ9XDsPojqEzw6zkDtBs2ilVmz22PnFFVEqWX/yt5yqdr2vEKcBfDUvlJ7pBBnUeqJdv64sPIdL0R1qm2KV+0gW/EZkmnIdVEeD+g9ZQGx0S+akVZAUws3YRsBflmZiB5zZJQ==</D></RSAKeyValue>");
            byte[] bytes = rsa.Decrypt(data, false);
            Console.WriteLine(Encoding.UTF8.GetString(bytes));
        }

        private static string GetInviteFor(string cellphone)
        {
            Random ran = new Random((int)DateTime.UtcNow.UnixTimestamp());
            int num = ran.Next(10, 100);
            return (cellphone.GetLast(10) + num).X10ToX36();
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToNullCoalescingExpression")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        private static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();

            CloudStorageAccount account = CloudStorageAccount.Parse("BlobEndpoint=https://jymstoreproduct.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoreproduct.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoreproduct.table.core.chinacloudapi.cn/;AccountName=jymstoreproduct;AccountKey=1MYJoT5rVW6HDdJKYGpTI4q28HrJtYnJbwqHPJdH+9KsOAbvSjTzWHfsaYButdzsF/vMZx3hwTv1Iz/CdcM2cw==");
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("JYMUser");

            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where("(AccountType eq {0}) and (((PartitionKey eq '{1}') or (RowKey eq '{2}')) and (IsAlive eq true))"
                .FormatWith(AccountType.Cellphone.Code, string.Empty, "15221094470"));
            watch.Start();

            watch.Stop();
            Console.WriteLine(watch.Elapsed);
            Console.ReadKey();
        }

        private static UserEntity ReadFormRedis(string userIdentifier)
        {
            ConnectionMultiplexer temp = ConnectionMultiplexer.Connect("biz.dev.ad.jinyinmao.com.cn:6383,password=Aa111111");
            IDatabase redisDatabase = temp.GetDatabase(2);
            return JsonConvert.DeserializeObject<UserEntity>(redisDatabase.StringGet(userIdentifier));
        }
    }
}