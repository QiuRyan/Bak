using Jinyinmao.AuthManager.Domain.Core.Events;
using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Domain.Interface.Events
{
    public class UserSetLoginPassword : Event, IEvent
    {
        #region IEvent Members

        [JsonIgnore]
        public string EventName
        {
            get { return "jym-user-set-login-password"; }
        }

        #endregion IEvent Members
    }
}