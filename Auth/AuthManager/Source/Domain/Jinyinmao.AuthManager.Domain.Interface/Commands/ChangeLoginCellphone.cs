using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class ChangeLoginCellphone : Command
    {
        public string LoginCellphone { get; set; }

        public string NewCellphone { get; set; }
    }
}