namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     Class PlatformTransaction.
    /// </summary>
    public class PlatformTransaction
    {
        /// <summary>
        ///     返利金额
        /// </summary>
        /// <value>The amount.</value>
        public long Amount{get; set;}

        /// <summary>
        ///     资产ID
        /// </summary>
        /// <value>The asset identifier.</value>
        public string AssetId{get; set;}

        /// <summary>
        ///     订单ID
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId{get; set;}

        /// <summary>
        ///     平台账号
        /// </summary>
        /// <value>The platform cg identifier.</value>
        public string PlatformCgId{get; set;}

        /// <summary>
        ///     备注 eg:红包,加息券等
        /// </summary>
        /// <value>The remark.</value>
        public string Remark{get; set;}

        /// <summary>
        ///     交易唯一编码
        /// </summary>
        /// <value>The transcation identifier.</value>
        public string TranscationId{get; set;}

        /// <summary>
        ///     用户唯一编码
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId{get; set;}
    }
}