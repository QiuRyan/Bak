using System;

namespace Jinyinmao.AuthManager.Domain.Core.Commands
{
    public interface ICommand
    {
        /// <summary>
        ///     Gets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        Guid CommandId { get; set; }
    }
}