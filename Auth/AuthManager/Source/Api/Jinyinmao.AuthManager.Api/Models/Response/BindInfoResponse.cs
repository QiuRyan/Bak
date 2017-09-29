using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class BindInfoResponse.
    /// </summary>
    public class BindInfoResponse
    {
        /// <summary>
        ///     微信绑定标识，true已绑定，false未绑定
        /// </summary>
        [Required]
        [JsonProperty("flag")]
        public int Flag { get; set; }

        /// <summary>
        ///     微信OpenId.
        /// </summary>
        [Required]
        [JsonProperty("openId")]
        public string OpenId { get; set; }

        /// <summary>
        ///     用户唯一标识.
        /// </summary>
        [Required]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }

    /// <summary>
    ///     Class BindInfoEx.
    /// </summary>
    internal static class BindInfoEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>BindInfoResponse.</returns>
        public static BindInfoResponse ToResponse(this BindInfo info)
        {
            return new BindInfoResponse
            {
                Flag = info.Flag,
                OpenId = info.OpenId ?? string.Empty,
                UserIdentifier = info.UserIdentifier ?? string.Empty
            };
        }
    }
}