using Moe.Lib;
using MoeLib.Jinyinmao.Web.Diagnostics;
using System;
using System.Net.Http;

namespace Jinyinmao.AuthManager.Api.Helper
{
    internal static class RequestHelper
    {
        internal static Guid BuildCommandId(this HttpRequestMessage request)
        {
            return request.GetTraceEntry().RequestId.AsGuid("N", Guid.NewGuid());
        }
    }
}