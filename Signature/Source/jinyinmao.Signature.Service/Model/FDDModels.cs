using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace jinyinmao.Signature.Service
{
    /// <summary>
    ///     Class AgreementInfo.
    /// </summary>
    public class AgreementInfo
    {
        /// <summary>
        ///     Gets or sets the FDD account identifier.
        /// </summary>
        /// <value>The FDD account identifier.</value>
        public string FDDAccountId { get; set; }

        /// <summary>
        ///     Gets or sets the order indentifier.
        /// </summary>
        /// <value>The order indentifier.</value>
        public string OrderIndentifier { get; set; }

        /// <summary>
        ///     Gets or sets the user indentifier.
        /// </summary>
        /// <value>The user indentifier.</value>
        public string UserIndentifier { get; set; }
    }

    public class AgreementInfoRequest
    {
        /// <summary>
        ///     Gets or sets the agreement infos.
        /// </summary>
        /// <value>The agreement infos.</value>
        public List<AgreementInfo> AgreementInfos { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is complate.
        /// </summary>
        /// <value><c>true</c> if this instance is complate; otherwise, <c>false</c>.</value>
        public bool isComplate { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductIdentifier { get; set; }
    }

    /// <summary>
    ///     自动签署合同输出参数
    /// </summary>
    /// <seealso cref="BasicResponse" />
    public class AutoSignConatractResponse : BasicResponse
    {
        /// <summary>
        ///     合同下载地址
        /// </summary>
        /// <value>The download_url.</value>
        [JsonProperty("download_url")]
        public string Download_Url { get; set; }

        /// <summary>
        ///     合同查看地址
        /// </summary>
        /// <value>The viewpdf_url.</value>
        [JsonProperty("viewpdf_url")]
        public string ViewPdf_Url { get; set; }
    }

    /// <summary>
    ///     自动签章回调接受参数
    /// </summary>
    public class AutoSignNotifyResponse
    {
        /// <summary>
        ///     合同编号
        /// </summary>
        /// <value>The contract_ identifier.</value>
        [JsonProperty("contract_id")]
        public string Contract_Id { get; set; }

        /// <summary>
        ///     合同下载地址
        /// </summary>
        /// <value>The download_url.</value>
        [JsonProperty("download_url")]
        public string Download_Url { get; set; }

        /// <summary>
        ///     消息摘要
        /// </summary>
        /// <value>The MSG_ digest.</value>
        [JsonProperty("msg_digest")]
        public string Msg_Digest { get; set; }

        /// <summary>
        ///     签章结果
        /// </summary>
        /// <value>The result_ code.</value>
        [JsonProperty("result_code")]
        public string Result_Code { get; set; }

        /// <summary>
        ///     签章结果描述
        /// </summary>
        /// <value>The result_ desc.</value>
        [JsonProperty("result_code")]
        public string Result_Desc { get; set; }

        /// <summary>
        ///     请求时间
        /// </summary>
        /// <value>The timestamp.</value>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        ///     签署接口的交易号
        /// </summary>
        /// <value>The transaction_ identifier.</value>
        [JsonProperty("transaction_id")]
        public string Transaction_Id { get; set; }

        /// <summary>
        ///     合同查看地址
        /// </summary>
        /// <value>The viewpdf_url.</value>
        [JsonProperty("viewpdf_url")]
        public string ViewPdf_Url { get; set; }
    }

    /// <summary>
    ///     Class BasicGenerationResult.
    /// </summary>
    public class BasicGenerationResult
    {
        /// <summary>
        ///     Gets or sets the jk contract identifier.
        /// </summary>
        /// <value>The jk contract identifier.</value>
        public string BorrowContractId { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the wt contract identifier.
        /// </summary>
        /// <value>The wt contract identifier.</value>
        public string EntrustContractId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BasicGenerationResult" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the result message.
        /// </summary>
        /// <value>The result message.</value>
        public string ResultMessage { get; set; }
    }

    /// <summary>
    ///     返回结果基类
    /// </summary>
    public class BasicResponse
    {
        /// <summary>
        ///     1000 操作成功
        ///     2001 参数确实或者不合法
        ///     2002 业务异常,失败原因见msg
        ///     2003 其他错误,请联系法大大
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        /// <value>The MSG.</value>
        [JsonProperty("msg")]
        public string Msg { get; set; }

        /// <summary>
        ///     success error
        /// </summary>
        /// <value>The result.</value>
        [JsonProperty("result")]
        public string Result { get; set; }
    }

    /// <summary>
    ///     请求参数基类
    /// </summary>
    public class BasicRquest
    {
        /// <summary>
        ///     接入方ID
        /// </summary>
        /// <value>The app_ identifier.</value>
        [JsonProperty("app_id")]
        [Required]
        public string App_Id { get; set; }

        /// <summary>
        ///     消息摘要
        /// </summary>
        /// <value>The MSG_ digest.</value>
        [JsonProperty("msg_digest")]
        [Required]
        public string Msg_Digest { get; set; }

        /// <summary>
        ///     请求时间  yyyyMMddHHmmss
        /// </summary>
        /// <value>The timestamp.</value>
        [JsonProperty("timestamp")]
        [Required]
        public string Timestamp { get; set; }

        /// <summary>
        ///     版本号 默认为2.0
        /// </summary>
        /// <value>The v.</value>
        [JsonProperty("v")]
        public string V { get; set; } = "2.0";
    }

    /// <summary>
    ///     Class CACretificate.
    /// </summary>
    public class CACretificate
    {
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        [JsonProperty("customerid")]
        public string CustomerId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    /// <summary>
    ///     个人AC申请输出参数
    /// </summary>
    /// <seealso cref="jinyinmao.Signature.Service.BasicResponse" />
    public class CACretificateApplyResponse : BasicResponse
    {
        [JsonProperty("customer_id")]
        public string Customer_Id { get; set; }
    }

    /// <summary>
    ///     Class CACretificateResult.
    /// </summary>
    public class CACretificateResult
    {
        [JsonProperty("customerid")]
        public string CustomerId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("responseMsg")]
        public string ResponseMsg { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }
    }

    /// <summary>
    ///     Class CACretificateTableEntity.
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.Storage.Table.TableEntity" />
    public class CACretificateTableEntity : TableEntity
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public string CustomerId { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }
    }

    /// <summary>
    ///     合同归档请求参数
    /// </summary>
    /// <seealso cref="BasicRquest" />
    public class ContractArchiveRequest : BasicRquest
    {
        /// <summary>
        ///     合同编号 通过合同编号指定在那份文档上签署
        /// </summary>
        /// <value>The contract_ identifier.</value>
        [JsonProperty("contract_id")]
        [Required]
        [StringLength(32, ErrorMessage = "只允许长度<=32的英文或者数字字符")]
        public string Contract_Id { get; set; }
    }

    /// <summary>
    ///     合同归档输出参数
    /// </summary>
    public class ContractArchiveResponse : BasicResponse
    {
    }

    /// <summary>
    ///     下载已签署文档请求参数
    /// </summary>
    /// <seealso cref="BasicRquest" />
    public class ContractDownloadRequest : BasicRquest
    {
        /// <summary>
        ///     合同编号 通过合同编号指定在那份文档上签署
        /// </summary>
        /// <value>The contract_ identifier.</value>
        [JsonProperty("contract_id")]
        [Required]
        [StringLength(32, ErrorMessage = "只允许长度<=32的英文或者数字字符")]
        public string Contract_Id { get; set; }
    }

    /// <summary>
    ///     下载已签署文档输出参数
    /// </summary>
    /// <seealso cref="BasicResponse" />
    public class ContractDownloadResponse : BasicResponse
    {
    }

    /// <summary>
    ///     合同生成输出参数
    /// </summary>
    /// <seealso cref="BasicResponse" />
    public class ContractGenerationResponse : BasicResponse
    {
        /// <summary>
        ///     合同下载地址
        /// </summary>
        /// <value>The download_url.</value>
        [JsonProperty("download_url")]
        public string Download_Url { get; set; }

        /// <summary>
        ///     合同查看地址
        /// </summary>
        /// <value>The viewpdf_url.</value>
        [JsonProperty("viewpdf_url")]
        public string ViewPdf_Url { get; set; }
    }

    /// <summary>
    ///     合同模板传输输出参数
    /// </summary>
    public class ContractTemplateTransferResponse : BasicResponse
    {
    }

    /// <summary>
    ///     生成结果
    /// </summary>
    public class GenerationResult : BasicGenerationResult
    {
        /// <summary>
        ///     Gets or sets the jk download URL.
        /// </summary>
        /// <value>The jk download URL.</value>
        public string BorrowDownloadUrl { get; set; }

        /// <summary>
        ///     Gets or sets the jk view URL.
        /// </summary>
        /// <value>The jk view URL.</value>
        public string BorrowViewUrl { get; set; }

        /// <summary>
        ///     Gets or sets the wt download URL.
        /// </summary>
        /// <value>The wt download URL.</value>
        public string EntrustDownloadUrl { get; set; }

        /// <summary>
        ///     Gets or sets the wt view URL.
        /// </summary>
        /// <value>The wt view URL.</value>
        public string EntrustViewUrl { get; set; }

        public static GenerationResult Failed(string resultMessage)
        {
            return new GenerationResult
            {
                Result = false,
                ResultMessage = resultMessage
            };
        }
    }

    /// <summary>
    ///     产品信息及其订单信息
    /// </summary>
    public class ProductOrderInfo
    {
        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public IEnumerable<OrderInfo> Order { get; set; }

        /// <summary>
        ///     Gets or sets the product information.
        /// </summary>
        /// <value>The product information.</value>
        public RegularProductInfo ProductInfo { get; set; }
    }

    /// <summary>
    ///     Class SignatureContractResult.
    /// </summary>
    public class SignatureContractResult
    {
        /// <summary>
        ///     Gets or sets the jk download URL.
        /// </summary>
        /// <value>The jk download URL.</value>
        public string DownloadUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BasicGenerationResult" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the result message.
        /// </summary>
        /// <value>The result message.</value>
        public string ResultMessage { get; set; }

        /// <summary>
        ///     Gets or sets the jk view URL.
        /// </summary>
        /// <value>The jk view URL.</value>
        public string ViewUrl { get; set; }
    }

    /// <summary>
    ///     签署结果异步通知请求
    /// </summary>
    /// <seealso cref="BasicRquest" />
    public class SignResultNotifyRequest
    {
        /// <summary>
        ///     合同编号
        /// </summary>
        /// <value>The contract_ identifier.</value>
        [JsonProperty("contract_id")]
        public string Contract_Id { get; set; }

        /// <summary>
        ///     合同下载地址
        /// </summary>
        /// <value>The download_url.</value>
        [JsonProperty("download_url")]
        public string Download_Url { get; set; }

        /// <summary>
        ///     消息摘要
        /// </summary>
        /// <value>The MSG_ digest.</value>
        [JsonProperty("msg_digest")]
        [Required]
        public string Msg_Digest { get; set; }

        /// <summary>
        ///     签章结果 3000=> 签章成功  3001=>签章失败
        /// </summary>
        /// <value>The result_ code.</value>
        [JsonProperty("result_code")]
        public int Result_Code { get; set; }

        /// <summary>
        ///     签章结果描述
        /// </summary>
        /// <value>The result_ desc.</value>
        [JsonProperty("result_desc")]
        public string Result_Desc { get; set; }

        /// <summary>
        ///     请求时间
        /// </summary>
        /// <value>The timestamp.</value>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///     交易号
        /// </summary>
        /// <value>The transaction_ identifier.</value>
        [JsonProperty("transaction_id")]
        public string Transaction_Id { get; set; }

        /// <summary>
        ///     合同查看地址
        /// </summary>
        /// <value>The viewpdf_url.</value>
        [JsonProperty("viewpdf_url")]
        public string ViewPdf_Url { get; set; }
    }

    /// <summary>
    ///     签署状态请求输出参数
    /// </summary>
    /// <seealso cref="jinyinmao.Signature.Service.BasicResponse" />
    public class SignStatusResponse : BasicResponse
    {
        /// <summary>
        ///     合同下载地址
        /// </summary>
        /// <value>The download_url.</value>
        [JsonProperty("download_url")]
        public string Download_Url { get; set; }

        /// <summary>
        ///     签署状态码 0 代签 ,1 已签
        /// </summary>
        /// <value>The sign_ status.</value>
        public int Sign_Status { get; set; }

        /// <summary>
        ///     签署状态说明  代签  已签
        /// </summary>
        /// <value>The sign_ status_ desc.</value>
        public string Sign_Status_Desc { get; set; }

        /// <summary>
        ///     交易号
        /// </summary>
        /// <value>The transaction_ identifier.</value>
        public string Transaction_Id { get; set; }

        /// <summary>
        ///     合同查看地址
        /// </summary>
        /// <value>The viewpdf_url.</value>
        [JsonProperty("viewpdf_url")]
        public string ViewPdf_Url { get; set; }
    }

    /// <summary>
    ///     上传文档请求信息
    /// </summary>
    /// <seealso cref="jinyinmao.Signature.Service.BasicRquest" />
    public class TransferContractRequest : BasicRquest
    {
        /// <summary>
        ///     合同编号 通过合同编号指定在那份文档上签署
        /// </summary>
        /// <value>The contract_ identifier.</value>
        [JsonProperty("contract_id")]
        [Required]
        [StringLength(32, ErrorMessage = "只允许长度<=32的英文或者数字字符")]
        public string Contract_Id { get; set; }

        /// <summary>
        ///     Gets or sets the doc_ title.
        /// </summary>
        /// <value>The doc_ title.</value>
        [JsonProperty("doc_title")]
        public string Doc_Title { get; set; }

        /// <summary>
        ///     Gets or sets the type of the doc_.
        /// </summary>
        /// <value>The type of the doc_.</value>
        [JsonProperty("doc_type")]
        public string Doc_Type { get; set; }

        /// <summary>
        ///     Gets or sets the doc_ URL.
        /// </summary>
        /// <value>The doc_ URL.</value>
        [JsonProperty("doc_url")]
        public string Doc_Url { get; set; }

        /// <summary>
        ///     Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        [JsonProperty("file")]
        public string File { get; set; }
    }

    /// <summary>
    ///     上传合同输出参数
    /// </summary>
    /// <seealso cref="jinyinmao.Signature.Service.BasicResponse" />
    public class TransferContractResponse : BasicResponse
    {
    }
}