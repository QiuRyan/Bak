using System.ComponentModel.DataAnnotations;
using Jinyinmao.Application.ViewModel.MessageManager.Enum;
using Newtonsoft.Json;

namespace Jinyinmao.MessageManager.Api.Models
{
    /// <summary>
    ///     MessageManagerResponse.
    /// </summary>
    public class MessageManagerResponse
    {
        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Required]
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonIgnore]
        internal static MessageManagerResponse NoTemplates
        {
            get
            {
                return new MessageManagerResponse
                {
                    Status = (long)SendResultCode.NoTemplates
                };
            }
        }

        [JsonIgnore]
        internal static MessageManagerResponse Success
        {
            get
            {
                return new MessageManagerResponse
                {
                    Status = (long)SendResultCode.Success
                };
            }
        }
    }
}