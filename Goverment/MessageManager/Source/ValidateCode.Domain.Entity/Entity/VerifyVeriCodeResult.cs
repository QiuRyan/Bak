namespace ValidateCode.Domain.Entity.Entity
{
    /// <summary>
    ///     VerifyVeriCodeResult.
    /// </summary>
    public class VerifyVeriCodeResult
    {
        /// <summary>
        ///     剩余次数
        /// </summary>
        /// <value>The remain count.</value>
        public int RemainCount { get; set; }

        /// <summary>
        ///     是否成功
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>
        ///     验签
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }
    }
}