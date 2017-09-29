// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  6:40 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  6:41 PM
// ***********************************************************************
// <copyright file="HttpTransientErrorDetectionStrategy.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Jinyinmao.Daemon.Utils
{
    /// <summary>
    ///     HttpTransientErrorDetectionStrategy.
    /// </summary>
    public class HttpTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        private readonly List<HttpStatusCode> statusCodes =
            new List<HttpStatusCode>
            {
                HttpStatusCode.GatewayTimeout,
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.ServiceUnavailable
            };

        #region ITransientErrorDetectionStrategy Members

        /// <summary>
        ///     Determines whether the specified exception represents a transient failure that can be compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>
        ///     true if the specified exception is considered as transient; otherwise, false.
        /// </returns>
        public bool IsTransient(Exception ex)
        {
            WebException we = ex as WebException;
            if (we == null)
                return false;

            HttpWebResponse response = we.Response as HttpWebResponse;

            bool isTransient = response != null
                               && this.statusCodes.Contains(response.StatusCode);
            return isTransient;
        }

        #endregion
    }
}