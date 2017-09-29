using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Dtos
{
    [Immutable]
    public class CheckPasswordResult
    {
        public string Cellphone { get; set; }

        public int RemainCount { get; set; }

        public bool Success { get; set; }

        public bool UserExist { get; set; }

        public Guid UserId { get; set; }
    }
}