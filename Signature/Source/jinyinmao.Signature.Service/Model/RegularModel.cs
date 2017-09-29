using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Newtonsoft.Json;

namespace jinyinmao.Signature.Service
{
    /// <summary>
    ///     Class OrderInfoEx.
    /// </summary>
    public static class OrderInfoEx
    {
        public static bool IsSignature(this OrderInfo order)
        {
            return order.WTAgreementViewUrl.IsNotNullOrEmpty() && order.WTAgreementDownUrl.IsNotNullOrEmpty() && order.JKAgreementViewUrl.IsNotNullOrEmpty() && order.JKAgreementDownUrl.IsNotNullOrEmpty();
        }
    }

    public class AgreementInfoEntity : TableEntity
    {
        /// <summary>
        ///     借款协议下载URL
        /// </summary>
        /// <value>The jk agreement down URL.</value>
        public string BorrowAgreementDownUrl { get; set; }

        /// <summary>
        ///     借款协议查看URL
        /// </summary>
        /// <value>The jk agreement view URL.</value>
        public string BorrowAgreementViewUrl { get; set; }

        /// <summary>
        ///     Gets or sets the jk contract identifier.
        /// </summary>
        /// <value>The jk contract identifier.</value>
        public string BorrowContractId { get; set; }

        /// <summary>
        ///     委托协议下载URL
        /// </summary>
        /// <value>The wt agreement down URL.</value>
        public string EntrustAgreementDownUrl { get; set; }

        /// <summary>
        ///     委托协议查看URL
        /// </summary>
        /// <value>The wt agreement view URL.</value>
        public string EntrustAgreementViewUrl { get; set; }

        /// <summary>
        ///     Gets or sets the wt contract identifier.
        /// </summary>
        /// <value>The wt contract identifier.</value>
        public string EntrustContractId { get; set; }

        /// <summary>
        ///     Gets or sets the FDD account identifier.
        /// </summary>
        /// <value>The FDD account identifier.</value>
        public string FDDAccountId { get; set; }

        /// <summary>
        ///     Gets or sets the order indentifier.
        /// </summary>
        /// <value>The order indentifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the user indentifier.
        /// </summary>
        /// <value>The user indentifier.</value>
        public string UserId { get; set; }
    }

    public class BasicUserInfo
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the credential no.
        /// </summary>
        /// <value>The credential no.</value>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real.
        /// </summary>
        /// <value>The name of the real.</value>
        public string RealName { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        /// </summary>
        public bool Verified { get; set; }
    }

    /// <summary>
    ///     OrderInfo.
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        ///     交易流水唯一标识
        /// </summary>
        [Required]
        [JsonProperty("accountTransactionIdentifier")]
        public string AccountTransactionIdentifier { get; set; }

        /// <summary>
        ///     卡券代码
        /// </summary>
        /// <value>The type of the coupon.</value>
        [Required]
        [JsonProperty("couponId")]
        public string CouponId { get; set; }

        /// <summary>
        ///     卡券类型
        /// </summary>
        /// <value>The type of the coupon.</value>
        [Required]
        [JsonProperty("couponType")]
        public int CouponType { get; set; }

        /// <summary>
        ///     默认的法大大账户
        /// </summary>
        /// <value>The FDD account identifier.</value>
        [JsonProperty("fddAccountID")]
        public string FDDAccountId { get; set; }

        /// <summary>
        ///     预期收益，以“分”为单位
        /// </summary>
        [Required]
        [JsonProperty("interest")]
        public long Interest { get; set; }

        /// <summary>
        ///     是否已经还款
        /// </summary>
        [Required]
        [JsonProperty("isRepaid")]
        public bool IsRepaid { get; set; }

        /// <summary>
        ///     借款协议PDF下载地址
        /// </summary>
        /// <value>The jk agreement down URL.</value>
        public string JKAgreementDownUrl { get; set; }

        /// <summary>
        ///     借款协议PDF查看地址
        /// </summary>
        /// <value>The jk agreement view URL.</value>
        public string JKAgreementViewUrl { get; set; }

        /// <summary>
        ///     订单唯一标识
        /// </summary>
        [Required]
        [JsonProperty("orderIdentifier")]
        public string OrderId { get; set; }

        /// <summary>
        ///     订单编号
        /// </summary>
        [Required]
        [JsonProperty("orderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        ///     订单状态 -1:交易关闭 10:投标中  20:项目满标  25:已起息 30:项目结息, 35:已还款 40:最迟还款日
        /// </summary>
        /// <value>The order status.</value>
        [Required]
        [JsonProperty("orderStatus")]
        public int OrderStatus { get; set; }

        /// <summary>
        ///     下单时间
        /// </summary>
        [Required]
        [JsonProperty("orderTime")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     投资本金
        /// </summary>
        [Required]
        [JsonProperty("principal")]
        public long Principal { get; set; }

        /// <summary>
        ///     产品类别
        /// </summary>
        [Required]
        [JsonProperty("productCategory")]
        public long ProductCategory { get; set; }

        /// <summary>
        ///     产品唯一标识
        /// </summary>
        [Required]
        [JsonProperty("productIdentifier")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     还款时间
        /// </summary>
        [Required]
        [JsonProperty("repaidTime")]
        public DateTime RepaidTime { get; set; }

        /// <summary>
        ///     状态码 -1 交易关闭 0 投标中 1 已放款(起息)
        /// </summary>
        [Required]
        [JsonProperty("resultCode")]
        public int ResultCode { get; set; }

        /// <summary>
        ///     订单确认结果时间
        /// </summary>
        [Required]
        [JsonProperty("resultTime")]
        public DateTime ResultTime { get; set; }

        /// <summary>
        ///     结息日期
        /// </summary>
        [Required]
        [JsonProperty("settleDate")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     交易描述
        /// </summary>
        [Required]
        [JsonProperty("transDesc")]
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public BasicUserInfo UserInfo { get; set; }

        /// <summary>
        ///     起息日期
        /// </summary>
        [Required]
        [JsonProperty("valueDate")]
        public DateTime ValueDate { get; set; }

        /// <summary>
        ///     委托协议PDF下载地址
        /// </summary>
        /// <value>The wt agreement down URL.</value>
        public string WTAgreementDownUrl { get; set; }

        /// <summary>
        ///     委托协议PDF查看地址
        /// </summary>
        /// <value>The wt agreement view URL.</value>
        public string WTAgreementViewUrl { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        [Required]
        [JsonProperty("yield")]
        public int Yield { get; set; }
    }

    /// <summary>
    ///     协议将要展示的字段信息
    /// </summary>
    public class ProductAgreementInfo : TableEntity
    {
        [JsonProperty("agreementValue")]
        public string AgreementValue { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }
    }

    public class ProductAgreementValue : TableEntity
    {
        /// <summary>
        ///     项目到期日
        /// </summary>
        /// <value>The bill due date.</value>
        [JsonProperty("billDueDate")]
        public string BillDueDate { get; set; }

        /// <summary>
        ///     出票金额
        /// </summary>
        /// <value>The bill money.</value>
        [JsonProperty("billMoney")]
        public string BillMoney { get; set; }

        /// <summary>
        ///     票号
        /// </summary>
        /// <value>The bill no.</value>
        [JsonProperty("billNo")]
        public string BillNo { get; set; }

        /// <summary>
        ///     融资人身份证号
        /// </summary>
        /// <value>The card no.</value>
        [JsonProperty("cardNo")]
        public string CardNo { get; set; }

        /// <summary>
        ///     融资企业营业执照号码
        /// </summary>
        /// <value>The enterprise license number.</value>
        [JsonProperty("enterpriseLicenseNum")]
        public string EnterpriseLicenseNum { get; set; }

        /// <summary>
        ///     融资企业名称
        /// </summary>
        /// <value>The financier name of user.</value>
        [JsonProperty("financierNameOfUser")]
        public string FinancierNameOfUser { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        /// <value>The fund usage.</value>
        [JsonProperty("fundUsage")]
        public string FundUsage { get; set; }

        /// <summary>
        ///     担保方
        /// </summary>
        /// <value>The full name of the guarantee.</value>
        [JsonProperty("guaranteeFullName")]
        public string GuaranteeFullName { get; set; }

        /// <summary>
        ///     原债务人信息
        /// </summary>
        /// <value>The name of the original debtor.</value>
        [JsonProperty("originalDebtorName")]
        public string OriginalDebtorName { get; set; }

        /// <summary>
        ///     付款行全称
        /// </summary>
        /// <value>The full name of the pay bank.</value>
        [JsonProperty("payBankFullName")]
        public string PayBankFullName { get; set; }

        /// <summary>
        ///     出票人
        /// </summary>
        /// <value>The payer name of user.</value>
        [JsonProperty("payerNameOfUser")]
        public string PayerNameOfUser { get; set; }

        /// <summary>
        ///     项目编号
        /// </summary>
        /// <value>The product code.</value>
        [JsonProperty("productCode")]
        public string ProductCode { get; set; }

        /// <summary>
        ///     最迟还款日
        /// </summary>
        /// <value>The product latest repayment date.</value>
        [JsonProperty("productLatestRepaymentDate")]
        public string ProductLatestRepaymentDate { get; set; }

        /// <summary>
        ///     募集开始日
        /// </summary>
        /// <value>The product raise start date time.</value>
        [JsonProperty("productRaiseStartDateTime")]
        public string ProductRaiseStartDateTime { get; set; }

        /// <summary>
        ///     年化利率
        /// </summary>
        /// <value>The product yield.</value>
        [JsonProperty("productYield")]
        public string ProductYield { get; set; }
    }

    public class PuHuiZhongYinCarInfo
    {
        /// <summary>
        ///     车子品牌
        /// </summary>
        /// <value>The car brand.</value>
        public string CarBrand { get; set; }

        /// <summary>
        ///     评估价格
        /// </summary>
        /// <value>The evaluating price.</value>
        public long EvaluatingPrice { get; set; }

        /// <summary>
        ///     车牌号
        /// </summary>
        /// <value>The license plate number.</value>
        public string LicensePlateNumber { get; set; }

        /// <summary>
        ///     公里数
        /// </summary>
        /// <value>The mileage.</value>
        public string Mileage { get; set; }

        /// <summary>
        ///     使用年限
        /// </summary>
        /// <value>The number of year.</value>
        public int NumberOfYear { get; set; }

        /// <summary>
        ///     车辆登记证
        /// </summary>
        /// <value>The registration certificate.</value>
        public string RegistrationCertificate { get; set; }

        /// <summary>
        ///     车产抵押物编号
        /// </summary>
        /// <value>The vehicle collateral identifier.</value>
        public string VehicleCollateralId { get; set; }

        /// <summary>
        ///     车辆信息
        /// </summary>
        /// <value>The vehicle information.</value>
        public string VehicleInfo { get; set; }

        /// <summary>
        ///     行驶证
        /// </summary>
        /// <value>The vehicle license.</value>
        public string VehicleLicense { get; set; }

        /// <summary>
        ///     车辆照片
        /// </summary>
        /// <value>The vehicle photos.</value>
        public string VehiclePhotos { get; set; }
    }

    public class PuHuiZhongYinDebtorBasicInfo
    {
        #region 教育信息

        /// <summary>
        ///     学历
        /// </summary>
        /// <value>The education.</value>
        public string Education { get; set; }

        #endregion 教育信息

        /// <summary>
        ///     职称
        /// </summary>
        /// <value>The job title.</value>
        public string JobTitle { get; set; }

        #region 基本信息

        /// <summary>
        ///     身份证
        /// </summary>
        /// <value>The card no.</value>
        public string CardNo { get; set; }

        /// <summary>
        ///     居住地址
        /// </summary>
        public string OriginalDebtorAddress { get; set; }

        /// <summary>
        ///     户口地址
        /// </summary>
        public string OriginalDebtorBeneficiaryAddress { get; set; }

        /// <summary>
        ///     出生日期
        /// </summary>
        public DateTime? OriginalDebtorBirthday { get; set; }

        /// <summary>
        ///     婚否 <see cref="OriginalDebtorMarriage" />.
        /// </summary>
        public bool OriginalDebtorMarriage { get; set; }

        /// <summary>
        ///     姓名
        /// </summary>
        public string OriginalDebtorName { get; set; }

        /// <summary>
        ///     居住地址
        /// </summary>
        public string OriginalDebtorPlaceOfResidence { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public string OriginalDebtorSex { get; set; }

        #endregion 基本信息
    }

    public class PuHuiZhongYinHouseInfo
    {
        /// <summary>
        ///     房地产地址
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        ///     房地产地址(code)
        /// </summary>
        /// <value>The realty collateral address.</value>
        public string RealtyCollateralAddress { get; set; }

        /// <summary>
        ///     房产证编号
        /// </summary>
        /// <value>The realty collateral area.</value>
        public string RealtyCollateralArea { get; set; }

        /// <summary>
        ///     Gets or sets the realty collateral code.
        /// </summary>
        /// <value>The realty collateral code.</value>
        public string RealtyCollateralCode { get; set; }

        /// <summary>
        ///     其他说明
        /// </summary>
        /// <value>The collateral code.</value>
        public string RealtyCollateralDescription { get; set; }

        /// <summary>
        ///     公允价值
        /// </summary>
        /// <value>The collateral code.</value>
        public string RealtyCollateralFairValue { get; set; }

        /// <summary>
        ///     户主
        /// </summary>
        /// <value>The collateral code.</value>
        public string RealtyCollateralHouseholder { get; set; }

        /// <summary>
        ///     抵押物ID(为Collateral的CollateralId)
        /// </summary>
        /// <value>The realty collateral identifier.</value>
        public string RealtyCollateralId { get; set; }

        /// <summary>
        ///     房产证截图
        /// </summary>
        /// <value>The collateral code.</value>
        public string RealtyCollateralImgs { get; set; }

        /// <summary>
        ///     房产抵押物状态
        /// </summary>
        /// <value><c>true</c> if [realty collateral status]; otherwise, <c>false</c>.</value>
        public bool RealtyCollateralStatus { get; set; }

        /// <summary>
        ///     房产有效面积
        /// </summary>
        /// <value>The usage counter.</value>
        public long UsageCounter { get; set; }

        #region 新增字段

        /// <summary>
        ///     The time completion
        /// </summary>
        private string timeCompletion;

        /// <summary>
        ///     房产截图
        /// </summary>
        /// <value>The estate screenshots.</value>
        public string EstateScreenshots { get; set; }

        /// <summary>
        ///     产调截图
        /// </summary>
        /// <value>From screenshots.</value>
        public string FromScreenshots { get; set; }

        /// <summary>
        ///     竣工日期
        /// </summary>
        /// <value>The time completion.</value>
        public string TimeCompletion
        {
            get
            {
                DateTime result;
                return DateTime.TryParse(this.timeCompletion, out result) ? result.ToString("MM/dd/yyyy HH:mm:ss") : DateTime.UtcNow.ToChinaStandardTime().ToString("MM/dd/yyyy HH:mm:ss");
            }
            set { this.timeCompletion = value; }
        }

        #endregion 新增字段
    }

    public class RegularProductInfo
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the AssetInfoDesc
        /// </summary>
        /// <value>The AssetInfoDesc</value>
        public string AssetInfoDesc { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the drawee.
        /// </summary>
        /// <value>The drawee.</value>
        public string Drawee { get; set; }

        /// <summary>
        ///     Gets or sets the drawee information.
        /// </summary>
        /// <value>The drawee information.</value>
        public string DraweeInfo { get; set; }

        /// <summary>
        ///     Gets or sets the endorse image link.
        /// </summary>
        /// <value>The endorse image link.</value>
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     Gets or sets the endorse images link.
        /// </summary>
        /// <value>The endorse images link.</value>
        public List<string> EndorseImagesLink { get; set; }

        /// <summary>
        ///     Gets or sets the end sell time.
        /// </summary>
        /// <value>The end sell time.</value>
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the enterprise information.
        /// </summary>
        /// <value>The enterprise information.</value>
        public string EnterpriseInfo { get; set; }

        /// <summary>
        ///     Gets or sets the enterprise license.
        /// </summary>
        /// <value>The enterprise license.</value>
        public string EnterpriseLicense { get; set; }

        /// <summary>
        ///     Gets or sets the name of the enterprise.
        /// </summary>
        /// <value>The name of the enterprise.</value>
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum amount.
        /// </summary>
        /// <value>The financing sum amount.</value>
        public long FinancingSumAmount { get; set; }

        /// <summary>
        ///     是否放款
        /// </summary>
        /// <value><c>true</c> if this instance is loans; otherwise, <c>false</c>.</value>
        public bool IsLoans { get; set; }

        /// <summary>
        ///     Gets or sets the Is PuHuiZhongYin.
        /// </summary>
        /// <value>The Is PuHuiZhongYin.</value>
        public bool IsPuHuiZhongYin { get; set; }

        /// <summary>
        ///     Gets or sets the issue no.
        /// </summary>
        /// <value>The issue no.</value>
        public int IssueNo { get; set; }

        /// <summary>
        ///     Gets or sets the issue time.
        /// </summary>
        /// <value>The issue time.</value>
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     Gets or sets the paid amount.
        /// </summary>
        /// <value>The paid amount.</value>
        public long PaidAmount { get; set; }

        /// <summary>
        ///     Gets or sets the period.
        /// </summary>
        /// <value>The period.</value>
        public int Period { get; set; }

        // /// <summary> /// Gets or sets the PHZYInfo arguments. /// </summary> /// <value>The
        // arguments.</value> public Dictionary<string, object> PHZYInfoArgs { get; set; }

        /// <summary>
        ///     Gets or sets the pledge no.
        /// </summary>
        /// <value>The pledge no.</value>
        public string PledgeNo { get; set; }

        /// <summary>
        ///     Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        public long ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>The product no.</value>
        public string ProductNo { get; set; }

        /// <summary>
        /// </summary>
        public PuHuiZhongYinCarInfo PuHuiZhongYinCarInfo { get; set; }

        /// <summary>
        /// </summary>
        public PuHuiZhongYinDebtorBasicInfo PuHuiZhongYinDebtorBasicInfo { get; set; }

        /// <summary>
        /// </summary>
        public PuHuiZhongYinHouseInfo PuHuiZhongYinHouseInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="RegularProductInfo" /> is repaid.
        /// </summary>
        /// <value><c>true</c> if repaid; otherwise, <c>false</c>.</value>
        public bool Repaid { get; set; }

        /// <summary>
        ///     Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public DateTime? RepaidTime { get; set; }

        /// <summary>
        ///     Gets or sets the repayment deadline.
        /// </summary>
        /// <value>The repayment deadline.</value>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     Gets or sets the ReturnMoneyMethod.
        /// </summary>
        /// <value>The pledge no.</value>
        public string ReturnMoneyMethod { get; set; }

        /// <summary>
        ///     Gets or sets the risk management.
        /// </summary>
        /// <value>The risk management.</value>
        public string RiskManagement { get; set; }

        /// <summary>
        ///     Gets or sets the risk management information.
        /// </summary>
        /// <value>The risk management information.</value>
        public string RiskManagementInfo { get; set; }

        /// <summary>
        ///     Gets or sets the risk management mode.
        /// </summary>
        /// <value>The risk management mode.</value>
        public string RiskManagementMode { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [sold out].
        /// </summary>
        /// <value><c>true</c> if [sold out]; otherwise, <c>false</c>.</value>
        public bool SoldOut { get; set; }

        /// <summary>
        ///     Gets or sets the sold out time.
        /// </summary>
        /// <value>The sold out time.</value>
        public DateTime? SoldOutTime { get; set; }

        /// <summary>
        ///     Gets or sets the start sell time.
        /// </summary>
        /// <value>The start sell time.</value>
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the TagNames.
        /// </summary>
        /// <value>The yield.</value>
        public string TagNames { get; set; }

        /// <summary>
        ///     Gets or sets the unit price.
        /// </summary>
        /// <value>The unit price.</value>
        public long UnitPrice { get; set; }

        /// <summary>
        ///     Gets or sets the usage.
        /// </summary>
        /// <value>The usage.</value>
        public string Usage { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the value date mode.
        /// </summary>
        /// <value>The value date mode.</value>
        public int? ValueDateMode { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        public int? ValueDays { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int Yield { get; set; }
    }

    /// <summary>
    ///     生成电子签章产品信息
    /// </summary>
    public class SignatureProductInfo
    {
        [JsonProperty("isLoans")]
        public bool IsLoans { get; set; }

        [JsonProperty("isSignature")]
        public bool IsSignature { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }
    }
}