using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    internal static class CheckCellphoneResponseEx
    {
        internal static CheckCellphoneResponse ToResponse(this CheckCellphoneResult result)
        {
            return new CheckCellphoneResponse
            {
                Result = result.Result
            };
        }
    }

    internal class CheckCellphoneResponse
    {
        /// <summary>
        ///     是否注册
        /// </summary>
        [JsonProperty("result")]
        [Required]
        public bool Result { get; set; }
    }
}