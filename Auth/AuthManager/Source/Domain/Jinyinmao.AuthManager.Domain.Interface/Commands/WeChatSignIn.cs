using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class WeChatSignIn : Command
    {
        public string Code { get; set; }
    }
}