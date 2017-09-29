using System;
using System.Data.SqlClient;
using Dapper;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Moe.Lib;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace Jinyinmao.Daemon.Services
{
    public class BankDepositoryService
    {
        private static readonly string ebibpbCenterConnectionString;
        private static readonly string gatewayHost;

        private readonly Lazy<HttpClient> callCenterClient;
        private HttpClient CallCenterClient => this.callCenterClient.Value;

        static BankDepositoryService()
        {
            ebibpbCenterConnectionString = CloudConfigurationManager.GetSetting("EbibpbCenterConnectionString");
            gatewayHost = CloudConfigurationManager.GetSetting("GatewayHost") + "/gateway/";
        }

        public BankDepositoryService()
        {
            this.callCenterClient = new Lazy<HttpClient>(this.InitCallCenterHttpClient);
        }

        /// <summary>
        ///     创建连接
        /// </summary>
        /// <returns></returns>
        private static SqlConnection GetOpenConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        #region 银行存管融资方流水和平台方无响应流水制为失败

        /// <summary>
        ///     银行存管融资方流水和平台方无响应流水制为失败 
        /// </summary>
        public void WorkUpdate()
        {
            try
            {
                //this.SynUpdateTranscations();
                this.SyncAccoutTransactionRun().Wait();
                this.SyncPlatformTranscationsRun().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("银行存管无响应流水制为失败：Error: {0}".FormatWith(e.Message), e);
            }
        }

        #region 注释
        //private void SynUpdateTranscations()
        //{
        //    try
        //    {
        //        using (SqlConnection connection = GetOpenConnection(ebibpbCenterConnectionString))
        //        {
        //            DateTime dt = DateTime.UtcNow.ToChinaStandardTime();
        //            DateTime dtend = dt.AddMinutes(-30);
        //            DateTime dtbegin = dt.AddHours(-24);
        //            //string sqlT = $"update PlatformTranscations set ResultTime='{dt}',LastEditTime='{dt}',ResultCode=-1 where LastEditTime>='{dtbegin}' and LastEditTime<='{dtend}' and TradeCode=1005051001 and ResultCode=0 and Status=0 and IsDel=0 ";
        //            //string sqlP = $"update AccoutTransaction set ResultTime='{dt}',LastEditTime='{dt}',ResultCode=-1 where LastEditTime>='{dtbegin}' and LastEditTime<='{dtend} and TradeCode=1005051001' and ResultCode=0 and Status=0 and IsDel=0 ";
        //            string sqlT = $"update PlatformTranscations set ResultTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',LastEditTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',ResultCode=-1 where LastEditTime>='{dtbegin.ToString("yyyy-MM-dd HH:mm:ss")}' and LastEditTime<='{dtend.ToString("yyyy-MM-dd HH:mm:ss")}' and ResultCode=0 and Status=0 and IsDel=0 ";
        //            string sqlP = $"update AccoutTransaction set ResultTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',LastEditTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',ResultCode=-1 where LastEditTime>='{dtbegin.ToString("yyyy-MM-dd HH:mm:ss")}' and LastEditTime<='{dtend.ToString("yyyy-MM-dd HH:mm:ss")} and ResultCode=0 and Status=0 and IsDel=0 ";
        //            string sql = $"{sqlT} {sqlP} ";
        //            connection.Execute(sql);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.LogError("银行存管无响应流水制为失败：Error: {0}".FormatWith(e.Message), e);
        //    }
        //}
        #endregion

        #region 查询借款户的流水

        /// <summary>
        /// 查询借款户的流水
        /// </summary>
        private async Task SyncAccoutTransactionRun()
        {
            DateTime dt = DateTime.UtcNow.ToChinaStandardTime();
            DateTime dtend = dt.AddMinutes(-30);
            //DateTime dtbegin = dt.AddHours(-24);
            DateTime dtbegin = dt.AddDays(-10);

            string strSqlWhere = $"LastEditTime>='{dtbegin.ToString("yyyy-MM-dd HH:mm:ss")}' and LastEditTime<='{dtend.ToString("yyyy-MM-dd HH:mm:ss")}' and ResultCode=0 and Status=0 and IsDel=0 ";
            IEnumerable<AccoutTransaction> list = SqlData(strSqlWhere);
            if (list?.Count() > 0)
            {
                foreach (var objAt in list)
                {
                    await this.EditAccoutTransaction(objAt.TransactionIdentifier, dt);
                }
            }
        }

        #endregion

        #region 借款户-SqlData
        /// <summary>
        /// 借款户-SqlData
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="strWhere">The string where.</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;Jinyinmao.Daemon.Services.AccoutTransaction&gt;.</returns>
        public IEnumerable<AccoutTransaction> SqlData(string strWhere = "1=1")
        {
            try
            {
                using (SqlConnection connection = GetOpenConnection(ebibpbCenterConnectionString))
                {
                    IEnumerable<AccoutTransaction> list = connection.Query<AccoutTransaction>($"select top 10000 TransactionIdentifier,Status from AccoutTransaction where {strWhere}");
                    return list;
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError("银行存管无响应流水制为失败：Error: {0}".FormatWith(e.Message), e);
            }
            return null;
        }
        #endregion

        #region 借款户修改状态
        /// <summary>
        /// 借款户修改状态
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="dt">The dt.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private async Task EditAccoutTransaction(string orderId, DateTime dt)
        {
            try
            {
                HttpResponseMessage responseMessage = await this.CallCenterClient.PostAsJsonAsync($"{gatewayHost}api/business/ordersearch", new { orderId = orderId });
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    OrderSearchResponse objOrder = await responseMessage.Content.ReadAsAsync<OrderSearchResponse>();
                    if (objOrder.RespCode == -1)
                    {
                        string sqlT = $"update AccoutTransaction set ResultTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',LastEditTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',ResultCode=-1 where TransactionIdentifier='{orderId}' ";
                        using (SqlConnection connection = GetOpenConnection(ebibpbCenterConnectionString))
                        {
                            connection.Execute(sqlT);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError("银行存管无响应流水制为失败-借款户/代偿户/个人融资户：Error: {0}".FormatWith(e.Message), e);
            }

        }
        #endregion

        #region 查询平台户的流水

        /// <summary>
        /// 查询借款户的流水
        /// </summary>
        private async Task SyncPlatformTranscationsRun()
        {
            DateTime dt = DateTime.UtcNow.ToChinaStandardTime();
            DateTime dtend = dt.AddMinutes(-30);
            //DateTime dtbegin = dt.AddHours(-24);
            DateTime dtbegin = dt.AddDays(-10);

            string strSqlWhere = $"LastEditTime>='{dtbegin.ToString("yyyy-MM-dd HH:mm:ss")}' and LastEditTime<='{dtend.ToString("yyyy-MM-dd HH:mm:ss")}' and ResultCode=0 and Status=0 and IsDel=0 ";
            IEnumerable<AccoutTransaction> list = SqlDataPlatformTranscations(strSqlWhere);
            if (list?.Count() > 0)
            {
                foreach (var objAt in list)
                {
                    await this.EditPlatformTranscations(objAt.TransactionIdentifier, dt);
                }
            }
        }

        #endregion

        #region 平台户-SqlData
        /// <summary>
        /// 借款户-SqlData
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="strWhere">The string where.</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;Jinyinmao.Daemon.Services.AccoutTransaction&gt;.</returns>
        public IEnumerable<AccoutTransaction> SqlDataPlatformTranscations(string strWhere = "1=1")
        {
            try
            {
                using (SqlConnection connection = GetOpenConnection(ebibpbCenterConnectionString))
                {
                    IEnumerable<AccoutTransaction> list = connection.Query<AccoutTransaction>($"select top 10000 TranscationId as TransactionIdentifier,Status from PlatformTranscations where {strWhere}");
                    return list;
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError("银行存管无响应流水制为失败：Error: {0}".FormatWith(e.Message), e);
            }
            return null;
        }
        #endregion

        #region 平台户修改状态
        /// <summary>
        /// 借款户修改状态
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="dt">The dt.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private async Task EditPlatformTranscations(string orderId, DateTime dt)
        {
            try
            {
                HttpResponseMessage responseMessage = await this.CallCenterClient.PostAsJsonAsync($"{gatewayHost}api/business/ordersearch", new { orderId = orderId });
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    OrderSearchResponse objOrder = await responseMessage.Content.ReadAsAsync<OrderSearchResponse>();
                    if (objOrder.RespCode == -1)
                    {
                        string sqlT = $"update PlatformTranscations set ResultTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',LastEditTime='{dt.ToString("yyyy-MM-dd HH:mm:ss")}',ResultCode=-1 where TranscationId='{orderId}' ";
                        using (SqlConnection connection = GetOpenConnection(ebibpbCenterConnectionString))
                        {
                            connection.Execute(sqlT);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError("银行存管无响应流水制为失败-平台户：Error: {0}".FormatWith(e.Message), e);
            }

        }
        #endregion


        #endregion 银行存管融资方流水和平台方无响应流水制为失败

        private HttpClient InitCallCenterHttpClient()
        {
            HttpClient httpClient = HttpClientHelper.InitHttpClient(gatewayHost);
            return httpClient;
        }

    }

    public class AccoutTransaction
    {
        public string TransactionIdentifier { get; set; }

        public string Status { get; set; }
    }

    /// <summary>
    /// 接口实体（根据某一商户订单号，查询存管系统对应的交易状态）
    /// </summary>
    public class OrderSearchResponse
    {
        /// <summary>
        /// 金额(单位分) ,
        /// </summary>
        /// <value>The amount.</value>
        public long? Amount { get; set; }
        /// <summary>
        /// 业务类型(1000-充值,2000-提现,3003-取消投资,3010-取消预约冻结,3011-预约冻结,3100-投资,5000-放款,7000-还款,8000-返利) ,
        /// </summary>
        /// <value>The type of the biz.</value>
        public string BizType { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        /// <value>The merchant identifier.</value>
        public string MerchantId { get; set; }
        /// <summary>
        /// 交易流水号 
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }
        /// <summary>
        /// 出款方
        /// </summary>
        /// <value>The pay user identifier.</value>
        public string PayUserId { get; set; }
        /// <summary>
        /// 收款方
        /// </summary>
        /// <value>The receive user identifier.</value>
        public string ReceiveUserId { get; set; }
        /// <summary>
        /// 状态(I-处理中, S-成功, F-失败) ,
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }
        /// <summary>
        /// 状态码(1-成功,0-处理中,-1-失败) ,
        /// </summary>
        /// <value>The resp code.</value>
        public int RespCode { get; set; }
        /// <summary>
        /// 返回处理信息
        /// </summary>
        /// <value>The resp MSG.</value>
        public string RespMsg { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>
        /// <value>The resp sub code.</value>
        public string RespSubCode { get; set; }
    }
}