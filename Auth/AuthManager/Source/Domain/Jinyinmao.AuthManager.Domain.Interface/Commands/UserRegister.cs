using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class UserRegister : Command
    {
        public string Cellphone { get; set; }

        public long ClientType { get; set; }

        public long ContractId { get; set; }

        public Dictionary<string, object> Info { get; set; }

        public string InviteBy { get; set; }

        public string InviteFor { get; set; }

        public string OutletCode { get; set; }

        public string Password { get; set; }

        public Guid UserId { get; set; }
    }
}