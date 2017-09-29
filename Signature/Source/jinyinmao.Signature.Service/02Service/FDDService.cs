using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using jinyinmao.Signature.lib;
using Newtonsoft.Json;

namespace jinyinmao.Signature.Service
{
    public partial class FddService
    {
        public FddService()
        {
            this.templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"pdf\templatefile\");
            this.contractFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"pdf\contractfile\");
            this.dirInfo = new DirectoryInfo(this.contractFilePath);
            this.fontSize = ConfigManager.FDDFontSize;
        }

        /// <summary>
        ///     通过模板生成合同
        /// </summary>
        /// <param name="templatefilepath">The templatefilepath.</param>
        /// <param name="contractfilepath">The contractfilepath.</param>
        /// <param name="contractid">The contractid.</param>
        /// <param name="content">The content.</param>
        /// <param name="productId"></param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        public async Task<Tuple<bool, string>> CreateContractFileByTemplateAsync(string templatefilepath, string contractfilepath, string contractid, Dictionary<string, string> content, string productId = "")
        {
            if (!Directory.Exists(contractfilepath))
            {
                Directory.CreateDirectory(contractfilepath);
            }
            contractfilepath = Path.Combine(contractfilepath, productId + "\\");
            if (!string.IsNullOrEmpty(productId) && !Directory.Exists(Path.Combine(contractfilepath, productId)))
            {
                Directory.CreateDirectory(contractfilepath);
            }
            BaseFont.AddToResourceSearch(Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "/iTextAsian.dll"));
            BaseFont.AddToResourceSearch(Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "/iTextAsianCmaps.dll"));

            this.baseFont = BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", BaseFont.EMBEDDED);
            string contractPath = contractfilepath + contractid + ".pdf";
            PdfReader reader = null;
            PdfStamper stamper = null;
            try
            {
                reader = new PdfReader(templatefilepath);

                stamper = new PdfStamper(reader, new FileStream(contractPath, FileMode.Create));

                AcroFields fields = stamper.AcroFields;

                foreach (KeyValuePair<string, string> con in content)
                {
                    fields.SetFieldProperty(con.Key, "textfont", this.baseFont, null);
                    fields.SetFieldProperty(con.Key, "textsize", this.fontSize, null);
                    fields.SetField(con.Key, con.Value);
                }
                stamper.FormFlattening = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stamper?.Close();
                reader?.Close();
            }
            return await Task.FromResult(Tuple.Create(true, contractPath));
        }

        /// <summary>
        ///     上传合同到法大大
        /// </summary>
        /// <param name="contractpath">The contractpath.</param>
        /// <param name="contractid">The contractid.</param>
        /// <param name="doctitle">The doctitle.</param>
        /// <returns>Task&lt;TransferContractResponse&gt;.</returns>
        public async Task<TransferContractResponse> UploadContractToFddAsync(string contractpath, string contractid, string doctitle)
        {
            try
            {
                if (!File.Exists(contractpath))
                {
                    return await Task.FromResult<TransferContractResponse>(null);
                }
                string appID = ConfigManager.FDDAppID;
                string url = ConfigManager.FDDBaseAddress;
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string doctytpe = ".pdf";
                string msgdigest = await this.BuildUploadContractMsgDigest(timestamp, contractid);

                string responseText;

                using (FileStream file = new FileStream(contractpath, FileMode.Open, FileAccess.ReadWrite))
                {
                    byte[] filebytes = new byte[file.Length];
                    file.Read(filebytes, 0, filebytes.Length);

                    FddHelper help = new FddHelper();
                    help.SetFieldValue("app_id", appID);
                    help.SetFieldValue("timestamp", timestamp);
                    help.SetFieldValue("contract_id", contractid);
                    help.SetFieldValue("doc_title", doctitle);
                    help.SetFieldValue("doc_type", doctytpe);
                    help.SetFieldValue("msg_digest", msgdigest);
                    help.SetFieldValue("file", contractid + ".pdf", "application/octet-stream", filebytes);
                    help.HttpPost(url + "uploaddocs.api", out responseText);

                    file.Close();
                }

                TransferContractResponse uploadResult = JsonConvert.DeserializeObject<TransferContractResponse>(responseText);

                if (uploadResult.Code == FDDResultCode.Success)
                {
                    return await Task.FromResult(uploadResult);
                }
                return await Task.FromResult(uploadResult);
            }
            // ReSharper disable once RedundantCatchClause
            catch (Exception)
            {
                throw;
            }
        }

        #region 自动签署合同辅助方法

        /// <summary>
        ///     构建自动签署参数
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="transactionid">The transactionid.</param>
        /// <param name="customerid">The customerid.</param>
        /// <param name="clientrole">The clientrole.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="doctitle">The doctitle.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>Task&lt;List&lt;KeyValuePair&lt;System.String, System.String&gt;&gt;&gt;.</returns>
        private async Task<List<KeyValuePair<string, string>>> BuildAutoSignatureContractParameterAsync(string contractid, string transactionid, string customerid, string clientrole, string keyword, string doctitle, string timestamp)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("app_id", ConfigManager.FDDAppID),
                new KeyValuePair<string, string>("timestamp", timestamp),
                new KeyValuePair<string, string>("transaction_id", transactionid),
                new KeyValuePair<string, string>("contract_id", contractid),
                new KeyValuePair<string, string>("customer_id", customerid),
                new KeyValuePair<string, string>("client_role", clientrole),
                new KeyValuePair<string, string>("doc_title", doctitle),
                new KeyValuePair<string, string>("sign_keyword", keyword),
                new KeyValuePair<string, string>("notify_url", ""),
                new KeyValuePair<string, string>("msg_digest", await this.BuildAutoSignConatractMsgDigestAsync(transactionid, timestamp, customerid))
            };
        }

        /// <summary>
        ///     自动签署消息摘要
        /// </summary>
        /// <param name="transactionid">The transactionid.</param>
        /// <param name="timestamp">The timestamo.</param>
        /// <param name="customerid">The cuetomerid.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> BuildAutoSignConatractMsgDigestAsync(string transactionid, string timestamp, string customerid)
        {
            string transactionIdMD5 = FddHelper.Md5(transactionid + timestamp);
            string appSecretSHA1 = FddHelper.Sha1(ConfigManager.FDDAppSecrect + customerid);
            string appIdSHA1 = FddHelper.Sha1(ConfigManager.FDDAppID + transactionIdMD5 + appSecretSHA1);
            string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(appIdSHA1));

            return await Task.FromResult(result);
        }

        /// <summary>
        ///     合同签署消息摘要
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="contractid">The contractid.</param>
        /// <param name="customerid">The customerid.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> BuildSignStatusMsgDigestAsync(string timestamp, string contractid, string customerid)
        {
            string md5timestamp = FddHelper.Md5(timestamp);

            string sha1secrate = FddHelper.Sha1(ConfigManager.FDDAppSecrect + contractid + customerid);

            string sha1all = FddHelper.Sha1(ConfigManager.FDDAppID + md5timestamp + sha1secrate);

            string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(sha1all));

            return await Task.FromResult(result);
        }

        #endregion 自动签署合同辅助方法

        #region 属性变量

        public readonly string contractFilePath;
        public readonly string templateFilePath;
        private readonly string chinaMobile = @"^1(34[0-8]|(3[5-9]|5[017-9]|8[278])\d)\d{7}$";
        private readonly string chinaTelePhone = @"^1((33|53|8[09])[0-9]|349)\d{7}$";
        private readonly string chinaUnion = @"^1(3[0-2]|5[256]|8[56])\d{8}$";
        private readonly float fontSize;
        private BaseFont baseFont;
        private DirectoryInfo dirInfo;

        #endregion 属性变量

        #region 99公用方法

        /// <summary>
        ///     清理生成的合同文件
        /// </summary>
        public void ClearContractFile()
        {
            if (this.dirInfo == null)
            {
                this.dirInfo = new DirectoryInfo(this.contractFilePath);
            }

            if (this.dirInfo.Exists)
            {
                this.dirInfo.Delete(true);
            }
            this.dirInfo.Create();
        }

        public string GetContractDocTitle(long productCategory, int agrenmentindex)
        {
            switch (productCategory)
            {
                //余额猫
                case 0:
                    if (agrenmentindex == 0) //赎回
                    {
                        return ContractDocTitleHelper.YEMAutomatedTradingTitle;
                    }
                    return ContractDocTitleHelper.YEMInvestmentAgreementTitle; //申购

                ////普惠众赢
                //case 100000040:
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.PhzyEntrustedAgreement;
                //    return TemplateFileHelper.PhzyCreditorRightsTransfer;

                //商票贷 普通银票
                case 100000020:
                case 100000010:
                    if (agrenmentindex == 0)
                    {
                        return ContractDocTitleHelper.EntrustedAgreementTitle;
                    }
                    return ContractDocTitleHelper.LoanAgreementTitle;

                case 100000021: //担保贷
                    if (agrenmentindex == 0)
                    {
                        return ContractDocTitleHelper.EntrustedAgreementTitle;
                    }
                    return ContractDocTitleHelper.GuaranteeLoanAgreementTitle;

                //case 100000022: //银保贷
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.BabillGuaranteeEntrustedAgreement;
                //    return TemplateFileHelper.BabillGuaranteeLoanAgreement;

                //case 100000023: //保理赢
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.FactoringSurplusEntrustedAgreement;
                //    return TemplateFileHelper.FactoringSurplusCreditorRightsTransfer;

                case 100000011: //银票保理
                    if (agrenmentindex == 0)
                    {
                        return ContractDocTitleHelper.BabillFactoringEntrustedAgreement;
                    }
                    return ContractDocTitleHelper.BabillFactoringCollectTransfer;

                //case 210001010: //富镇银票
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.BabillFactoringEntrustedAgreement;
                //    return TemplateFileHelper.BabillFactoringCollectTransfer;

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        ///     获取产品的协议模板 0:委托协议 1:借款协议
        /// </summary>
        /// <param name="productCategory">The product category.</param>
        /// <param name="agrenmentindex">The agrenmentindex.</param>
        /// <returns>System.String.</returns>
        public string GetTemplateFilePath(long productCategory, int agrenmentindex)
        {
            switch (productCategory)
            {
                //余额猫
                case 0:
                    if (agrenmentindex == 0) //赎回
                    {
                        return TemplateFileHelper.YEMAutomatedTrading;
                    }
                    return TemplateFileHelper.YEMInvestmentAgreement; //申购

                ////普惠众赢
                //case 100000040:
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.PhzyEntrustedAgreement;
                //    return TemplateFileHelper.PhzyCreditorRightsTransfer;

                //商融保赢
                case 100000020: //商票贷
                case 100000022:
                case 100000023:
                    if (agrenmentindex == 0)
                    {
                        return TemplateFileHelper.TabillEntrustedAgreement;
                    }
                    return TemplateFileHelper.TabillLoanAgreement;

                case 100000021: //担保贷
                    if (agrenmentindex == 0)
                    {
                        return TemplateFileHelper.GuaranteeLoanEntrustedAgreement;
                    }
                    return TemplateFileHelper.GuaranteeLoanAgreement;

                //case 100000022: //银保贷
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.BabillGuaranteeEntrustedAgreement;
                //    return TemplateFileHelper.BabillGuaranteeLoanAgreement;

                //case 100000023: //保理赢
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.FactoringSurplusEntrustedAgreement;
                //    return TemplateFileHelper.FactoringSurplusCreditorRightsTransfer;

                //银企众盈
                case 100000010: //普通银票
                    if (agrenmentindex == 0)
                    {
                        return TemplateFileHelper.OrdinaryBabillEntrustedAgreement;
                    }
                    return TemplateFileHelper.OrdinaryBabillLoanAgreement;

                case 100000011: //银票保理
                    if (agrenmentindex == 0)
                    {
                        return TemplateFileHelper.BabillFactoringEntrustedAgreement;
                    }
                    return TemplateFileHelper.BabillFactoringCollectTransfer;

                //case 210001010: //富镇银票
                //    if (agrenmentindex == 0)
                //        return TemplateFileHelper.BabillFactoringEntrustedAgreement;
                //    return TemplateFileHelper.BabillFactoringCollectTransfer;

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        ///     获取手机号码对应的默认邮箱地址
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>System.String.</returns>
        private string GetCellphoneEmail(string cellphone)
        {
            if (Regex.IsMatch(cellphone, this.chinaMobile))
            {
                return cellphone + "@139.com";
            }
            if (Regex.IsMatch(cellphone, this.chinaUnion))
            {
                return cellphone + "@wo.cn";
            }
            if (Regex.IsMatch(cellphone, this.chinaTelePhone))
            {
                return cellphone + "@189.cn";
            }
            return string.Empty;
        }

        #endregion 99公用方法

        #region 05合同下载

        private async Task<string> BuildDownLoadContractMsgDigest(string timestamp, string contractid)
        {
            string result = Convert.ToBase64String(Encoding.Default.GetBytes(FddHelper.Sha1(ConfigManager.FDDAppID + FddHelper.Md5(timestamp) + FddHelper.Sha1(ConfigManager.FDDAppSecrect + contractid))));

            return await Task.FromResult(result);
        }

        private async Task DownLoadContractAsync(string contractid)
        {
            //WebClient client = new WebClient();
            //client.DownloadFile(downloadurl, "d:/p.pdf");

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string msgdigest = await this.BuildDownLoadContractMsgDigest(timestamp, contractid);
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("app_id", ConfigManager.FDDAppID),
                new KeyValuePair<string, string>("timestamp", timestamp),
                new KeyValuePair<string, string>("contract_id", contractid),
                new KeyValuePair<string, string>("msg_digest", msgdigest)
            };

            FddHelper.PostByWebRequest($"{ConfigManager.FDDBaseAddress}downLoadContract.api", param);
        }

        #endregion 05合同下载

        #region 00模板上传

        /// <summary>
        ///     上传合同模板
        /// </summary>
        /// <param name="templatepath">The templatepath.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> UploadContractTemplateAsync(string templatepath)
        {
            string app_id = ConfigManager.FDDAppID;
            string url = ConfigManager.FDDBaseAddress;
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string templateid = Utility.GenerateNo();
            string msgdigest = await this.BuildUploadContractTemplateMsgDigest(timestamp, templateid);

            string responseText;

            using (FileStream file = new FileStream(templatepath, FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] filebytes = new byte[file.Length];
                file.Read(filebytes, 0, filebytes.Length);

                FddHelper help = new FddHelper();
                help.SetFieldValue("app_id", app_id);
                help.SetFieldValue("timestamp", timestamp);
                help.SetFieldValue("template_id", templateid);
                help.SetFieldValue("msg_digest", msgdigest);
                help.SetFieldValue("file", "fdd.pdf", "application/octet-stream", filebytes);
                help.HttpPost(url + "uploadtemplate.api", out responseText);

                file.Close();
            }

            ContractTemplateTransferResponse Upresult = JsonConvert.DeserializeObject<ContractTemplateTransferResponse>(responseText);

            if (Upresult.Code == FDDResultCode.UploadTemplateSuccess)
            {
                return await Task.FromResult(templateid);
            }
            return await Task.FromResult(string.Empty);
        }

        /// <summary>
        ///     合同模板上传消息摘要
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="templateid">The templateid.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> BuildUploadContractTemplateMsgDigest(string timestamp, string templateid)
        {
            string result = Convert.ToBase64String(Encoding.Default.GetBytes(FddHelper.Sha1(ConfigManager.FDDAppID + FddHelper.Md5(timestamp) + FddHelper.Sha1(ConfigManager.FDDAppSecrect + templateid))));

            return await Task.FromResult(result);
        }

        #endregion 00模板上传
    }
}