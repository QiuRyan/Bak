using Jinyinmao.AuthManager.Domain.Core.Events;
using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Domain.Interface.Events
{
    public class UserResetLoginPassword : Event, IEvent
    {
        #region IEvent Members

        [JsonIgnore]
        public string EventName
        {
            get { return "jym-user-reset-login-password"; }
        }

        #endregion IEvent Members
    }
}