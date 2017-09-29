using System;

namespace SqlTranferToBlob
{
    /// <summary>
    ///     ExtraInterestRecords.
    /// </summary>
    public class ExtraInterestRecord
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the operation identifier.
        /// </summary>
        /// <value>The operation identifier.</value>
        public Guid OperationId { get; set; }
    }
}