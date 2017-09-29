using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class WeChatBind : Command
    {
        public string OpenId { get; set; }

        public string UserIdentifier { get; set; }
    }
}