using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using jinyinmao.Signature.lib;
using jinyinmao.Signature.lib.Common;
using jinyinmao.Signature.lib.Helper;
using jinyinmao.Signature.Service.Model;
using Moe.Lib;

namespace jinyinmao.Signature.Service
{
    public class YemService
    {
        private readonly FddService fddService;

        private readonly TirisfalService tirisfalService;

        public YemService()
        {
            this.tirisfalService = new TirisfalService();
            this.fddService = new FddService();
        }

        /// <summary>
        ///     生成余额猫订单协议
        /// </summary>
        /// <param name="transactions">The transactions.</param>
        /// <returns>Task&lt;List&lt;YEMAgreementInfo&gt;&gt;.</returns>
        public async Task<List<AgreementInfo>> GenerationYemContractAsync(IEnumerable<YEMOrderInfo> transactions)
        {
            List<AgreementInfo> resultList = new List<AgreementInfo>();
            try
            {
                IEnumerable<IGrouping<string, YEMOrderInfo>> transGroupByUserID = transactions.GroupBy(trans => trans.UserId);

                foreach (IGrouping<string, YEMOrderInfo> groupOrder in transGroupByUserID)
                {
                    //获取用户信息
                    YEMOrderUserInfo userInfo = await this.tirisfalService.GetYemUserInfoAsync(groupOrder.Key);

                    LogHelper.WriteLog($"用户 {userInfo.UserId} 开始生成余额猫PDF协议", userInfo.UserId);

                    foreach (YEMOrderInfo order in groupOrder)
                    {
                        GenerationResult result = await this.GenerationYemSigleOrderContractAsync(userInfo, order);
                        if (!result.Result)
                        {
                            LogHelper.WriteLog($"用户 {userInfo.UserId} 订单{order.OrderId} 生成余额猫PDF协议失败,失败原因:{result.ResultMessage}", userInfo.UserId);
                            continue;
                        }

                        AgreementInfoEntity info = new AgreementInfoEntity
                        {
                            FDDAccountId = result.Email,
                            OrderId = order.OrderId,
                            RowKey = order.OrderId,
                            UserId = order.UserId,
                            EntrustAgreementViewUrl = result.EntrustViewUrl,
                            EntrustAgreementDownUrl = result.EntrustDownloadUrl,
                            BorrowAgreementViewUrl = result.BorrowViewUrl,
                            BorrowAgreementDownUrl = result.BorrowDownloadUrl,
                            EntrustContractId = result.EntrustContractId,
                            BorrowContractId = result.BorrowContractId
                        };

                        ConfigManager.AzureCloudTableClient.SaveToCloudTable("CustomerYemAgreementInfo", info, DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMM"));

                        resultList.Add(new AgreementInfo
                        {
                            FDDAccountId = result.Email,
                            UserIndentifier = order.UserId,
                            OrderIndentifier = order.OrderId
                        });
                    }
                    LogHelper.WriteLog($" 用户 {userInfo.UserId} 生成余额猫PDF协议成功", userInfo.UserId);
                }

                if (await this.tirisfalService.NotifyTirisfalYemAgreementAsync(resultList))
                {
                    return await Task.FromResult(resultList);
                }
                return await Task.FromResult(resultList);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "GenerationYEMContractAsync");
                throw new Exception("余额猫生成PDF出错", ex);
            }
        }

        /// <summary>
        ///     两小时执行一次 可配置
        /// </summary>
        public async void SignatureYemAgreement()
        {
            LogHelper.WriteLog("开始生成余额猫协议...", "DoWork");
            try
            {
                //查询需要电子签章的订单列表
                List<YEMOrderInfo> yemAssetRecordList = JYMDBHelper.GetYemTransactionList();

                LogHelper.WriteLog($"共有{yemAssetRecordList.Count}个订单需要生成电子签章", "DoWork");

#if DEBUG
                yemAssetRecordList.Clear();
                yemAssetRecordList.Add(new YEMOrderInfo
                {
                    OrderId = "C471C29E3BB34CC08300071C340FFC73",
                    UserId = "F39800ADEC1446C682A1015E3CDF0ECC",
                    OrderType = 0,
                    ResultTime = DateTime.Now
                });
                yemAssetRecordList.Add(new YEMOrderInfo
                {
                    OrderId = "E471C29E3BB34CC08300071C340FFC73",
                    UserId = "D39800ADEC1446C682A1015E3CDF0ECC",
                    OrderType = 0,
                    ResultTime = DateTime.Now.AddDays(1)
                });
                string s = yemAssetRecordList.ToJson();

#endif

                List<AgreementInfo> generationResult = await this.GenerationYemContractAsync(yemAssetRecordList);

                LogHelper.WriteLog($"余额猫生成PDF文档成功,应生成 {yemAssetRecordList.Count()} 个,实际生成 {generationResult.Count} 个", "YEM");

                if (ConfigManager.IsClearFile)
                {
                    this.fddService.ClearContractFile();
                }
                LogHelper.WriteLog("余额猫协议生成完成...", "DoWork");
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "DoWork");
            }
        }

        /// <summary>
        ///     构造余额猫协议填充内容
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <param name="orderInfo">The order.</param>
        /// <returns>Task&lt;Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        /// <exception cref="System.Exception"></exception>
        private async Task<Dictionary<string, string>> BuildYemContractContentAsync(YEMOrderUserInfo userInfo, YEMOrderInfo orderInfo)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            try
            {
                //委托协议
                content.Add("ClientID", userInfo.RealName);
                content.Add("LenderNO", userInfo.CredentialNo.ConfusionCredentialNo());
                content.Add("LenderJYMName", userInfo.Cellphone.ConfusionCellphone());
                content.Add("SignDate", orderInfo.ResultTime.ToString("yyyy年MM月dd日"));

                //借款协议
                content.Add("Lender", orderInfo.ResultTime.ToString("yyyy年MM月dd日"));

                return await Task.FromResult(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"LocalGenerationContractPdfFileAsync {ex.Message}");
            }
        }

        /// <summary>
        ///     余额猫生成单个协议法大大协议
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <param name="order">The order.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;GenerationResult&gt;.</returns>
        private async Task<GenerationResult> GenerationYemSigleAgreementContractAsync(YEMOrderUserInfo userInfo, YEMOrderInfo order, int agreementIndex, Dictionary<string, string> content)
        {
            if (agreementIndex >= 2)
            {
                return GenerationResult.Failed("协议类型错误");
            }

            string contractId = Guid.NewGuid().ToGuidString();

            //模板路径
            BasicResult<string> templateResult = await this.GetYemContractTemplateFilePathAsync(agreementIndex);

            if (!templateResult.Result)
            {
                return GenerationResult.Failed(templateResult.Message);
            }

            //生成合同PDF文件
            Tuple<bool, string> createContractResult = await this.fddService.CreateContractFileByTemplateAsync(templateResult.Data, this.fddService.contractFilePath, contractId, content, "YEM");
            if (!createContractResult.Item1)
            {
                return GenerationResult.Failed("生成余额猫合同PDF文件失败");
            }

            LogHelper.WriteLog($"用户 {userInfo.UserId} 余额猫订单 {order.OrderId} 生成本地PDF文件成功", order.OrderId);

            //上传合同至法大大
            TransferContractResponse uploadContractResult = await this.fddService.UploadContractToFddAsync(createContractResult.Item2, contractId, this.fddService.GetContractDocTitle(agreementIndex, order.OrderType));
            if (uploadContractResult == null || uploadContractResult.Code != FDDResultCode.Success)
            {
                return GenerationResult.Failed(uploadContractResult?.Msg);
            }

            LogHelper.WriteLog($"用户 {userInfo.UserId} 余额猫订单 {order.OrderId} 上传PDF文件到法大大成功", order.OrderId);
            //申请CA证书
            CACretificateResult applyCaCretificateResult = await this.fddService.ApplyCaCretificateAsync(userInfo.RealName, userInfo.CredentialNo, userInfo.Cellphone);

            if (!applyCaCretificateResult.Result)
            {
                return GenerationResult.Failed($"申请CA证书失败,失败原因:{applyCaCretificateResult.ResponseMsg}");
            }
            LogHelper.WriteLog($"用户 {userInfo.UserId} 余额猫订单 {order.OrderId} 申请CA证书成功", order.OrderId);

            // 开始签署 第一次签署用户章 第二次签署公司章
            SignatureContractResult customerSignatureContractResult = await this.fddService.AutoSignatureConatractAsync(contractId, Guid.NewGuid().ToGuidString(), applyCaCretificateResult.CustomerId, ConfigManager.FDDCustomerRole, ConfigManager.FDDCustomerSignKey, this.fddService.GetContractDocTitle(0, order.OrderType)); //签署

            if (!customerSignatureContractResult.Result)
            {
                return GenerationResult.Failed($"用户签章失败,失败原因:{customerSignatureContractResult.ResultMessage}");
            }
            LogHelper.WriteLog($"用户 {userInfo.UserId} 余额猫订单 {order.OrderId} 受让人签章成功", order.OrderId);

            SignatureContractResult companySignatureContractResult = await this.fddService.AutoSignatureConatractAsync(contractId, Guid.NewGuid().ToGuidString(), ConfigManager.FDDCompanyID, ConfigManager.FDDCompanyRole, ConfigManager.FDDCompanySignKey, this.fddService.GetContractDocTitle(0, order.OrderType)); //签署
            if (!companySignatureContractResult.Result)
            {
                return GenerationResult.Failed($"平台签章失败,失败原因:{customerSignatureContractResult.ResultMessage}");
            }
            LogHelper.WriteLog($"用户 {userInfo.UserId} 余额猫订单 {order.OrderId} 平台签章成功", order.OrderId);
            //归档
            BasicResult<string> contractArchiveResult = await this.fddService.ContractArchiveAsync(contractId);
            if (!contractArchiveResult.Result)
            {
                return GenerationResult.Failed($"合同归档失败,失败原因:{contractArchiveResult.Message}");
            }
            switch (agreementIndex)
            {
                case 0:
                    return new GenerationResult
                    {
                        EntrustViewUrl = companySignatureContractResult.ViewUrl,
                        EntrustDownloadUrl = companySignatureContractResult.DownloadUrl,
                        EntrustContractId = contractId,
                        Email = applyCaCretificateResult.Email,
                        Result = true,
                        ResultMessage = "成功"
                    };

                case 1:
                    return new GenerationResult
                    {
                        BorrowViewUrl = companySignatureContractResult.ViewUrl,
                        BorrowDownloadUrl = companySignatureContractResult.DownloadUrl,
                        BorrowContractId = contractId,
                        Email = applyCaCretificateResult.Email,
                        Result = true,
                        ResultMessage = "成功"
                    };

                default:
                    return GenerationResult.Failed("未知类型错误,请联系管理员");
            }
        }

        /// <summary>
        ///     余额猫生成单个订单法大大协议
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <param name="order">The order.</param>
        /// <returns>Task&lt;GenerationResult&gt;.</returns>
        private async Task<GenerationResult> GenerationYemSigleOrderContractAsync(YEMOrderUserInfo userInfo, YEMOrderInfo order)
        {
            //模板填充内容
            Dictionary<string, string> content = await this.BuildYemContractContentAsync(userInfo, order);

            GenerationResult entrustResult = await this.GenerationYemSigleAgreementContractAsync(userInfo, order, Constants.EntrustAgreementIndex, content);
            if (!entrustResult.Result)
            {
                return GenerationResult.Failed(entrustResult.ResultMessage);
            }
            GenerationResult borrowResult = await this.GenerationYemSigleAgreementContractAsync(userInfo, order, Constants.BorrowAgreementIndex, content);

            if (!borrowResult.Result)
            {
                return GenerationResult.Failed(borrowResult.ResultMessage);
            }
            return new GenerationResult
            {
                EntrustDownloadUrl = entrustResult.EntrustDownloadUrl,
                EntrustViewUrl = entrustResult.EntrustViewUrl,
                EntrustContractId = entrustResult.EntrustContractId,
                BorrowViewUrl = borrowResult.BorrowViewUrl,
                BorrowDownloadUrl = borrowResult.BorrowDownloadUrl,
                BorrowContractId = borrowResult.BorrowContractId,
                Email = entrustResult.Email,
                Result = true,
                ResultMessage = "成功"
            };
        }

        /// <summary>
        ///     获取协议模板地址
        /// </summary>
        /// <param name="orderType">Type of the order.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<BasicResult<string>> GetYemContractTemplateFilePathAsync(int orderType)
        {
            string templatePath = this.fddService.templateFilePath + this.fddService.GetTemplateFilePath(0, orderType);

            if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
            {
                return await Task.FromResult(BasicResult<string>.Failed("模板文件不存在", string.Empty));
            }
            return await Task.FromResult(BasicResult<string>.Success("成功", templatePath));
        }
    }
}