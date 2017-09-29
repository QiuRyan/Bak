using Microsoft.WindowsAzure.Storage.Table;

namespace Jinyinmao.Daemon.Models
{
    public class ConfigEntity : TableEntity
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this instance is work day.
        /// </summary>
        /// <value><c>true</c> if this instance is work day; otherwise, <c>false</c>.</value>
        public bool IsWorkday { get; set; }
    }
}