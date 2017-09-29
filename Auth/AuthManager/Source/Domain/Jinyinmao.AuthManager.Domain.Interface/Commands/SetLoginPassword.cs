using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class SetLoginPassword : Command
    {
        public string Password { get; set; }

        public string Salt { get; set; }

        public Guid UserId { get; set; }
    }
}