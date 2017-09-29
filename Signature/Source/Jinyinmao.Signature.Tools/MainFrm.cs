using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using jinyinmao.Signature.lib;
using jinyinmao.Signature.lib.Common;
using jinyinmao.Signature.Service;
using Moe.Lib;
using Newtonsoft.Json;
using NLog;

namespace Jinyinmao.Signature.Tools
{
    public partial class MainFrm : Form
    {
        private readonly FddService fddService;

        private readonly RegularProductService regularProductService;
        private readonly TirisfalService tirisfalService;
        private readonly YemService yemService;

        public MainFrm()
        {
            this.InitializeComponent();

            this.regularProductService = new RegularProductService();
            this.yemService = new YemService();
            this.tirisfalService = new TirisfalService();
            this.fddService = new FddService();
        }

        private async void btn_YEM_Click(object sender, EventArgs e)
        {
            string request = this.txtRequest.Text.Trim();

            List<YEMOrderInfo> orderList = JsonConvert.DeserializeObject<List<YEMOrderInfo>>(request);

            List<AgreementInfo> generationResult = await this.yemService.GenerationYemContractAsync(orderList);

            MessageBox.Show($"余额猫生成PDF文档成功,应生成 {orderList.Count()} 个,实际生成 {generationResult.Count} 个");
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            LogManager.GetCurrentClassLogger().Info("Test");

            await Task.Factory.StartNew(() =>
            {
                if (this.txtProductId.Text.Trim().IsNullOrEmpty())
                {
                    MessageBox.Show("产品号不能为空!");
                }
                Guid ProductId;
                if (!Guid.TryParse(this.txtProductId.Text.Trim(), out ProductId))
                {
                    MessageBox.Show("产品号不正确!");
                }

                if (!this.txtOrderiD.Text.Trim().IsNullOrEmpty())
                {
                    Guid OrderId;
                    if (!Guid.TryParse(this.txtOrderiD.Text.Trim(), out OrderId))
                    {
                        MessageBox.Show("订单号号不正确!");
                    }
                    this.GenerationSigleOrder(ProductId, OrderId).GetAwaiter().GetResult();
                }
                else
                {
                    this.GenerationProduct(ProductId).GetAwaiter().GetResult();
                }
            }).ContinueWith(task => this.fddService.ClearContractFile());
        }

        private async Task GenerationProduct(Guid productid)
        {
            await this.regularProductService.GenerationSigleRegularProductContractAsync(productid.ToGuidString());

            MessageBox.Show("电子合同生成失败!");
        }

        private async Task GenerationSigleOrder(Guid productid, Guid orderid)
        {
            BasicResult<ProductOrderInfo> productorderinfo = await this.tirisfalService.GetProductOrderInfoAsync(productid.ToGuidString());
            if (productorderinfo == null || !productorderinfo.Data.Order.Any())
            {
                MessageBox.Show("没有要生成电子合同的订单!");
                return;
            }
            OrderInfo orderInfo = productorderinfo.Data.Order.Single(o => o.OrderId == orderid.ToGuidString());

            List<AgreementInfo> list = await this.regularProductService.GenerationRegularProductSigleOrderAsync(productorderinfo.Data.ProductInfo, orderInfo);

            await this.tirisfalService.NotifyTirisfalRegularAgreementAsync(productorderinfo.Data.ProductInfo.ProductId.ToGuidString(), list);

            MessageBox.Show("电子合同生成成功!");
        }
    }
}