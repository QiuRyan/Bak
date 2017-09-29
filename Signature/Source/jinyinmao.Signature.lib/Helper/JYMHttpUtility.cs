using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;

namespace jinyinmao.Signature.lib.Helper
{
    public static class JYMHttpUtility
    {
        public static HttpClient InitHttpClient(string baseAddress)
        {
            List<DelegatingHandler> delegatingHandlers = new List<DelegatingHandler>
            {
                //new JinyinmaoHttpStatusHandler(),
                //new JinyinmaoLogHandler("HTTP Client Request", "HTTP Client Response"),
                new JYMSignatureHttpRetryHandler()
            };

            HttpClient client = HttpClientFactory.Create(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }, delegatingHandlers.ToArray());

            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.5));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.1));
            client.DefaultRequestHeaders.AcceptEncoding.Clear();
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip", 1.0));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate", 0.5));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("*", 0.1));
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            client.Timeout = 3.Minutes();

            return client;
        }
    }

    public class JYMSignatureHttpRequestTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        private readonly IList<HttpStatusCode> statusCodes = new List<HttpStatusCode>
        {
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.BadGateway,
            HttpStatusCode.BadRequest
        };

        #region ITransientErrorDetectionStrategy Members

        /// <summary>
        ///     Determines whether the specified exception represents a transient failure that can be compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>
        ///     <c>true</c> if the specified exception is considered as transient; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTransient(Exception ex)
        {
            WebException webException = ex as WebException;
            HttpWebResponse httpWebResponse = (webException != null ? webException.Response : (WebResponse)null) as HttpWebResponse;
            if (httpWebResponse != null)
            {
                return this.statusCodes.Contains(httpWebResponse.StatusCode);
            }
            return false;
        }

        #endregion ITransientErrorDetectionStrategy Members
    }

    public class JYMSignatureHttpRetryHandler : DelegatingHandler
    {
        private static readonly RetryPolicy retryPolicy = new RetryPolicy(new JYMSignatureHttpRequestTransientErrorDetectionStrategy(), 5, 3.Seconds());

        /// <summary>
        ///     Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <returns>
        ///     Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was null.</exception>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return retryPolicy.ExecuteAction(() => base.SendAsync(request, cancellationToken));
        }
    }
}