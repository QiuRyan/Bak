using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Jinyinmao.AuthManager.Service.User
{
    public class UserEntity : TableEntity
    {
        /// <summary>
        ///     Gets or sets the type of the account.
        /// </summary>
        /// <value>The type of the account.</value>
        public int AccountType { get; set; }

        /// <summary>
        ///     Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is alive.
        /// </summary>
        /// <value><c>true</c> if this instance is alive; otherwise, <c>false</c>.</value>
        public bool IsAlive { get; set; }

        /// <summary>
        ///     Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }
    }
}