namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    ///     Class BasicResponse.
    /// </summary>
    public class BasicResponse
    {
        /// <summary>
        ///     Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        public string remark { get; set; }

        /// <summary>
        ///     Gets or sets the response data.
        /// </summary>
        /// <value>The response data.</value>
        public int responseData { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BasicResponse" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool result { get; set; }
    }
}