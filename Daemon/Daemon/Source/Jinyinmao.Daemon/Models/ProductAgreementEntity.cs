using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    ///     返回结果基类
    /// </summary>
    public class BasicResponse
    {
        /// <summary>
        ///     1    操作成功
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
        ///     请求时间  yyyyMMddHHmmss
        /// </summary>
        /// <value>The timestamp.</value>
        [JsonProperty("timestamp")]
        [Required]
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///     版本号 默认为2.0
        /// </summary>
        /// <value>The v.</value>
        [JsonProperty("v")]
        public string V { get; set; } = "2.0";
    }

    /// <summary>
    ///     合同生成请求参数
    /// </summary>
    public class ContractGenerationRequest : BasicRquest
    {
        /// <summary>
        ///     合同编号
        /// </summary>
        /// <value>The contract_ identifier.</value>
        [JsonProperty("contract_id")]
        [Required]
        [StringLength(32, ErrorMessage = "只允许长度<=32的英文或者数字字符")]
        public string Contract_Id { get; set; }

        /// <summary>
        ///     文档标题
        /// </summary>
        /// <value>The doc_ title.</value>
        [JsonProperty("doc_title")]
        [Required]
        public string Doc_Title { get; set; }

        /// <summary>
        ///     动态表格
        /// </summary>
        /// <value>The dynamic_ tables.</value>
        [JsonProperty("dynamic_tables")]
        public string Dynamic_Tables { get; set; }

        /// <summary>
        ///     字体大小 可选 不传则默认为 9
        /// </summary>
        /// <value>The size of the font_.</value>
        [JsonProperty("font_size")]
        public int Font_Size { get; set; }

        /// <summary>
        ///     字体类型  0-宋体 1-仿宋 2-黑体 3-楷体 4-微软雅黑
        /// </summary>
        /// <value>The type of the font_.</value>
        [JsonProperty("font_type")]
        [Range(0, 4)]
        public int Font_Type { get; set; }

        /// <summary>
        ///     消息摘要 加密
        /// </summary>
        /// <value>The dynamic_ tables.</value>
        [JsonProperty("msg_digest")]
        [Required]
        public string Msg_Digest { get; set; }

        /// <summary>
        ///     填充内容 json字符串 key为文本域 value 为值  eg:{"Name":"小明","platform":"金银猫"}
        /// </summary>
        /// <value>The parameter_ map.</value>
        [JsonProperty("parameter_map")]
        [Required]
        public string Parameter_Map { get; set; }

        /// <summary>
        ///     模板编号
        /// </summary>
        /// <value>The template_ identifier.</value>
        [JsonProperty("template_id")]
        [Required]
        [StringLength(32, ErrorMessage = "只允许长度<=32的英文或者数字字符")]
        public string Template_Id { get; set; }
    }

    /// <summary>
    ///     合同生成输出参数
    /// </summary>
    /// <seealso cref="Jinyinmao.Daemon.Models.BasicResponse" />
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
    ///     合同模板传输请求参数
    /// </summary>
    public class ContractTemplateTransferRequest
    {
        /// <summary>
        ///     文档地址 类型须为URL  doc_url 和 file 必选其一
        /// </summary>
        /// <value>The doc_ URL.</value>
        [JsonProperty("doc_url")]
        public string Doc_Url { get; set; }

        /// <summary>
        ///     PDF模板  类型为文件流 doc_url 和 file 必选其一
        /// </summary>
        /// <value>The doc_ URL.</value>
        [JsonProperty("file")]
        public FileStream File { get; set; }

        /// <summary>
        ///     消息摘要
        /// </summary>
        /// <value>The MSG_ digest.</value>
        [JsonProperty("msg_digest")]
        [Required]
        public string Msg_Digest { get; set; }

        /// <summary>
        ///     模板编号
        /// </summary>
        /// <value>The template_ identifier.</value>
        [JsonProperty("template_id")]
        [Required]
        [StringLength(32, ErrorMessage = "只允许长度<=32的英文或者数字字符")]
        public string Template_Id { get; set; }
    }

    /// <summary>
    ///     合同模板传输输出参数
    /// </summary>
    public class ContractTemplateTransferResponse : BasicResponse
    {
    }
}