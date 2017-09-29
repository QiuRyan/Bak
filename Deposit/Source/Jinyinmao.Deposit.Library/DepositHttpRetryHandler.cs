// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : DepositHttpRetryHandler.cs
// Created          : 2017-08-10  15:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:30
// ******************************************************************************************************
// <copyright file="DepositHttpRetryHandler.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;

namespace Jinyinmao.Deposit.Lib
{
    /// <summary>
    ///     Class DepositHttpRequestTransientErrorDetectionStrategy.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.ITransientErrorDetectionStrategy" />
    public class DepositHttpRequestTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
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
                return this.statusCodes.Contains(httpWebResponse.StatusCode);
            return false;
        }

        #endregion ITransientErrorDetectionStrategy Members
    }

    /// <summary>
    ///     Class DepositHttpRetryHandler.
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class DepositHttpRetryHandler : DelegatingHandler
    {
        private static readonly RetryPolicy retryPolicy = new RetryPolicy(new DepositHttpRequestTransientErrorDetectionStrategy(), 5, 3.Seconds());

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