using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Moe.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class UserAuthInfoResponse.
    /// </summary>
    public class UserAuthInfoResponse
    {
        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        [Required]
        [JsonProperty("clientType")]
        public long ClientType { get; set; }

        /// <summary>
        ///     账户是否冻结
        /// </summary>
        [Required]
        [JsonProperty("closed")]
        public bool Closed { get; set; }

        /// <summary>
        ///     活动编号(推广相关).
        /// </summary>
        [Required]
        [JsonProperty("contractId")]
        public long ContractId { get; set; }

        /// <summary>
        ///     是否设定了密码
        /// </summary>
        [Required]
        [JsonProperty("hasSetPassword")]
        public bool HasSetPassword { get; set; }

        /// <summary>
        ///     额外信息.
        /// </summary>
        [Required]
        [JsonProperty("info")]
        public Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     邀请码(推广相关).
        /// </summary>
        [Required]
        [JsonProperty("inviteBy")]
        public string InviteBy { get; set; }

        /// <summary>
        ///     邀请码(推广相关).
        /// </summary>
        [Required]
        [JsonProperty("inviteFor")]
        public string InviteFor { get; set; }

        /// <summary>
        ///     登录名.
        /// </summary>
        [Required]
        [JsonProperty("loginNames")]
        public List<string> LoginNames { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        [Required]
        [JsonProperty("outletCode")]
        public string OutletCode { get; set; }

        /// <summary>
        ///     登录密码错误次数
        /// </summary>
        [Required]
        [JsonProperty("passwordErrorCount")]
        public int PasswordErrorCount { get; set; }

        /// <summary>
        ///     注册时间.
        /// </summary>
        [Required]
        [JsonProperty("registerTime")]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        ///     用户唯一标识.
        /// </summary>
        [Required]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }

    internal static class UserAuthInfoEx
    {
        internal static UserAuthInfoResponse ToResponse(this UserInfo info)
        {
            return new UserAuthInfoResponse
            {
                Info = info.Info,
                Cellphone = info.Cellphone,
                Closed = info.Closed,
                ContractId = info.ContractId,
                HasSetPassword = info.HasSetPassword,
                InviteBy = info.InviteBy,
                InviteFor = info.InviteFor,
                LoginNames = info.LoginNames,
                OutletCode = info.OutletCode,
                PasswordErrorCount = info.PasswordErrorCount,
                RegisterTime = info.RegisterTime,
                UserIdentifier = info.UserId.ToGuidString()
            };
        }
    }
}