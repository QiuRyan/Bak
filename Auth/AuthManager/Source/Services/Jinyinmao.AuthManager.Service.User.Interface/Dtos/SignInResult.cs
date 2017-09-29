using System;

namespace Jinyinmao.AuthManager.Service.User.Interface.Dtos
{
    /// <summary>
    ///     Class SignInResult.
    /// </summary>
    public class SignInResult
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="SignInResult" /> is lock.
        /// </summary>
        /// <value><c>true</c> if lock; otherwise, <c>false</c>.</value>
        public bool Lock
        {
            get { return this.RemainCount < 1; }
        }

        /// <summary>
        ///     Gets or sets the remain count.
        /// </summary>
        /// <value>The remain count.</value>
        public int RemainCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SignInResult" /> is successed.
        /// </summary>
        /// <value><c>true</c> if successed; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [user exist].
        /// </summary>
        /// <value><c>true</c> if [user exist]; otherwise, <c>false</c>.</value>
        public bool UserExist { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}