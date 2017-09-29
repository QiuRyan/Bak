using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jinyinmao.Daemon.Services
{
    public class CreateProductAgreementService
    {
        private string baseUrl = "";
        private Lazy<HttpClient> FDDhttpClient = new Lazy<HttpClient>(() => new HttpClient { BaseAddress = new Uri("") });
        private Lazy<HttpClient> httpClient = new Lazy<HttpClient>(() => new HttpClient { BaseAddress = new Uri("") });

        public CreateProductAgreementService()
        {
            this.baseUrl = "";
        }

        public CreateProductAgreementService(string baseurl)
        {
            this.baseUrl = baseurl;
        }

        /// <summary>
        ///     检查产品是否打款
        /// </summary>
        /// <param name="productid">The productid.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CheckProductRepay(string productid)
        {
            return await Task.FromResult(false);
        }

        /// <summary>
        ///     更新该产品下的订单信息
        /// </summary>
        /// <param name="productid">The productid.</param>
        /// <returns>Task.</returns>
        public async Task UpdateOrder(string productid)
        {
            await Task.FromResult("");
        }
    }
}