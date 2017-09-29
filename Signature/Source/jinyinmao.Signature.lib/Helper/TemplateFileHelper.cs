using Newtonsoft.Json;

namespace jinyinmao.Signature.lib
{
    /// <summary>
    ///     基础输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicResult<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }

        public static BasicResult<T> Failed(string message, T data)
        {
            return new BasicResult<T>
            {
                Result = false,
                Message = message,
                Data = data
            };
        }

        public static BasicResult<T> Success(string message, T data)
        {
            return new BasicResult<T>
            {
                Result = true,
                Message = message,
                Data = data
            };
        }
    }

    public class ContractDocTitleHelper
    {
        /// <summary>
        ///     银票保理应收账款转让协议
        /// </summary>
        /// <value>The babill factoring collect transfer.</value>
        public static readonly string BabillFactoringCollectTransfer = "银票保理应收账款转让协议";

        /// <summary>
        ///     银票保理委托协议
        /// </summary>
        /// <value>The babill factoring entrusted agreement.</value>
        public static readonly string BabillFactoringEntrustedAgreement = "银票保理委托协议";

        ///// <summary>
        /////     银保贷委托协议
        ///// </summary>
        ///// <value>The tabill entrusted agreement.</value>
        //public static readonly string BabillGuaranteeEntrustedAgreement = "银保贷委托协议.pdf";

        ///// <summary>
        /////     银保贷担保借款协议
        ///// </summary>
        ///// <value>The tabill loan agreement.</value>
        //public static readonly string BabillGuaranteeLoanAgreement = "银保贷担保借款协议.pdf";

        ///// <summary>
        /////     保理盈债权转让协议
        ///// </summary>
        ///// <value>The tabill entrusted agreement.</value>
        //public static readonly string FactoringSurplusCreditorRightsTransfer = "保理盈债权转让协议.pdf";

        ///// <summary>
        /////     保理盈委托协议
        ///// </summary>
        ///// <value>The tabill loan agreement.</value>
        //public static readonly string FactoringSurplusEntrustedAgreement = "保理盈委托协议.pdf";

        /// <summary>
        ///     委托协议
        /// </summary>
        /// <value>The tabill entrusted agreement.</value>
        public static readonly string EntrustedAgreementTitle = "委托协议";

        /// <summary>
        ///     担保贷担保借款协议
        /// </summary>
        /// <value>The tabill loan agreement.</value>
        public static readonly string GuaranteeLoanAgreementTitle = "担保借款协议";

        ///// <summary>
        /////     普惠众盈委托协议
        ///// </summary>
        ///// <value>The tabill entrusted agreement.</value>
        //public static readonly string PhzyEntrustedAgreement = "普惠众盈委托协议.pdf";
        /// <summary>
        ///     借款协议
        /// </summary>
        /// <value>The tabill loan agreement.</value>
        public static readonly string LoanAgreementTitle = "借款协议";

        /// <summary>
        ///     普通银票委托协议
        /// </summary>
        /// <value>The ordinary babill entrusted agreement.</value>
        public static readonly string OrdinaryBabillEntrustedAgreementTitle = "银票委托协议模板.pdf";

        /// <summary>
        ///     普通银票借款协议
        /// </summary>
        /// <value>The ordinary babill loan agreement.</value>
        public static readonly string OrdinaryBabillLoanAgreementTitle = "银票借款协议模板.pdf";

        ///// <summary>
        /////     普惠众盈债权转让协议
        ///// </summary>
        ///// <value>The tabill loan agreement.</value>
        //public static readonly string PhzyCreditorRightsTransfer = "普惠众盈债权转让协议.pdf";
        /// <summary>
        ///     余额猫自动交易授权委托书
        /// </summary>
        /// <value>The yem automated trading.</value>
        public static readonly string YEMAutomatedTradingTitle = "余额猫自动交易授权委托书";

        /// <summary>
        ///     余额猫投资协议
        /// </summary>
        /// <value>The yem investment agreement.</value>
        public static readonly string YEMInvestmentAgreementTitle = "余额猫投资协议";
    }

    public class TemplateFileHelper
    {
        /// <summary>
        ///     银票保理应收账款转让协议
        /// </summary>
        /// <value>The babill factoring collect transfer.</value>
        public static readonly string BabillFactoringCollectTransfer = "银票保理应收账款转让协议模板.pdf";

        /// <summary>
        ///     银票保理委托协议
        /// </summary>
        /// <value>The babill factoring entrusted agreement.</value>
        public static readonly string BabillFactoringEntrustedAgreement = "银票保理委托协议模板.pdf";

        ///// <summary>
        /////     银保贷委托协议
        ///// </summary>
        ///// <value>The tabill entrusted agreement.</value>
        //public static readonly string BabillGuaranteeEntrustedAgreement = "银保贷委托协议.pdf";

        ///// <summary>
        /////     银保贷担保借款协议
        ///// </summary>
        ///// <value>The tabill loan agreement.</value>
        //public static readonly string BabillGuaranteeLoanAgreement = "银保贷担保借款协议.pdf";

        ///// <summary>
        /////     保理盈债权转让协议
        ///// </summary>
        ///// <value>The tabill entrusted agreement.</value>
        //public static readonly string FactoringSurplusCreditorRightsTransfer = "保理盈债权转让协议.pdf";

        ///// <summary>
        /////     保理盈委托协议
        ///// </summary>
        ///// <value>The tabill loan agreement.</value>
        //public static readonly string FactoringSurplusEntrustedAgreement = "保理盈委托协议.pdf";

        /// <summary>
        ///     担保贷担保借款协议
        /// </summary>
        /// <value>The tabill loan agreement.</value>
        public static readonly string GuaranteeLoanAgreement = "担保贷借款协议模板.pdf";

        /// <summary>
        ///     担保贷委托协议
        /// </summary>
        /// <value>The tabill entrusted agreement.</value>
        public static readonly string GuaranteeLoanEntrustedAgreement = "担保贷委托协议模板.pdf";

        /// <summary>
        ///     普通银票委托协议
        /// </summary>
        /// <value>The ordinary babill entrusted agreement.</value>
        public static readonly string OrdinaryBabillEntrustedAgreement = "普通银票委托协议模板.pdf";

        /// <summary>
        ///     普通银票借款协议
        /// </summary>
        /// <value>The ordinary babill loan agreement.</value>
        public static readonly string OrdinaryBabillLoanAgreement = "普通银票借款协议模板.pdf";

        ///// <summary>
        /////     普惠众盈债权转让协议
        ///// </summary>
        ///// <value>The tabill loan agreement.</value>
        //public static readonly string PhzyCreditorRightsTransfer = "普惠众盈债权转让协议.pdf";

        ///// <summary>
        /////     普惠众盈委托协议
        ///// </summary>
        ///// <value>The tabill entrusted agreement.</value>
        //public static readonly string PhzyEntrustedAgreement = "普惠众盈委托协议.pdf";

        /// <summary>
        ///     商票贷委托协议
        /// </summary>
        /// <value>The tabill entrusted agreement.</value>
        public static readonly string TabillEntrustedAgreement = "商票委托协议模板.pdf";

        /// <summary>
        ///     商票贷借款协议
        /// </summary>
        /// <value>The tabill loan agreement.</value>
        public static readonly string TabillLoanAgreement = "商票借款协议模板.pdf";

        /// <summary>
        ///     余额猫自动交易授权委托书
        /// </summary>
        /// <value>The yem automated trading.</value>
        public static readonly string YEMAutomatedTrading = "余额猫自动交易授权委托书模板.pdf";

        /// <summary>
        ///     余额猫投资协议
        /// </summary>
        /// <value>The yem investment agreement.</value>
        public static readonly string YEMInvestmentAgreement = "余额猫投资协议模板.pdf";
    }
}