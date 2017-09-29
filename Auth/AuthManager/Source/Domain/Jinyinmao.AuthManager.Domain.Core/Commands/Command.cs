using System;

namespace Jinyinmao.AuthManager.Domain.Core.Commands
{
    public abstract class Command : ICommand
    {
        /// <summary>
        ///     Gets or sets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        public Guid EntityId { get; set; }

        #region ICommand Members

        /// <summary>
        ///     Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public Guid CommandId { get; set; }

        #endregion ICommand Members
    }
}