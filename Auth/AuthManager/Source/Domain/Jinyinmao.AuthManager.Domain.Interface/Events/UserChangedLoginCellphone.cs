using Jinyinmao.AuthManager.Domain.Core.Events;
using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Domain.Interface.Events
{
    public class UserChangedLoginCellphone : Event, IEvent
    {
        #region IEvent Members

        [JsonIgnore]
        public string EventName
        {
            get { return "jym-user-changed-login-cellphone"; }
        }

        #endregion IEvent Members
    }
}