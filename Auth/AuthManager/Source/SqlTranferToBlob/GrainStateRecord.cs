namespace SqlTranferToBlob
{
    /// <summary>
    ///     Class GrainStateRecord.
    /// </summary>
    public class GrainStateRecord
    {
        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the long identifier.
        /// </summary>
        /// <value>The long identifier.</value>
        public long LongId { get; set; }

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public long TimeStamp { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
    }
}