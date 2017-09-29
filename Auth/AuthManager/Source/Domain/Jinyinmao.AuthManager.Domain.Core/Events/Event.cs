using Moe.Lib;
using System;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Domain.Core.Events
{
    public abstract class Event
    {
        public Guid EventId { get; set; }

        public Dictionary<string, object> Payload { get; set; }

        public long Timestamp
        {
            get { return DateTime.UtcNow.UnixTimestamp(); }
        }
    }
}