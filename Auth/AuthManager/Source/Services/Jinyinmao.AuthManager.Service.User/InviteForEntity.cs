using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Jinyinmao.AuthManager.Service.User
{
    /// <summary>
    ///     Class InviteForEntity.
    /// </summary>
    public class InviteForEntity : TableEntity
    {
        /// <summary>
        ///     Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the use time.
        /// </summary>
        /// <value>The use time.</value>
        public DateTime UseTime { get; set; }
    }
}