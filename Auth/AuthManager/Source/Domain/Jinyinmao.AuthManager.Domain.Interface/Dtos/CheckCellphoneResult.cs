namespace Jinyinmao.AuthManager.Domain.Interface.Dtos
{
    /// <summary>
    ///     Class CheckCellphoneResult.
    /// </summary>
    public class CheckCellphoneResult
    {
        /// <summary>
        ///     是否注册
        /// </summary>
        public bool Result { get; set; }

        public string UserIdentifier { get; set; }
    }
}