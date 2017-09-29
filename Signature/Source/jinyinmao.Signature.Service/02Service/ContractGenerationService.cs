using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using jinyinmao.Signature.lib;
using jinyinmao.Signature.lib.Helper;
using Moe.Lib;
using Newtonsoft.Json;

namespace jinyinmao.Signature.Service
{
    public partial class FddService
    {
        /// <summary>
        ///     自动签署合同
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<SignatureContractResult> AutoSignatureConatractAsync(string contractid, string transactionid, string customerid, string clientrole, string keyword, string doctitle)
        {
            try
            {
                SignStatusResponse signatureStatus = await this.CheckContractSignStatusAsync(contractid, customerid);

                if (signatureStatus != null && signatureStatus.Code == FDDResultCode.Success && signatureStatus.Sign_Status == FDDResultCode.Signatured)
                {
                    return await Task.FromResult(new SignatureContractResult { Result = true, ResultMessage = "签署成功", DownloadUrl = signatureStatus.Download_Url, ViewUrl = signatureStatus.ViewPdf_Url });
                }

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                List<KeyValuePair<string, string>> param = await this.BuildAutoSignatureContractParameterAsync(contractid, transactionid, customerid, clientrole, keyword, doctitle, timestamp);

                string result = FddHelper.PostByWebRequest(ConfigManager.FDDBaseAddress + "extsign_auto.api", param);

                AutoSignConatractResponse response = JsonConvert.DeserializeObject<AutoSignConatractResponse>(result);

                if (response.Code == FDDResultCode.Success)
                {
                    return await Task.FromResult(new SignatureContractResult { Result = true, ResultMessage = "签署成功", DownloadUrl = response.Download_Url, ViewUrl = response.ViewPdf_Url });
                }
                return await Task.FromResult(new SignatureContractResult { Result = false, ResultMessage = response.Msg });
            }
            catch (Exception ex)
            {
                throw new Exception($"自动签署合同异常: {ex.ToJson()}");
            }
        }

        /// <summary>
        ///     06 03 合同上传消息摘要
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="contractid">The contractid.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> BuildUploadContractMsgDigest(string timestamp, string contractid)
        {
            string result = Convert.ToBase64String(Encoding.Default.GetBytes(FddHelper.Sha1(ConfigManager.FDDAppID + FddHelper.Md5(timestamp) + FddHelper.Sha1(ConfigManager.FDDAppSecrect + contractid))));

            return await Task.FromResult(result);
        }

        #region 检查合同签署状态

        /// <summary>
        ///     构建检查合同签署状态参数
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="customerid">The customerid.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>Task&lt;List&lt;KeyValuePair&lt;System.String, System.String&gt;&gt;&gt;.</returns>
        private async Task<List<KeyValuePair<string, string>>> BuildCheckContactSignStatusParameterAsync(string contractid, string customerid, string timestamp)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("app_id", ConfigManager.FDDAppID),
                new KeyValuePair<string, string>("timestamp", timestamp),
                new KeyValuePair<string, string>("contract_id", contractid),
                new KeyValuePair<string, string>("customer_id", customerid),
                new KeyValuePair<string, string>("msg_digest", await this.BuildSignStatusMsgDigestAsync(timestamp, contractid, customerid))
            };
        }

        /// <summary>
        ///     检查合同签署状态
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="customerid">The customerid.</param>
        /// <returns>Task&lt;SignStatusResponse&gt;.</returns>
        private async Task<SignStatusResponse> CheckContractSignStatusAsync(string contractid, string customerid)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            List<KeyValuePair<string, string>> param = await this.BuildCheckContactSignStatusParameterAsync(contractid, customerid, timestamp);

            string result = FddHelper.PostByWebRequest(ConfigManager.FDDBaseAddress + "query_signstatus.api", param);

            SignStatusResponse response = JsonConvert.DeserializeObject<SignStatusResponse>(result);

            if (response.Code == FDDResultCode.Success)
            {
                return await Task.FromResult(response);
            }
            LogHelper.WriteLog($"检查合同签署状态失败{response.Msg}", "CheckContractSignStatusAsync");
            return await Task.FromResult<SignStatusResponse>(null);
        }

        #endregion 检查合同签署状态

        #region 合同归档

        /// <summary>
        ///     合同归档请求
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<BasicResult<string>> ContractArchiveAsync(string contractid)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("app_id", ConfigManager.FDDAppID),
                    new KeyValuePair<string, string>("timestamp", timestamp),
                    new KeyValuePair<string, string>("contract_id", contractid),
                    new KeyValuePair<string, string>("msg_digest", await this.BuildGenerationContractMsgDigestAsync(contractid, timestamp))
                };

                string result = FddHelper.PostByWebRequest(ConfigManager.FDDBaseAddress + "contractFiling.api", param);

                ContractArchiveResponse response = JsonConvert.DeserializeObject<ContractArchiveResponse>(result);

                if (response.Code == FDDResultCode.Success)
                {
                    return BasicResult<string>.Success("合同归档成功", "成功");
                }
                return BasicResult<string>.Failed("{response.Msg}", "失败");
            }
            catch (Exception ex)
            {
                throw new Exception($"合同归档异常: {ex.ToJson()}");
            }
        }

        /// <summary>
        ///     构造合同归档参数
        /// </summary>
        /// <param name="constractid">The constractid.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> BuildGenerationContractMsgDigestAsync(string constractid, string timestamp)
        {
            string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(FddHelper.Sha1(ConfigManager.FDDAppID + FddHelper.Md5(timestamp) + FddHelper.Sha1(ConfigManager.FDDAppSecrect + constractid))));

            return await Task.FromResult(result);
        }

        #endregion 合同归档

        #region 申请CA证书

        /// <summary>
        ///     申请CA证书
        /// </summary>
        /// <param name="credentialNo"></param>
        /// <param name="cellPhone">The cellphone.</param>
        /// <param name="realName"></param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        public async Task<CACretificateResult> ApplyCaCretificateAsync(string realName, string credentialNo, string cellPhone)
        {
            try
            {
                ////申请CA证书
                CACretificate caCretificate = JsonConvert.DeserializeObject<CACretificate>(RedisHelper.GetStringValue($"FDD:CACretificate:{cellPhone}"));

                if (!string.IsNullOrEmpty(caCretificate?.CustomerId))
                {
                    return await Task.FromResult(new CACretificateResult { Result = true, CustomerId = caCretificate.CustomerId, Email = caCretificate.Email });
                }

                CACretificateTableEntity caCretificateEntity = ConfigManager.AzureCloudTableClient.ReadFromCloudTable<CACretificateTableEntity>(ConfigManager.CustomerCACretificateTable, "cellphone", cellPhone);

                if (!string.IsNullOrEmpty(caCretificateEntity?.CustomerId))
                {
                    RedisHelper.SetStringValue($"FDD:CACretificate:{cellPhone}", new CACretificate { Cellphone = cellPhone, CustomerId = caCretificateEntity.CustomerId, Email = caCretificateEntity.Email }.ToJson());

                    return await Task.FromResult(new CACretificateResult { Result = true, CustomerId = caCretificateEntity.CustomerId, Email = caCretificateEntity.Email });
                }

                CACretificateResult caCretificateResult = await this.ApplyCaCretificateFromFddAsync(realName, credentialNo, cellPhone);

                if (!caCretificateResult.Result)
                {
                    return await Task.FromResult(new CACretificateResult { Result = false, ResponseMsg = caCretificateResult.ResponseMsg });
                }
                return await Task.FromResult(new CACretificateResult { Result = true, CustomerId = caCretificateResult.CustomerId, Email = caCretificateResult.Email });
            }
            catch (Exception ex)
            {
                throw new Exception($" 申请CA证书异常: {ex.ToJson()}");
            }
        }

        /// <summary>
        ///     申请个人AC证书
        /// </summary>
        /// <param name="customername">The customername.</param>
        /// <param name="credentialNo">The card.</param>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<CACretificateResult> ApplyCaCretificateFromFddAsync(string customername, string credentialNo, string cellphone)
        {
            credentialNo = "620521198910295671"; //todo 身份证需要修改

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string email = this.GetCellphoneEmail(cellphone);
            string id_mobile = FddHelper.Encrypt3Des($"{credentialNo}|{cellphone}", ConfigManager.FDDAppSecrect);
            string msg_digest = this.BuildCACretificateApplyMsgDigest(timestamp);
            List<KeyValuePair<string, string>> param = await this.BuildCACretificateParameterAsync(customername, timestamp, id_mobile, email, msg_digest);

            string result = FddHelper.PostByWebRequest($"{ConfigManager.FDDBaseAddress}syncPerson_auto.api", param);

            CACretificateApplyResponse response = JsonConvert.DeserializeObject<CACretificateApplyResponse>(result);

            if (response.Code == FDDResultCode.Success)
            {
                RedisHelper.SetStringValue($"FDD:CACretificate:{cellphone}", new CACretificate { Cellphone = cellphone, CustomerId = response.Customer_Id, Email = email }.ToJson());

                ConfigManager.AzureCloudTableClient.SaveToCloudTable(ConfigManager.CustomerCACretificateTable, new CACretificateTableEntity { RowKey = cellphone, Cellphone = cellphone, CustomerId = response.Customer_Id, Email = email }, "FDD-CACretificate");

                return await Task.FromResult(new CACretificateResult { Result = true, CustomerId = response.Customer_Id, Email = email });
            }
            return await Task.FromResult(new CACretificateResult { Result = false, ResponseMsg = response.Msg });
        }

        /// <summary>
        ///     个人CA证书消息摘要
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>System.String.</returns>
        private string BuildCACretificateApplyMsgDigest(string timestamp)
        {
            return HttpUtility.UrlEncode(Convert.ToBase64String(Encoding.Default.GetBytes(FddHelper.Sha1(ConfigManager.FDDAppID + FddHelper.Md5(timestamp) + FddHelper.Sha1(ConfigManager.FDDAppSecrect)))));
        }

        /// <summary>
        ///     构建个人AC证书申请参数
        /// </summary>
        /// <param name="customername">The customername.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="id_mobile">The id_mobile.</param>
        /// <param name="email">The email.</param>
        /// <param name="msg_digest">The msg_digest.</param>
        /// <returns>Task&lt;List&lt;KeyValuePair&lt;System.String, System.String&gt;&gt;&gt;.</returns>
        private async Task<List<KeyValuePair<string, string>>> BuildCACretificateParameterAsync(string customername, string timestamp, string id_mobile, string email, string msg_digest)
        {
            return await Task.FromResult(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("app_id", ConfigManager.FDDAppID),
                new KeyValuePair<string, string>("timestamp", timestamp),
                new KeyValuePair<string, string>("customer_name", customername),
                new KeyValuePair<string, string>("id_mobile", id_mobile),
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("msg_digest", msg_digest)
            });
        }

        #endregion 申请CA证书
    }
}