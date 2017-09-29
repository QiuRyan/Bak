using Orleans.Concurrency;
using System;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Domain.Interface.Dtos
{
    [Immutable]
    public class UserInfo
    {
        public string Cellphone { get; set; }

        public bool Closed { get; set; }

        public long ContractId { get; set; }

        public bool HasSetPassword { get; set; }

        public Dictionary<string, object> Info { get; set; }

        public string InviteBy { get; set; }

        public string InviteFor { get; set; }

        public DateTime LastModified { get; set; }

        public List<string> LoginNames { get; set; }

        public string OutletCode { get; set; }

        public int PasswordErrorCount { get; set; }

        public DateTime RegisterTime { get; set; }

        public Guid UserId { get; set; }
    }
}