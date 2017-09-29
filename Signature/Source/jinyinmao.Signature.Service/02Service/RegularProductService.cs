using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using jinyinmao.Signature.lib;
using jinyinmao.Signature.lib.Helper;
using Moe.Lib;
using Newtonsoft.Json;

namespace jinyinmao.Signature.Service
{
    public class RegularProductService
    {
        private readonly List<AgreementInfo> agreementInfoList;
        private readonly Dictionary<string, string> contractContentDict;
        private readonly string contractFilePath;
        private readonly FddService fddService;
        private readonly string templateFilePath;
        private readonly TirisfalService tirisfalService;
        private bool isComplate;

        public RegularProductService()
        {
            this.tirisfalService = new TirisfalService();
            this.fddService = new FddService();
            this.isComplate = false;
            this.agreementInfoList = new List<AgreementInfo>();
            this.contractContentDict = new Dictionary<string, string>();
            this.templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"pdf\templatefile\");
            this.contractFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"pdf\contractfile\");
        }

        #region 生成合同

        /// <summary>
        ///     02 定期生成单个订单的协议
        /// </summary>
        /// <param name="productInfo">The product information.</param>
        /// <param name="order">The order.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task<List<AgreementInfo>> GenerationRegularProductSigleOrderAsync(RegularProductInfo productInfo, OrderInfo order)
        {
            LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {order.OrderId} 开始生成", order.OrderId);

            string entrustContractId = Guid.NewGuid().ToGuidString();
            Dictionary<string, string> entrustAgreementContent = await this.BuildContractContentAsync(entrustContractId, order, productInfo, Constants.EntrustAgreementIndex);
            GenerationResult entrustResult = await this.GenerationSigleAgreementContractAsync(productInfo, order, Constants.EntrustAgreementIndex, entrustAgreementContent, entrustContractId); //生成

            if (!entrustResult.Result)
            {
                this.isComplate = false;
                LogHelper.WriteLog($"{entrustResult.ResultMessage}", order.OrderId, order);
                return this.agreementInfoList;
            }
            string borrowContractId = Guid.NewGuid().ToGuidString();
            Dictionary<string, string> borrowAgreementContent = await this.BuildContractContentAsync(borrowContractId, order, productInfo, Constants.BorrowAgreementIndex);
            GenerationResult borrowResult = await this.GenerationSigleAgreementContractAsync(productInfo, order, Constants.BorrowAgreementIndex, borrowAgreementContent, borrowContractId); //生成

            if (!borrowResult.Result)
            {
                this.isComplate = false;
                LogHelper.WriteLog($"{borrowResult.ResultMessage}", order.OrderId, order);
                return this.agreementInfoList;
            }

            AgreementInfoEntity info = new AgreementInfoEntity
            {
                FDDAccountId = entrustResult.Email,
                OrderId = order.OrderId,
                RowKey = order.OrderId,
                UserId = order.UserInfo.UserId.ToGuidString(),
                EntrustAgreementViewUrl = entrustResult.EntrustViewUrl,
                EntrustAgreementDownUrl = entrustResult.EntrustDownloadUrl,
                BorrowAgreementViewUrl = borrowResult.BorrowViewUrl,
                BorrowAgreementDownUrl = borrowResult.BorrowDownloadUrl,
                EntrustContractId = entrustContractId,
                BorrowContractId = borrowContractId
            };

            ConfigManager.AzureCloudTableClient.SaveToCloudTable("CustomerAgreementInfo", info, productInfo.ProductId.ToGuidString());

            this.agreementInfoList.Add(new AgreementInfo
            {
                UserIndentifier = order.UserInfo.UserId.ToGuidString(),
                FDDAccountId = entrustResult.Email
            });
            this.isComplate = true;
            LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {order.OrderId} 生成成功", order.OrderId);
            return this.agreementInfoList;
        }

        /// <summary>
        ///     01 定期生成单个产品下所有订单的合同
        /// </summary>
        /// <param name="productOrderInfo">The productorderinfo.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<BasicResult<List<AgreementInfo>>> GenerationRegularProductOrdersContractAsync(ProductOrderInfo productOrderInfo)
        {
            try
            {
                await productOrderInfo.Order.Where(order => !order.IsSignature())
                    .ForEach(async order => await this.GenerationRegularProductSigleOrderAsync(productOrderInfo.ProductInfo, order));

                if (!this.isComplate)
                {
                    return BasicResult<List<AgreementInfo>>.Failed("产品订单未全部完成", this.agreementInfoList);
                }
                await this.tirisfalService.NotifyTirisfalRegularAgreementAsync(productOrderInfo.ProductInfo.ProductId.ToGuidString(), this.agreementInfoList);

                return BasicResult<List<AgreementInfo>>.Success("成功", this.agreementInfoList);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "GenerationContractAsync");
                throw new Exception($"GenerationSigleProductContractAsync {ex.Message}");
            }
        }

        /// <summary>
        ///     05 生成单个协议合同
        /// </summary>
        /// <param name="productInfo"></param>
        /// <param name="orderInfo"></param>
        /// <param name="agreementIndex"></param>
        /// <param name="contractContent"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        private async Task<GenerationResult> GenerationSigleAgreementContractAsync(RegularProductInfo productInfo, OrderInfo orderInfo, int agreementIndex, Dictionary<string, string> contractContent, string contractId)
        {
            try
            {
                string contractType = agreementIndex == 0 ? "委托" : "借款";

                #region 本地生成PDF

                string templatePath = this.templateFilePath + this.fddService.GetTemplateFilePath(productInfo.ProductCategory, agreementIndex);

                if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
                {
                    return GenerationResult.Failed("模板文件不存在");
                }

                //生成合同PDF文件
                Tuple<bool, string> createContractResult = await this.fddService.CreateContractFileByTemplateAsync(templatePath, this.contractFilePath, contractId, contractContent, productInfo.ProductId.ToGuidString());
                if (!createContractResult.Item1)
                {
                    return GenerationResult.Failed("生成合同PDF文件失败");
                }
                LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {orderInfo.OrderId} {contractType}协议生成本地PDF文件成功", orderInfo.OrderId);
                //上传合同至法大大
                TransferContractResponse uploadContractResult = await this.fddService.UploadContractToFddAsync(createContractResult.Item2, contractId, this.fddService.GetContractDocTitle(productInfo.ProductCategory, agreementIndex));
                if (uploadContractResult == null || uploadContractResult.Code != FDDResultCode.Success)
                {
                    return GenerationResult.Failed($"上传合同至法大大失败, 失败原因: {uploadContractResult?.Msg}");
                }
                LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {orderInfo.OrderId} {contractType} 协议上传合同至法大大成功", orderInfo.OrderId);

                #endregion 本地生成PDF

                //申请CA证书
                CACretificateResult applyCaCretificateResult = await this.fddService.ApplyCaCretificateAsync(orderInfo.UserInfo.RealName, orderInfo.UserInfo.CredentialNo, orderInfo.UserInfo.Cellphone);

                if (!applyCaCretificateResult.Result)
                {
                    return GenerationResult.Failed($"申请CA证书失败,失败原因:{applyCaCretificateResult.ResponseMsg}");
                }
                LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {orderInfo.OrderId} {contractType}协议申请CA证书成功", orderInfo.OrderId);

                // 开始签署 第一次签署用户章 第二次签署公司章
                SignatureContractResult customerSignatureContractResult = await this.fddService.AutoSignatureConatractAsync(contractId, Guid.NewGuid().ToGuidString(), applyCaCretificateResult.CustomerId, ConfigManager.FDDCustomerRole, ConfigManager.FDDCustomerSignKey, this.fddService.GetContractDocTitle(productInfo.ProductCategory, agreementIndex)); //签署

                if (!customerSignatureContractResult.Result)
                {
                    //受让人签章失败
                    return GenerationResult.Failed($"用户签章失败,失败原因:{customerSignatureContractResult.ResultMessage}");
                }
                LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {orderInfo.OrderId} {contractType}协议受让人签章成功", orderInfo.OrderId);

                SignatureContractResult companySignatureContractResult = await this.fddService.AutoSignatureConatractAsync(contractId, Guid.NewGuid().ToGuidString(), ConfigManager.FDDCompanyID, ConfigManager.FDDCompanyRole, ConfigManager.FDDCompanySignKey, this.fddService.GetContractDocTitle(productInfo.ProductCategory, agreementIndex)); //签署
                if (!companySignatureContractResult.Result)
                {
                    //平台签章失败
                    return GenerationResult.Failed($"平台签章失败,失败原因:{customerSignatureContractResult.ResultMessage}");
                }
                LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {orderInfo.OrderId} {contractType}协议平台签章成功", orderInfo.OrderId);

                //归档
                BasicResult<string> contractArchiveResult = await this.fddService.ContractArchiveAsync(contractId);
                if (!contractArchiveResult.Result)
                {
                    //合同归档失败
                    return GenerationResult.Failed($"合同归档失败,失败原因:{contractArchiveResult.Message}");
                }
                LogHelper.WriteLog($"产品 {productInfo.ProductId.ToGuidString()} 订单 {orderInfo.OrderId} {contractType}协议合同归档成功", orderInfo.OrderId);

                switch (agreementIndex)
                {
                    case 0:
                        return new GenerationResult
                        {
                            EntrustViewUrl = companySignatureContractResult.ViewUrl,
                            EntrustDownloadUrl = companySignatureContractResult.DownloadUrl,
                            Email = applyCaCretificateResult.Email,
                            Result = true,
                            ResultMessage = "委托协议成功"
                        };

                    case 1:
                        return new GenerationResult
                        {
                            BorrowViewUrl = companySignatureContractResult.ViewUrl,
                            BorrowDownloadUrl = companySignatureContractResult.DownloadUrl,
                            Email = applyCaCretificateResult.Email,
                            Result = true,
                            ResultMessage = "借款协议成功"
                        };

                    default:
                        return GenerationResult.Failed("未知类型错误,请联系管理员");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"生成法大大协议异常: {ex.ToJson()}");
            }
        }

        #region 辅助方法

        /// <summary>
        ///     06 01 构造模板中需要填充的内容
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="order">The order.</param>
        /// <param name="product">The product.</param>
        /// <param name="agreementindex">The agreementindex.</param>
        /// <returns>
        ///     System.Threading.Tasks.Task&lt;System.Collections.Generic.Dictionary&lt;System.String, System.String&gt;&gt;.
        /// </returns>
        private async Task<Dictionary<string, string>> BuildContractContentAsync(string contractid, OrderInfo order,
            RegularProductInfo product, int agreementindex)
        {
            //担保贷借款协议
            switch (product.ProductCategory)
            {
                case 100000021: //担保贷
                    return await this.BuildGuaranteedLoanContractContentAsync(contractid, order, product, agreementindex);

                case 100000010: //普通银票
                    return await this.BuildGeneralBankBillContractContentAsync(contractid, order, product, agreementindex);

                case 100000020: //商票
                case 100000022:
                case 100000023:
                    return await this.BuildCommercialInvoiceContractContentAsync(contractid, order, product, agreementindex);

                case 100000011: //银票保理
                    return await this.BuildBankBillFactorContractContentAsync(contractid, order, product, agreementindex);
            }

            return await Task.FromResult(this.contractContentDict);
        }

        #endregion 辅助方法

        #endregion 生成合同

        #region 根据产品类型生成不同合同

        private async Task<string> GetProductAgreementInfoAsync(string productid)
        {
            ProductAgreementInfo info = ConfigManager.AzureCloudTableClient.ReadFromCloudTable<ProductAgreementInfo>("JYMPublicAgreementParameter", "ProductId", productid);

            if (info == null)
            {
                return await Task.FromResult(string.Empty);
            }
            return await Task.FromResult(info.AgreementValue);
        }

        #region 商票

        /// <summary>
        ///     商票借款协议内容
        /// </summary>
        /// <param name="contractid"></param>
        /// <param name="order"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> BuildCommercialInvoiceBorrowAgreementContentAsync(string contractid, OrderInfo order, RegularProductInfo product)
        {
            ProductAgreementValue agreementInfo = JsonConvert.DeserializeObject<ProductAgreementValue>(await this.GetProductAgreementInfoAsync(product.ProductId.ToGuidString()));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Clear();
            dict.Add("ContractID", contractid);
            dict.Add("Lender", order.UserInfo.RealName); //甲方(出借人)
            dict.Add("LenderNO", order.UserInfo.CredentialNo.ConfusionCredentialNo());
            dict.Add("LenderJYMName", order.UserInfo.Cellphone.ConfusionCellphone());

            dict.Add("Borrower", agreementInfo?.FinancierNameOfUser); //乙方(借款人)
            dict.Add("License", agreementInfo?.EnterpriseLicenseNum);

            dict.Add("ProductNO", agreementInfo?.ProductCode);
            //this.dict.Add("Usage", agreementInfo?.FundUsage);
            dict.Add("Principal", (order.Principal * 1.0 / 100).ToString(CultureInfo.InvariantCulture));
            dict.Add("Interest", (order.Interest * 1.0 / 100).ToString(CultureInfo.InvariantCulture));
            dict.Add("Yield", (product.Yield * 1.0 / 100).ToString(CultureInfo.InvariantCulture));

            //借款开始日期
            dict.Add("StartDate", order.OrderTime.ToString("yyyy年MM月dd日"));

            //借款结束日期
            dict.Add("EndDate", product.RepaidTime.GetValueOrDefault().ToString("yyyy年MM月dd日"));

            //还款日期
            dict.Add("RepayDate", product.RepaidTime.GetValueOrDefault().ToString("yyyy年MM月dd日"));

            dict.Add("Drawer", agreementInfo?.PayerNameOfUser);
            dict.Add("BillEndDate", agreementInfo?.BillDueDate.ToDateTime().ToString("yyyy年MM月dd日"));
            dict.Add("BillNO", agreementInfo?.BillNo);
            dict.Add("BillAmount", agreementInfo?.BillMoney);

            // 签署日期 为订单生成日期
            dict.Add("SignDate", order.ResultTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        /// <summary>
        ///     商票协议
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="order">The order.</param>
        /// <param name="product">The product.</param>
        /// <param name="agreementindex">The agreementindex.</param>
        /// <returns>Task&lt;Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        private async Task<Dictionary<string, string>> BuildCommercialInvoiceContractContentAsync(string contractid,
            OrderInfo order, RegularProductInfo product, int agreementindex)
        {
            switch (agreementindex)
            {
                case 0:
                    return await this.BuildCommercialInvoiceEntrustAgreementContentAsync(contractid, order, product);

                case 1:
                    return await this.BuildCommercialInvoiceBorrowAgreementContentAsync(contractid, order, product);

                default:
                    return await Task.FromResult(this.contractContentDict);
            }
        }

        /// <summary>
        ///     商票委托协议内容
        /// </summary>
        /// <param name="contractid"></param>
        /// <param name="order"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> BuildCommercialInvoiceEntrustAgreementContentAsync(
            string contractid, OrderInfo order, RegularProductInfo product)
        {
            ProductAgreementValue agreementInfo = JsonConvert.DeserializeObject<ProductAgreementValue>(await this.GetProductAgreementInfoAsync(product.ProductId.ToGuidString()));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Clear();
            dict.Add("ContractID", contractid);

            dict.Add("ClientID", order.UserInfo.RealName); //委托人
            dict.Add("Borrower", agreementInfo?.FinancierNameOfUser); //融资企业名称

            dict.Add("BillNO", agreementInfo?.BillNo);
            dict.Add("BillAmount", agreementInfo?.BillMoney);
            dict.Add("Drawer", agreementInfo?.PayerNameOfUser);
            dict.Add("BillEndDate", agreementInfo?.BillDueDate.ToDateTime().ToString("yyyy年MM月dd日"));

            //签署日期
            dict.Add("SignDate", order.ResultTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        #endregion 商票

        #region 担保贷

        /// <summary>
        ///     担保贷借款协议内容
        /// </summary>
        /// <param name="contractid"></param>
        /// <param name="order"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> BuildGuaranteedLoanBorrowAgreementContentAsync(string contractid, OrderInfo order, RegularProductInfo product)
        {
            ProductAgreementValue agreementInfo = JsonConvert.DeserializeObject<ProductAgreementValue>(await this.GetProductAgreementInfoAsync(product.ProductId.ToGuidString()));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Clear();
            dict.Add("ContractID", contractid);

            dict.Add("Lender", order.UserInfo.RealName); //甲方(出借人)
            dict.Add("LenderJYMName", order.UserInfo.Cellphone.ConfusionCellphone());

            dict.Add("Borrower", agreementInfo?.FinancierNameOfUser); //乙方(借款人)
            dict.Add("License", agreementInfo?.EnterpriseLicenseNum);

            dict.Add("ProductNO", agreementInfo?.ProductCode);
            dict.Add("Usage", agreementInfo?.FundUsage);
            dict.Add("Principal", (order.Principal * 1.0 / 100).ToString(CultureInfo.InvariantCulture));
            //this.dict.Add("Interest", (order.Interest * 1.0 / 100).ToString(CultureInfo.InvariantCulture));
            dict.Add("Yield", (product.Yield * 1.0 / 100).ToString(CultureInfo.InvariantCulture));

            //借款开始日期
            dict.Add("StartDate", order.OrderTime.ToString("yyyy年MM月dd日"));
            //借款结束日期
            dict.Add("EndDate", product.RepaidTime.GetValueOrDefault().ToString("yyyy年MM月dd日"));

            //还款日期
            dict.Add("RepayDate", product.RepaidTime.GetValueOrDefault().ToString("yyyy年MM月dd日"));

            dict.Add("Guarantor", agreementInfo?.GuaranteeFullName); //担保人

            dict.Add("SignDate", order.ResultTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        /// <summary>
        ///     担保贷协议内容
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="order">The order.</param>
        /// <param name="product">The product.</param>
        /// <param name="agreementindex">The agreementindex.</param>
        /// <returns>
        ///     System.Threading.Tasks.Task&lt;System.Collections.Generic.Dictionary&lt;System.String, System.String&gt;&gt;.
        /// </returns>
        private async Task<Dictionary<string, string>> BuildGuaranteedLoanContractContentAsync(string contractid, OrderInfo order, RegularProductInfo product, int agreementindex)
        {
            switch (agreementindex)
            {
                case 0:
                    return await this.BuildGuaranteedLoanEntrustAgreementContentAsync(contractid, order, product);

                case 1:
                    return await this.BuildGuaranteedLoanBorrowAgreementContentAsync(contractid, order, product);

                default:
                    return await Task.FromResult(this.contractContentDict);
            }
        }

        /// <summary>
        ///     担保贷委托协议内容
        /// </summary>
        /// <param name="contractid"></param>
        /// <param name="order"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> BuildGuaranteedLoanEntrustAgreementContentAsync(string contractid, OrderInfo order, RegularProductInfo product)
        {
            ProductAgreementValue agreementInfo = JsonConvert.DeserializeObject<ProductAgreementValue>(await this.GetProductAgreementInfoAsync(product.ProductId.ToGuidString()));
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Clear();

            dict.Add("ContractID", contractid);
            dict.Add("ClientID", order.UserInfo.RealName); //委托人
            dict.Add("Borrower", agreementInfo?.FinancierNameOfUser); // 借款人

            dict.Add("SignDate", order.ResultTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        #endregion 担保贷

        #region 银票

        /// <summary>
        ///     普通银票借款协议内容
        /// </summary>
        /// <param name="contractid"></param>
        /// <param name="order"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> BuildGeneralBankBillBorrowAgreementContentAsync(string contractid, OrderInfo order, RegularProductInfo product)
        {
            ProductAgreementValue agreementInfo =
                JsonConvert.DeserializeObject<ProductAgreementValue>(
                    await this.GetProductAgreementInfoAsync(product.ProductId.ToGuidString()));
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Clear();
            dict.Add("ContractID", contractid);
            dict.Add("Lender", order.UserInfo?.RealName); //甲方(出借人)
            dict.Add("LenderNO", order.UserInfo?.CredentialNo.ConfusionCredentialNo());
            dict.Add("LenderJYMName", order.UserInfo?.Cellphone.ConfusionCellphone());

            //this.dict.Add("Borrower", agreementInfo?.FinancierNameOfUser); //乙方(借款人)
            //this.dict.Add("License", agreementInfo?.EnterpriseLicenseNum);

            dict.Add("ProductNO", agreementInfo?.ProductCode);
            //this.dict.Add("Usage", agreementInfo?.FundUsage);
            dict.Add("Principal", (order.Principal * 1.0 / 100).ToString(CultureInfo.InvariantCulture));
            dict.Add("Interest", (order.Interest * 1.0 / 100).ToString(CultureInfo.InvariantCulture));
            dict.Add("Yield", (product.Yield / 100).ToString());

            //借款开始日期
            dict.Add("StartDate", order.OrderTime.ToString("yyyy年MM月dd日"));

            //借款结束日期
            dict.Add("EndDate", product.RepaidTime.GetValueOrDefault().ToString("yyyy年MM月dd日"));

            //还款日期
            dict.Add("RepayDate", product.RepaidTime.GetValueOrDefault().ToString("yyyy年MM月dd日"));

            //银票信息
            dict.Add("BillNO", agreementInfo?.BillNo);
            dict.Add("BillAmount", agreementInfo?.BillMoney);
            dict.Add("Drawer", agreementInfo?.PayerNameOfUser); //出票银行
            dict.Add("ExDate", agreementInfo?.BillDueDate.ToDateTime().ToString("yyyy年MM月dd日"));

            //背书交付日
            dict.Add("EndorseDate",
                agreementInfo?.ProductRaiseStartDateTime.ToDateTime().ToString("yyyy年MM月dd日"));

            //签署日期
            dict.Add("SignDate", order.ResultTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        /// <summary>
        ///     普通银票借款协议
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="order">The order.</param>
        /// <param name="product">The product.</param>
        /// <param name="agreementindex">The agreementindex.</param>
        /// <returns>Task&lt;Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        private async Task<Dictionary<string, string>> BuildGeneralBankBillContractContentAsync(string contractid, OrderInfo order, RegularProductInfo product, int agreementindex)
        {
            switch (agreementindex)
            {
                case 0:
                    return await this.BuildGeneralBankBillEntrustAgreementContentAsync(contractid, order, product);

                case 1:
                    return await this.BuildGeneralBankBillBorrowAgreementContentAsync(contractid, order, product);

                default:
                    return await Task.FromResult(this.contractContentDict);
            }
        }

        /// <summary>
        ///     普通银票委托协议内容
        /// </summary>
        /// <param name="contractid"></param>
        /// <param name="order"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> BuildGeneralBankBillEntrustAgreementContentAsync(string contractid, OrderInfo order, RegularProductInfo product)
        {
            ProductAgreementValue agreementInfo = JsonConvert.DeserializeObject<ProductAgreementValue>(await this.GetProductAgreementInfoAsync(product.ProductId.ToGuidString()));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Clear();
            dict.Add("ContractID", contractid);

            dict.Add("ClientID", order.UserInfo.RealName); //甲方(委托人)

            //银票信息
            dict.Add("BillNO", agreementInfo?.BillNo);
            dict.Add("BillAmount", agreementInfo?.BillMoney);
            dict.Add("Drawer", agreementInfo?.PayerNameOfUser); //出票银行
            dict.Add("ExDate", agreementInfo?.BillDueDate.ToDateTime().ToString("yyyy年MM月dd日"));

            //签署日期
            dict.Add("SignDate", order.ResultTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        #endregion 银票

        #region 银票保理

        /// <summary>
        ///     银票保理借款协议
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="order">The order.</param>
        /// <param name="product">The product.</param>
        /// <returns>Task&lt;Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        private async Task<Dictionary<string, string>> BuildBankBillFactorBorrowAgreementContentAsync(string contractid, OrderInfo order, RegularProductInfo product)
        {
            ProductAgreementValue agreementInfo = JsonConvert.DeserializeObject<ProductAgreementValue>(await this.GetProductAgreementInfoAsync(product.ProductId.ToGuidString()));
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Clear();
            dict.Add("ContractID", contractid);
            dict.Add("Lender", order.UserInfo.RealName); //甲方(出借人)
            dict.Add("LenderJYMName", order.UserInfo.Cellphone.ConfusionCellphone());

            //银票信息
            dict.Add("Drawer", agreementInfo?.PayerNameOfUser); //出票银行
            dict.Add("BillNO", agreementInfo?.BillNo);
            dict.Add("BillAmount", agreementInfo?.BillMoney);
            dict.Add("ExDate", agreementInfo?.BillDueDate);

            dict.Add("ProductNO", agreementInfo?.ProductCode);
            dict.Add("Principal", (order.Principal * 1.0 / 100).ToString(CultureInfo.InvariantCulture));
            dict.Add("Yield", (product.Yield / 100).ToString());

            //借款开始日期
            dict.Add("StartDate", order.OrderTime.ToString("yyyy年MM月dd日"));

            //借款结束日期
            dict.Add("EndDate", order.RepaidTime.ToString("yyyy年MM月dd日"));

            //最迟还款日期
            dict.Add("RepayDate", product.RepaymentDeadline.ToString("yyyy年MM月dd日"));

            //签署日期
            dict.Add("SignDate", order.ResultTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        /// <summary>
        ///     银票保理协议
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="order">The order.</param>
        /// <param name="product">The product.</param>
        /// <param name="agreementindex">The agreementindex.</param>
        /// <returns>Task&lt;Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        private async Task<Dictionary<string, string>> BuildBankBillFactorContractContentAsync(string contractid,
            OrderInfo order, RegularProductInfo product, int agreementindex)
        {
            switch (agreementindex)
            {
                case 0:
                    return await this.BuildBankBillFactorEntrustAgreementContentAsync(contractid, order.UserInfo.RealName,
                        order.ResultTime);

                case 1:
                    return await this.BuildBankBillFactorBorrowAgreementContentAsync(contractid, order, product);

                default:
                    return await Task.FromResult(this.contractContentDict);
            }
        }

        /// <summary>
        ///     银票保理委托协议
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <param name="realName"></param>
        /// <param name="signTime"></param>
        /// <returns>Task&lt;Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        private async Task<Dictionary<string, string>> BuildBankBillFactorEntrustAgreementContentAsync(
            string contractid, string realName, DateTime signTime)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Clear();
            dict.Add("ContractID", contractid);
            dict.Add("ClientID", realName); //甲方(委托人)
            //签署日期
            dict.Add("SignDate", signTime.ToString("yyyy年MM月dd日"));

            return await Task.FromResult(dict);
        }

        #endregion 银票保理

        #endregion 根据产品类型生成不同合同

        #region 合同生成(不使用)

        /* 法大大生成合同
        /// <summary>
        /// 通过法大大生成合同
        /// </summary>
        /// <param name="productinfo">The productinfo.</param>
        /// <param name="orderinfo">The orderinfo.</param>
        /// <param name="agreementindex">The agreementindex.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> GenerationSigleAgreementContractByFDDAsync(RegularProductInfo productinfo, OrderInfo orderinfo, int agreementindex = 0)
        {
#region 由法大大生成合同

            KeyValuePair<int, string> dictionary = new KeyValuePair<int, string>(0, "EF1N1100115000");

            //生成合同
            Tuple<bool, string> generationresult = await this.GenerationContractAsync(productinfo, orderinfo, dictionary);

            if (!generationresult.Item1)
            {
                return await Task.FromResult(string.Empty);
            }
            return await Task.FromResult(Guid.NewGuid().ToGuidString());

#endregion 由法大大生成合同
        }

        /// <summary>
        /// 法大大生成合同
        /// </summary>
        /// <param name="productinfo">The productinfo.</param>
        /// <param name="orderinfo">The orderinfo.</param>
        /// <param name="agreement">The agreement.</param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, GenerationResult&gt;&gt;.</returns>
        private async Task<Tuple<bool, string>> GenerationContractAsync(RegularProductInfo productinfo, OrderInfo orderinfo, KeyValuePair<int, string> agreement)
        {
            //EF1N1100115000
            string contractid = Guid.NewGuid().ToGuidString();
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string parametermap = this.BuildParameterMapAsync(productinfo, orderinfo, contractid);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("app_id", ConfigManager.FDDAppID),
                new KeyValuePair<string, string>("timestamp", timestamp),
                new KeyValuePair<string, string>("template_id", agreement.Value),
                new KeyValuePair<string, string>("contract_id", contractid),
                new KeyValuePair<string, string>("font_size", ConfigManager.FDDFontSize.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("font_type", ConfigManager.FDDFontType.ToString()),
                new KeyValuePair<string, string>("parameter_map", parametermap),
                new KeyValuePair<string, string>("msg_digest", await this.BuildGenerationContractMsgDigestAsync(timestamp, agreement.Value, contractid, parametermap))
            };

            string result = FddHelper.PostByWebRequest(ConfigManager.FDDBaseAddress + "generate_contract.api", parameters);

            ContractGenerationResponse response = JsonConvert.DeserializeObject<ContractGenerationResponse>(result);

            if (response.Code != FDDCode.Success)
            {
                LogHelper.WriteLog($"产品{productinfo.ProductId.ToGuidString()}订单{orderinfo.OrderId.ToGuidString()}协议生成合同失败       {response.Msg}", "GenerationContractAsync");
                return await Task.FromResult(Tuple.Create(false, contractid));
            }
            return await Task.FromResult(Tuple.Create(true, contractid));
        }

        /// <summary>
        /// 合同生成消息摘要
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="templateid">The templateid.</param>
        /// <param name="contractid">The contractid.</param>
        /// <param name="parametermap">The parametermap.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> BuildGenerationContractMsgDigestAsync(string timestamp, string templateid, string contractid, string parametermap)
        {
            string md5timestamp = FddHelper.Md5(timestamp);

            string sha1secret = FddHelper.Sha1(ConfigManager.FDDAppSecrect + templateid + contractid);

            string sha1all = FddHelper.Sha1(ConfigManager.FDDAppID + md5timestamp + sha1secret + parametermap);

            string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(sha1all));

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 构建合同生成填充内容
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="order">The order.</param>
        /// <param name="contractid">The contractid.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        private string BuildParameterMapAsync(RegularProductInfo product, OrderInfo order, string contractid)
        {
            this.parameterMaps.Clear();

            this.parameterMaps.Add("ContractId", contractid);
            this.parameterMaps.Add("RealName", order.UserInfo.RealName);
            this.parameterMaps.Add("CredentialNo", order.UserInfo.CredentialNo); //order.UserInfos.CredentialNo
            this.parameterMaps.Add("Cellphone", order.UserInfo.Cellphone); // order.UserInfos.Cellphone
            this.parameterMaps.Add("LendRealName", product.EnterpriseName.IsNullOrEmpty() ? "默认" : product.EnterpriseName); //product.EnterpriseName
            this.parameterMaps.Add("LendCredentialNo", product.EnterpriseLicense.IsNullOrEmpty() ? "123456" : product.EnterpriseLicense); //product.EnterpriseLicense
            this.parameterMaps.Add("BidName", product.ProductName); //product.ProductName
            this.parameterMaps.Add("ProductIssno", product.IssueNo.ToString()); //product.IssueNo.ToString()
            this.parameterMaps.Add("FinancingSumAmount", (product.FinancingSumAmount / 100).ToString()); // (product.FinancingSumAmount / 100).ToString()
            this.parameterMaps.Add("Yield", (product.Yield / 100).ToString()); // (product.Yield / 100).ToString()
            this.parameterMaps.Add("OrderTime", order.OrderTime.ToString("yyyyMMdd")); //order.OrderTime.ToString("yyyyMMdd")
            this.parameterMaps.Add("RepaymentDeadline", product.RepaymentDeadline.ToString("yyyyMMdd")); //product.RepaymentDeadline.ToString("yyyyMMdd");

            return this.parameterMaps.ToJson();
        }

        */

        #endregion 合同生成(不使用)

        #region 定期订单协议生成

        /// <summary>
        ///     生成单个定期产品协议
        /// </summary>
        /// <param name="productid">The productid.</param>
        public async Task GenerationSigleRegularProductContractAsync(string productid)
        {
            LogHelper.WriteLog($"产品 [{productid}] 开始生成PDF文档", productid);

            BasicResult<ProductOrderInfo> tirisfalResult = await this.tirisfalService.GetProductOrderInfoAsync(productid);

            if (!tirisfalResult.Result)
            {
                LogHelper.WriteLog($"查询产品 {productid} 订单信息出错,错误细信息:{tirisfalResult.Message}", productid);
                return;
            }

            BasicResult<List<AgreementInfo>> generationResult = await this.GenerationRegularProductOrdersContractAsync(tirisfalResult.Data);

            LogHelper.WriteLog($"产品 [{productid}] 生成PDF文档成功,应生成 {tirisfalResult.Data.Order.Count()} 个,实际生成 {generationResult.Data.Count} 个", productid);
        }

        /// <summary>
        ///     两小时执行一次 可配置
        /// </summary>
        public void SignatureRegularAgreement()
        {
            LogHelper.WriteLog("开始生成定期产品...", "DoWork");
            try
            {
                //查询需要电子签章的产品列表
                List<string> productList = JYMDBHelper.GetRegularProductLists().ToList();

                productList.Clear();

                productList.Add("716ACEFD6FF649A3BE33BB10C303716F");
                productList.Add("FA53AE93648546A09DFCB2DBA105F9B5");
                productList.Add("EE6C1B2F6E724773A65A7F8EC2810D0F");
                productList.Add("C06AD6EF80B5434993CC98ED7564159B");
                productList.Add("52774E92681C46A5A597E7A31BC73E29");
                productList.Add("E8450C11F2E646D2ABDE042613F79D8A");

                LogHelper.WriteLog($"共有 [{productList.Count}] 个产品需要生成电子签章", "DoWork");

                productList.ForEach(async productid => await this.GenerationSigleRegularProductContractAsync(productid));

                LogHelper.WriteLog("定期产品生成完成...", "DoWork");

                if (ConfigManager.IsClearFile)
                {
                    this.fddService.ClearContractFile();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "DoWork");
            }
        }

        #endregion 定期订单协议生成
    }
}