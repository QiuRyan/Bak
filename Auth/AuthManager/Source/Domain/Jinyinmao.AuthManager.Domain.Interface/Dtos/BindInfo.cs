namespace Jinyinmao.AuthManager.Domain.Interface.Dtos
{
    /// <summary>
    ///     Class BindInfo.
    /// </summary>
    public class BindInfo
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BindInfo" /> is flag.
        /// </summary>
        /// <value><c>true</c> if flag; otherwise, <c>false</c>.</value>
        public int Flag { get; set; }

        /// <summary>
        ///     Gets or sets the open identifier.
        /// </summary>
        /// <value>The open identifier.</value>
        public string OpenId { get; set; }

        /// <summary>
        ///     Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string UserIdentifier { get; set; }
    }
}