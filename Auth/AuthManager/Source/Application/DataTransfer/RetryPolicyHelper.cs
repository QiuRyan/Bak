using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;

namespace DataTransfer
{
    /// <summary>
    ///     RetryPolicyFactory.
    /// </summary>
    internal static class RetryPolicyHelper
    {
        private static readonly Lazy<RetryPolicy> SqlDatabaseRetryPolicy = new Lazy<RetryPolicy>(
            () => new RetryPolicy(new SqlDatabaseTransientErrorDetectionStrategy(), new ExponentialBackoff(
                "SqlDatabase", 10, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1), true)));

        /// <summary>
        ///     Gets the SQL database retry policy.
        /// </summary>
        internal static RetryPolicy SqlDatabase => SqlDatabaseRetryPolicy.Value;
    }
}