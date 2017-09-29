using System;
using System.Net.Http;
using System.Threading.Tasks;
using jinyinmao.MessageWorker.Config;
using Jinyinmao.Application.Constants;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.TransientFaultHandling;
using MoeLib.Diagnostics;

namespace Jinyinmao.MessageWorker.Domain.Bll.Impl
{
    /// <summary>
    ///     ClSmsService.
    /// </summary>
    public class ClSmsService : ISmsService
    {
        /// <summary>
        ///     The message template
        /// </summary>
        private static readonly string MessageTemplate = @"account={0}&pswd={1}&mobile={2}&msg={3}&needstatus=true";

        private static readonly string Password = ConfigsManager.ClSmsServicePassword;
        private static readonly bool SmsEnable = ConfigsManager.SmsEnable;
        private static readonly string UserName = ConfigsManager.ClSmsServiceUserName;
        private static readonly string ClSendMessageUrl = ConfigsManager.ClSendMessageUrl;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private readonly RetryPolicy retryPolicy = new RetryPolicy(new HttpRequestTransientErrorDetectionStrategy(), RetryStrategy.DefaultExponential);

        private readonly SmsServiceBase smsServiceBase;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClSmsService" /> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public ClSmsService(SmsChannel channel)
        {
            switch (channel)
            {
                case SmsChannel.YanZhengMa:
                    this.ProductId = "676767";
                    break;

                case SmsChannel.TongZhi:
                    this.ProductId = "48661";
                    break;

                default:
                    this.ProductId = "251503";
                    break;
            }
            this.smsServiceBase = new SmsServiceBase(this);
        }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        private ILogger Logger
        {
            get { return App.LogManager.CreateLogger(); }
        }

        #region ISmsService Members

        public string ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the SMS gateway.
        /// </summary>
        /// <value>The name of the SMS gateway.</value>
        public SmsGateway SmsGatewayName
        {
            get { return SmsGateway.ChuangLan; }
        }

        /// <summary>
        ///     send message as an asynchronous operation.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>Task.</returns>
        public async Task SendMessageAsync(string args, string cellphone, string message, string signature)
        {
            try
            {
                string responseMessage = string.Empty;
                if (SmsEnable)
                {
                    string requestUri = this.BuildRequestUri(cellphone, message);

                    using (HttpClient client = InitHttpClient())
                    {
#if DEBUG
                        HttpResponseMessage response = await client.GetAsync(requestUri);
#else
                        HttpResponseMessage response = await this.retryPolicy.ExecuteAsync(async () => await client.GetAsync(requestUri));
#endif

                        responseMessage = await response.Content.ReadAsStringAsync();
                    }
                }

                await this.smsServiceBase.StoreSmsMessageAsync(args, cellphone, message, responseMessage, this.SmsGatewayName);
            }
            catch (Exception e)
            {
                this.LogError(cellphone, message, e);
            }
        }

        #endregion ISmsService Members

        /// <summary>
        ///     Gets the send message result.
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private static bool GetSendMessageResult(string responseMessage)
        {
            if (responseMessage.IsNullOrEmpty())
            {
                return false;
            }

            string[] infos = responseMessage.Split(',');
            return infos.Length > 1 && infos[1].StartsWith("0", StringComparison.Ordinal);
        }

        /// <summary>
        ///     Initializes the HTTP client.
        /// </summary>
        /// <returns>HttpClient.</returns>
        private static HttpClient InitHttpClient()
        {
            return new HttpClient { Timeout = 30.Seconds() };
        }

        /// <summary>
        ///     Builds the request URI.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        private string BuildRequestUri(string cellphone, string message)
        {
            return ClSendMessageUrl + MessageTemplate.FormatWith(UserName, Password, cellphone, message);
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        private void LogError(string cellphone, string message, Exception exception)
        {
            string logMessage = $"Failed to send sms message. Cellphone:{cellphone}. Message:{message}.";
            this.Logger.Error(logMessage, "ClSmsService", 0UL, "", null, exception);
        }
    }
}