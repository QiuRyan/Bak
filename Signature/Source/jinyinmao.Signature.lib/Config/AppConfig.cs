using System.Collections.Generic;

namespace jinyinmao.Signature.lib
{
    /// <summary>
    ///     应用程序配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        ///     Gets or sets the biz database connection string.
        /// </summary>
        /// <value>The biz database connection string.</value>
        public string BizDBConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the biz redis connectiong string.
        /// </summary>
        /// <value>The biz redis connectiong string.</value>
        public string BizRedisConnectiongString { get; set; }

        /// <summary>
        ///     个人CA证书存储表名
        /// </summary>
        /// <value>The customer ca cretificate table.</value>
        public string CustomerCACretificateTable { get; set; }

        /// <summary>
        ///     Gets or sets the data connection string.
        /// </summary>
        /// <value>The data connection string.</value>
        public string DataConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the do work interval.
        /// </summary>
        /// <value>The do work interval.</value>
        public int DoWorkInterval { get; set; }

        /// <summary>
        ///     Gets or sets the exceptionless API key.
        /// </summary>
        /// <value>The exceptionless API key.</value>
        public string ExceptionlessApiKey { get; set; }

        /// <summary>
        ///     Gets or sets the exceptionless server URL.
        /// </summary>
        /// <value>The exceptionless server URL.</value>
        public string ExceptionlessServerUrl { get; set; }

        /// <summary>
        ///     Gets or sets the FDD application identifier.
        /// </summary>
        /// <value>The FDD application identifier.</value>
        public string FDDAppID { get; set; }

        /// <summary>
        ///     Gets or sets the FDD application secrect.
        /// </summary>
        /// <value>The FDD application secrect.</value>
        public string FDDAppSecrect { get; set; }

        /// <summary>
        ///     Gets or sets the FDD base address.
        /// </summary>
        /// <value>The FDD base address.</value>
        public string FDDBaseAddress { get; set; }

        /// <summary>
        ///     Gets or sets the FDD client identifier.
        /// </summary>
        /// <value>The FDD client identifier.</value>
        public string FDDCompanyID { get; set; }

        /// <summary>
        ///     Gets or sets the FDD client role.
        /// </summary>
        /// <value>The FDD client role.</value>
        public string FDDCompanyRole { get; set; }

        /// <summary>
        ///     Gets or sets the FDD company sign key.
        /// </summary>
        /// <value>The FDD company sign key.</value>
        public string FDDCompanySignKey { get; set; }

        /// <summary>
        ///     Gets or sets the FDD client role.
        /// </summary>
        /// <value>The FDD client role.</value>
        public string FDDCustomerRole { get; set; }

        /// <summary>
        ///     Gets or sets the FDD customer sign key.
        /// </summary>
        /// <value>The FDD customer sign key.</value>
        public string FDDCustomerSignKey { get; set; }

        /// <summary>
        ///     Gets or sets the size of the FDD font.
        /// </summary>
        /// <value>The size of the FDD font.</value>
        public float FDDFontSize { get; set; }

        /// <summary>
        ///     Gets or sets the type of the FDD font.
        /// </summary>
        /// <value>The type of the FDD font.</value>
        public int FDDFontType { get; set; }

        /// <summary>
        ///     Gets the ip whitelists.
        /// </summary>
        /// <value>The ip whitelists.</value>
        public List<string> IPWhitelists { get; set; }

        /// <summary>
        ///     Gets or sets the is clear file.
        /// </summary>
        /// <value>The is clear file.</value>
        public bool IsClearFile { get; set; }

        /// <summary>
        ///     Gets or sets the publist date.
        /// </summary>
        /// <value>The publist date.</value>
        public string PublishDate { get; set; }

        /// <summary>
        ///     定期查询可生成电子签章的sql语句
        /// </summary>
        /// <value>The regular query string.</value>
        public string RegularQueryString { get; set; }

        /// <summary>
        ///     Gets the resources.
        /// </summary>
        /// <value>
        ///     The resources.
        /// </value>
        public Dictionary<string, string> Resources { get; set; }

        /// <summary>
        ///     ServiceBus连接串
        /// </summary>
        /// <value>The service bus connection string.</value>
        public string ServiceBusConnectionString { get; set; }

        /// <summary>
        ///     交易系统角色
        /// </summary>
        public string TirisfalServiceBaseUrl { get; set; }

        /// <summary>
        ///     余额猫查询可生成电子签章的SQL语句
        /// </summary>
        /// <value>The yem query string.</value>
        public string YEMQueryString { get; set; }
    }
}