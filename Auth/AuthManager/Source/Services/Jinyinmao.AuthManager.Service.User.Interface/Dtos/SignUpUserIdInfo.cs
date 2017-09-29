using System;

namespace Jinyinmao.AuthManager.Service.User.Interface.Dtos
{
    /// <summary>
    ///     Class SignUpUserIdInfo.
    /// </summary>
    public class SignUpUserIdInfo
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the cellphone is registered.
        /// </summary>
        /// <value><c>true</c> if registered; otherwise, <c>false</c>.</value>
        public bool Registered { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}