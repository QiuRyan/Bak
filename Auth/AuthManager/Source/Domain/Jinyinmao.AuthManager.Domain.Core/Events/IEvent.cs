using System;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Domain.Core.Events
{
    public interface IEvent
    {
        Guid EventId { get; set; }

        string EventName { get; }

        Dictionary<string, object> Payload { get; set; }
    }
}